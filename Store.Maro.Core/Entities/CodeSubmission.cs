using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Entities
{
    public class CodeSubmission
    {
        public string sourceCode { get; set; }
        public string languageId { get; set; }
        public string input { get; set; }
    }
}
