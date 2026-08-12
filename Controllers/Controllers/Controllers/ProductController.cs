using CLDV7112_Project1.Models;
using CLDV7112_Project1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLDV7112_Project1.Controllers
{
    public class ProductController : Controller
    {
        private readonly AzureStorageService _storage;

        public ProductController(AzureStorageService storage)
        {
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _storage.GetProductsAsync();

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.PartitionKey = "Products";
            product.RowKey = Guid.NewGuid().ToString();

            await _storage.AddProductAsync(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
