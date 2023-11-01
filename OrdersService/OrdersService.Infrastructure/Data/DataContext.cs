namespace OrdersService.Infrastructure.Data
{
    public class DataContext
    {
        public const string SectionName = "DataContext";

        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
