using CLDV7112_Project1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLDV7112_Project1.Controllers
{
    public class BlobController : Controller
    {
        private readonly AzureStorageService _storage;

        public BlobController(AzureStorageService storage)
        {
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var files = await _storage.GetBlobNamesAsync();

            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Message"] = "Please select a file.";

                return RedirectToAction(nameof(Index));
            }

            await _storage.UploadBlobAsync(
                file.OpenReadStream(),
                file.FileName);

            TempData["Message"] =
                "Image uploaded successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
