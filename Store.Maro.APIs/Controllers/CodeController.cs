using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Store.Maro.Core.Dtos.Codes;
using Store.Maro.Core.Entities;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Maro.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private const string JudgeApiHost = "judge0-ce.p.rapidapi.com";
        private const string BaseUrl = "https://judge0-ce.p.rapidapi.com/submissions";

        public CodeController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClientFactory.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }


        [HttpPost("run")]
        public async Task<IActionResult> RunCode([FromBody]CodeSubmission submission)
        {

            if (submission is null) return BadRequest("Submission cannot be null");


            var sourceCodeBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(submission.sourceCode));
            var stdinBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(submission.input ?? ""));
            // store the encoded code, and input
            var requestBody = new
            {
                language_id = submission.languageId,
                source_code = sourceCodeBase64,
                stdin = stdinBase64
            };

            // request configurations
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://judge0-ce.p.rapidapi.com/submissions?base64_encoded=true&wait=true&fields=*");

            request.Headers.Add("x-rapidapi-key", _configuration["Judge0:ApiKey"]);
            request.Headers.Add("x-rapidapi-host", "judge0-ce.p.rapidapi.com");
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            //sending
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //get the response
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseBody);


            json.RootElement.TryGetProperty("stdout", out JsonElement stdoutElement);

            var result = Encoding.UTF8.GetString(Convert.FromBase64String(stdoutElement.GetString()));

            json.RootElement.TryGetProperty("memory", out JsonElement memory);
            json.RootElement.TryGetProperty("time", out JsonElement time);
            json.RootElement.TryGetProperty("token", out JsonElement token);
            json.RootElement.TryGetProperty("language_id", out JsonElement languageId);



            return Ok(new CompilationToReturnDto
            {
                Result = result,
                Memory = memory.ToString(),
                Time = time.ToString(),
                LanguageId = languageId.ToString(),
                Token = token.ToString()
            });



        }

        /// <summary>
        /// Retrieves the result of a previously submitted code execution
        /// </summary>
        /// <param name="token">Submission token</param>
        /// <returns>Execution result</returns>
        [HttpGet("result/{token}")]
        public async Task<IActionResult> GetResult(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token cannot be null or empty");
            }

            return await GetResultInternal(token);
        }

        /// <summary>
        /// Internal method to retrieve execution results that can be called by other methods
        /// </summary>
        /// <param name="token">Submission token</param>
        /// <returns>Execution result</returns>
        private async Task<IActionResult> GetResultInternal(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token cannot be null or empty");
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"{BaseUrl}/{token}?base64_encoded=true&fields=*");

                AddApiHeaders(request);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(responseBody);

                if (json.RootElement.TryGetProperty("stdout", out JsonElement stdoutElement) && !stdoutElement.ValueKind.Equals(JsonValueKind.Null))
                {
                    var result = Encoding.UTF8.GetString(Convert.FromBase64String(stdoutElement.GetString() ?? ""));
                    return Ok(new { result });
                }

                // If execution is still in progress or has no output
                return Ok(new { result = string.Empty });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                    new { message = "Error communicating with Judge0 API", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred", error = ex.Message });
            }
        }

        #region Private Helper Methods

        private object CreateSubmissionRequest(CodeSubmission submission)
        {
            var sourceCodeBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(submission.sourceCode));
            var stdinBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(submission.input ?? string.Empty));

            return new
            {
                language_id = submission.languageId,
                source_code = sourceCodeBase64,
                stdin = stdinBase64
            };
        }

        private void AddApiHeaders(HttpRequestMessage request)
        {
            string apiKey = _configuration["Judge0:ApiKey"] ??
                throw new InvalidOperationException("Judge0 API key not configured");

            request.Headers.Add("x-rapidapi-key", apiKey);
            request.Headers.Add("x-rapidapi-host", JudgeApiHost);
        }

        #endregion
    }
}