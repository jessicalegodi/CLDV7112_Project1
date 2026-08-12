using CLDV7112_Project1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLDV7112_Project1.Controllers
{
    public class QueueController : Controller
    {
        private readonly AzureStorageService _storage;

        public QueueController(AzureStorageService storage)
        {
            _storage = storage;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await _storage.AddQueueMessageAsync(message);

                TempData["Message"] =
                    "Order message added successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
