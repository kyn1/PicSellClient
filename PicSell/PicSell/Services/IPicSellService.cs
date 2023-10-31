using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.JsonPatch;
using PicSell.Models;

namespace PicSell.Services
{
	public interface IPicSellService
	{
		IEnumerable<Picture> GetPictures();
		Picture GetPicture(int id);

		Task CreatePictureAsync(Picture picture);
		Task UpdatePictureAsync(int pictureId, Picture picture);
		Task DeletePictureAsync(int id);

		Task<UploadResult> UploadPictureAsync(IFormFile Picturefile, string folderName);
		Task<IEnumerable<UploadResult>> UploadPicturesAsync(IEnumerable<IFormFile> Picturefiles, string folderName);
		Task UpdatePartial(int id, JsonPatchDocument<Picture> patchDocument);
	}
}
