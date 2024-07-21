namespace util.API.Utility.Configuration
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string LedgerCollection { get; set; } = null!;
    }
}
