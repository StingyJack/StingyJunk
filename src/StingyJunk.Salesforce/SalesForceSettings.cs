namespace StingyJunk.Salesforce
{
    public class SalesForceSettings
    {
        public string SecurityToken { get; set; }

        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsSandboxUser { get; set; }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SecurityToken) == false
                    && string.IsNullOrWhiteSpace(ConsumerKey) == false
                    && string.IsNullOrWhiteSpace(ConsumerSecret) == false
                    && string.IsNullOrWhiteSpace(UserName) == false
                    && string.IsNullOrWhiteSpace(Password) == false)
                {
                    return true;
                }
                return false;
            }
        }
    }
}