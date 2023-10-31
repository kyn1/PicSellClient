using PicSell.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PicSell.Services;
using System.Xml.XPath;
using Microsoft.AspNetCore.JsonPatch;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Azure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Buffers.Text;
using System;
using System.ComponentModel.DataAnnotations;

namespace PicSell.Controllers
{
	

	[Route("api/pictures")]
	[ApiController]
	public class PicSell : ControllerBase
	{
		private readonly IPicSellService _picSellService;


		public PicSell(IPicSellService picSellService)
		{
			_picSellService = picSellService;
		}
		// GET: api/pictures
		[HttpGet]
		public IActionResult GetPictures()
		{
			// Implement code to retrieve a list of pictures from your data source.
			var pictures = _picSellService.GetPictures();
			return Ok(pictures);
			// Example: return a list of Picture objects.
		}

		// GET: api/pictures/{id}
		[HttpGet("{id}")]
		public IActionResult GetPicture(int id)
		{
			// Implement code to retrieve a picture by its ID from your data source.
			var picture = _picSellService.GetPicture(id);
			// Example: return a Picture object or NotFound if not found.
			if (picture == null)
			{
				return NotFound();
			}

			return Ok(picture);
		}

		// POST: api/pictures
		[HttpPost]
		[Route("Create")]
		public async Task<IActionResult> CreatePicture([FromBody] Picture picture)
		{
			// Implement code to create a new picture and save it to your data source.
			try {
				await _picSellService.CreatePictureAsync(picture);
				return Ok(picture);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
		
			// Example: return the created Picture object with the ID.
		}

		// PUT: api/pictures/{id}
		[HttpPut("{pictureId:int}")]
		public async Task<IActionResult> UpdatePicture(int pictureId,[FromBody] Picture picture)
		{
			// Implement code to update an existing picture with the given ID.
			try
			{
				picture.PictureId = pictureId;
				await _picSellService.UpdatePictureAsync(pictureId, picture);
				return Ok(picture);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
			// Example: return NoContent() or NotFound() if the picture doesn't exist.
		}

		// DELETE: api/pictures/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePicture(int id)
		{
			// Implement code to delete a picture with the given ID.
			try
			{
				await _picSellService.DeletePictureAsync(id);
				return NoContent();
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
			// Example: return NoContent() or NotFound() if the picture doesn't exist.
		}

		
		[HttpPost("upload/{folderName}")]
		public async Task<IActionResult> UploadPicture([FromRoute] string folderName, IFormFile Picturefile)
		{
			try
			{
				var uploadResult = await _picSellService.UploadPictureAsync(Picturefile, folderName);

				if (uploadResult.PublicId != null)
				{
					// You can save the uploadResult information in your database or use it as needed.
					return Ok(new { publicId = uploadResult.PublicId, url = uploadResult.Url });
				}
				else
				{
					return BadRequest("Invalid upload result. PublicId is null.");
				}
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error uploading image: {ex.Message}");
			}
		}

		[HttpPost("upload")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UploadPictures([FromForm] UploadPicturesRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.FolderName))
			{
				return BadRequest("FolderName is required.");
			}

			if (request.Files == null || !request.Files.Any())
			{
				return BadRequest("No files provided for upload.");
			}

			try
			{
				var uploadResults = await _picSellService.UploadPicturesAsync(request.Files, request.FolderName);

				if (uploadResults.Any(result => result.PublicId == null))
				{
					return BadRequest("Some files could not be uploaded.");
				}

				var response = uploadResults.Select(result => new { publicId = result.PublicId, url = result.Url });
				return Ok(response);
			}
			catch (Exception ex)
			{

				return StatusCode(500, $"Error uploading images: {ex.Message}");
			}
		}


		[HttpPatch]
		[Route("{id:int}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdatePartial(int id, [FromBody] JsonPatchDocument<Picture> patchDocument)
		{
			try
			{
				await _picSellService.UpdatePartial(id, patchDocument);
				return Ok(patchDocument);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
		}









	}

}
