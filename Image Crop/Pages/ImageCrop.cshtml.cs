using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;

public class ImageCropModel : PageModel
{
    [BindProperty]
    public required IFormFile OriginalImage { get; set; }

    [BindProperty]
    public required IFormFile CroppedImage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostUploadAsync(IFormFile OriginalImage, IFormFile CroppedImage)
    {
        if (OriginalImage == null || CroppedImage == null)
        {
            return BadRequest(new { message = "Both images must be provided." });
        }

        try
        {
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(Path.Combine(uploadsFolderPath, "original"));
            Directory.CreateDirectory(Path.Combine(uploadsFolderPath, "cropped"));

            var originalFilePath = Path.Combine(uploadsFolderPath, "original", OriginalImage.FileName);
            using (var stream = new FileStream(originalFilePath, FileMode.Create))
            {
                await OriginalImage.CopyToAsync(stream);
            }

            var croppedFilePath = Path.Combine(uploadsFolderPath, "cropped", CroppedImage.FileName);
            using (var stream = new FileStream(croppedFilePath, FileMode.Create))
            {
                await CroppedImage.CopyToAsync(stream);
            }

            return new JsonResult(new { message = "Upload successful" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }



}
