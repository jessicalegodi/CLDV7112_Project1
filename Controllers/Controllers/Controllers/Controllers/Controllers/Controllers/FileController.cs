using CLDV7112_Project1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLDV7112_Project1.Controllers
{
    public class FileController : Controller
    {
        private readonly AzureStorageService _storage;

        public FileController(AzureStorageService storage)
        {
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var files = await _storage.GetFileNamesAsync();

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

            await _storage.UploadFileAsync(
                file.OpenReadStream(),
                file.FileName);

            TempData["Message"] =
                "Log file uploaded successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
