namespace ELOTEC.Infrastructure.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenLife { get; set; }
        public int NoOfEmailTry { get; set; }
        public string ConnectionString { get; set; }
    }
}
