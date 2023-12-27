using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UploadImageController : ControllerBase
	{
		private readonly IWebHostEnvironment _env;
		public UploadImageController(IWebHostEnvironment env)
		{
			_env = env;
		}
		[HttpPost("SaveFile")]
		public async Task<IActionResult> SaveFile(IFormFile formFile, string id)
		{
			string filename = formFile.FileName;
			var physicalPath = _env.ContentRootPath + "/Photos/" + id + "/" + filename;
			using (var stream = new FileStream(physicalPath, FileMode.Create))
			{
				formFile.CopyTo(stream);
			}
			string imageUrl = "https://localhost:7117/Photos/" + id + "/" + filename;
			return Ok(imageUrl);
		}
	}
}
