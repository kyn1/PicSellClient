using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PicSell.Models
{
	public class Picture
	{
		public int PictureId { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public decimal Price { get; set; }

		[NotMapped]
		public IFormFile? PictureFile { get; set; }

	}
}
