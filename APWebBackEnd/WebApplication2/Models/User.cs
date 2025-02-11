namespace APWeb.Models
{
    public class User
    {

        public string Name { get; set; }

        public int Store { get; set; }

        public string Role { get; set; }

        public int ID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
