using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Uploader.Models;

namespace Uploader.Controllers
{
	[Route("api/[controller]")]
	public class UpController : Controller
	{
		[HttpPost]
		public async Task<IActionResult> Upload()
		{
			if (ModelState.IsValid)
			{

				var files = HttpContext.Request.Form.Files;
				
				if (files.Any())
				{
					foreach (IFormFile file in files)
					{
						var bytes = await GetBytes(file);

						// Saving file to just for demo purpose
						await SaveToDisk(file, bytes); 
					}
				}

				var model = JsonConvert.DeserializeObject<UploadViewModel>(HttpContext.Request.Form["infos"].First());

				return Ok(new { model.Name, Success = true });
			}

			return BadRequest(new { ModelValid = false });
		}

		private static async Task<byte[]> GetBytes(IFormFile file)
		{
			using (Stream memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream);
				memoryStream.Seek(0, SeekOrigin.Begin);
				var bytes = new byte[memoryStream.Length];
				await memoryStream.ReadAsync(bytes, 0, bytes.Length);
				return bytes;
			}
		}

		private static async Task SaveToDisk(IFormFile file, byte[] bytes)
		{
			var filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"uploads\{file.FileName}");

			using (Stream stream = new FileStream(filepath, FileMode.OpenOrCreate))
			{
				await stream.WriteAsync(bytes, 0, (int) bytes.Length);
			}
		}
		
	}
}
