using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using CLDV7112_Project1.Models;

namespace CLDV7112_Project1.Services
{
    public class AzureStorageService
    {
        private readonly TableClient _customerTable;
        private readonly TableClient _productTable;
        private readonly BlobContainerClient _blobContainer;
        private readonly QueueClient _queueClient;
        private readonly ShareClient _fileShare;

        public AzureStorageService(IConfiguration configuration)
        {
            string connectionString =
                configuration.GetConnectionString("AzureStorage")
                ?? throw new InvalidOperationException(
                    "Azure Storage connection string not configured.");

            _customerTable =
                new TableClient(connectionString, "Customers");

            _productTable =
                new TableClient(connectionString, "Products");

            _blobContainer =
                new BlobContainerClient(
                    connectionString,
                    "product-images");

            _queueClient =
                new QueueClient(
                    connectionString,
                    "order-processing");

            _fileShare =
                new ShareClient(
                    connectionString,
                    "logs");
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _customerTable.CreateIfNotExistsAsync();
            await _customerTable.AddEntityAsync(customer);
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            await _customerTable.CreateIfNotExistsAsync();

            var customers = new List<Customer>();

            await foreach (Customer customer
                in _customerTable.QueryAsync<Customer>())
            {
                customers.Add(customer);
            }

            return customers;
        }

        public async Task AddProductAsync(Product product)
        {
            await _productTable.CreateIfNotExistsAsync();
            await _productTable.AddEntityAsync(product);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            await _productTable.CreateIfNotExistsAsync();

            var products = new List<Product>();

            await foreach (Product product
                in _productTable.QueryAsync<Product>())
            {
                products.Add(product);
            }

            return products;
        }

        public async Task UploadBlobAsync(
            Stream stream,
            string fileName)
        {
            await _blobContainer.CreateIfNotExistsAsync();

            BlobClient blob =
                _blobContainer.GetBlobClient(fileName);

            await blob.UploadAsync(stream, true);
        }

        public async Task<List<string>> GetBlobNamesAsync()
        {
            await _blobContainer.CreateIfNotExistsAsync();

            var files = new List<string>();

            await foreach (var blob
                in _blobContainer.GetBlobsAsync())
            {
                files.Add(blob.Name);
            }

            return files;
        }

        public async Task AddQueueMessageAsync(string message)
        {
            await _queueClient.CreateIfNotExistsAsync();

            await _queueClient.SendMessageAsync(message);
        }

        public async Task UploadFileAsync(
            Stream stream,
            string fileName)
        {
            await _fileShare.CreateIfNotExistsAsync();

            ShareDirectoryClient directory =
                _fileShare.GetRootDirectoryClient();

            ShareFileClient file =
                directory.GetFileClient(fileName);

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            await file.CreateAsync(stream.Length);

            await file.UploadRangeAsync(
                new Azure.HttpRange(0, stream.Length),
                stream);
        }

        public async Task<List<string>> GetFileNamesAsync()
        {
            await _fileShare.CreateIfNotExistsAsync();

            ShareDirectoryClient directory =
                _fileShare.GetRootDirectoryClient();

            var files = new List<string>();

            await foreach (var item
                in directory.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory)
                {
                    files.Add(item.Name);
                }
            }

            return files;
        }
    }
}
