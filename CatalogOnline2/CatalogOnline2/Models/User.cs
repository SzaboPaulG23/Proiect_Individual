namespace CatalogOnline2.Models
{
    public class User
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; } = "x";
        public string user_type { get; set; } = "x";
    }
}
