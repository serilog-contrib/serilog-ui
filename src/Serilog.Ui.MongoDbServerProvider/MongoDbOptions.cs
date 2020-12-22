namespace Serilog.Ui.MongoDbServerProvider
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }
    }
}