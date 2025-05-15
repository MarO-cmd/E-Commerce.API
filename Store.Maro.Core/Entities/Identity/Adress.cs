namespace Store.Maro.Core.Entities.Identity
{
    public class Adress
    {
        public int Id { get; set; }

        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Country { get; set; }
        public string AppUserId { get; set; }  // fk 
        public AppUser AppUser { get; set; }


    }
}