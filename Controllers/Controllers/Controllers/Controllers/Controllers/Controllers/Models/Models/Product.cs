using Azure;
using Azure.Data.Tables;

namespace CLDV7112_Project1.Models
{
    public class Product : ITableEntity
    {
        public string PartitionKey { get; set; } = "Products";

        public string RowKey { get; set; } =
            Guid.NewGuid().ToString();

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        public string ProductName { get; set; } = "";

        public double Price { get; set; }

        public int StockQuantity { get; set; }
    }
}
