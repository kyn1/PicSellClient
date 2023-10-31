using System.ComponentModel.DataAnnotations;

namespace PicSell.Models
{
	public class UploadPicturesRequest
	{
		[Required(ErrorMessage = "FolderName is required.")]
		[MaxLength(255, ErrorMessage = "FolderName cannot exceed 255 characters.")]
		public string? FolderName { get; set; }
		[Required(ErrorMessage = "No files provided for upload.")]
		public List<IFormFile>? Files { get; set; }
	} 
}
