namespace CatalogOnline2.Models
{
    public class NewUser
    {
        public int ID { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public int anNastere { get; set; }
        public string email { get; set; } = "x";
        public string user_type { get; set; } = "x";
    }
}
