namespace StingyJunk.IO.Odd
{
    public class Credentials
    {
        public Credentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}