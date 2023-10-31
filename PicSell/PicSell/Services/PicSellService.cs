using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PicSell.Data;
using PicSell.Models;
using System.Runtime.Serialization;

namespace PicSell.Services
{
	public class PicSellService : IPicSellService
	{
		private readonly ApiContext _context;
		private readonly Cloudinary _cloudinary;
		public PicSellService(ApiContext context, Cloudinary cloudinary)
		{
			_context = context;
			_cloudinary = cloudinary;
		}
		

		
		public async Task CreatePictureAsync([FromBody] Picture picture)
		{
			if(picture.PictureId == 0)
			{
				_context.Pictures.Add(picture);
			}
			else
			{
				var existnPicture = _context.Pictures.Find(picture.PictureId);
				if (existnPicture == null)
				{
					throw new DirectoryNotFoundException("Picture not found");
				}

				existnPicture.PictureId = picture.PictureId;

			}
			await _context.SaveChangesAsync();
		}

		public async Task DeletePictureAsync([FromRoute] int id)
		{
			var picture = await _context.Pictures.FindAsync(id);
			if(picture == null)
			{
				throw new DirectoryNotFoundException("Picture not found");
			}

			_context.Pictures.Remove(picture);
			await _context.SaveChangesAsync();
		}

		public Picture GetPicture(int id)
		{
			var result = _context.Pictures.Find(id);
            if ( result == null)
            {
				throw new NotImplementedException("Picture not found");

			}
			return result;
        }

		public IEnumerable<Picture> GetPictures()
		{
			return _context.Pictures.ToList();
		}

		public async Task UpdatePartial(int id, JsonPatchDocument<Picture> patchDocument)
		{
			if (patchDocument == null || id <= 0)
			{
				throw new DirectoryNotFoundException("Picture not found");
			}
			var existnRec = _context.Pictures.FirstOrDefault(s => s.PictureId == id);
			if(existnRec == null)
			{
				throw new DirectoryNotFoundException("Picture not found");
			}

			var newPic = new Picture
			{
				PictureId = existnRec.PictureId,
				Title = existnRec.Title,
				Description = existnRec.Description,
				ImageUrl = existnRec.ImageUrl,
				Price = existnRec.Price,
				PictureFile = existnRec.PictureFile,


			};

			patchDocument.ApplyTo(newPic);

			existnRec.PictureId = newPic.PictureId;
			existnRec.Price = newPic.Price;
			existnRec.Title = newPic.Title;
			existnRec.ImageUrl = newPic.ImageUrl;
			existnRec.Description = newPic.Description;
			existnRec.PictureFile = newPic.PictureFile;

			await _context.SaveChangesAsync();
		}

		public async Task UpdatePictureAsync(int pictureId,Picture picture)
		{
			var existnRec = await _context.Pictures.FindAsync(picture.PictureId);
			if (existnRec == null)
			{
				throw new DirectoryNotFoundException("Picture not found");
			}

			existnRec.Title = picture.Title;
			existnRec.PictureId = picture.PictureId;
			existnRec.Description = picture.Description;
			existnRec.ImageUrl = picture.ImageUrl;
			existnRec.Price = picture.Price;
			existnRec.PictureFile = picture.PictureFile;

			_context.Pictures.Update(existnRec);
			await _context.SaveChangesAsync();

		}
		//single uploads
		public async Task<UploadResult> UploadPictureAsync(IFormFile Picturefile, string folderName)
		{
			if (Picturefile == null || Picturefile.Length == 0)
			{
				throw new ArgumentException("invalidFile");
			}

			using (var memoryStream = new MemoryStream())
			{
				// Copy the IFormFile stream to the MemoryStream
				await Picturefile.CopyToAsync(memoryStream);

				// Reset the stream position to the beginning
				memoryStream.Seek(0, SeekOrigin.Begin);

				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(Picturefile.FileName, memoryStream),
					Transformation = new Transformation().Width(500).Height(500).Crop("fill"),
					Folder = folderName
				};

				return await _cloudinary.UploadAsync(uploadParams);
			}
		}



		//Multiple uploads
		public async Task<IEnumerable<UploadResult>> UploadPicturesAsync(IEnumerable<IFormFile> Picturefiles, string folderName)
		{
			var tasks = new List<Task<UploadResult>>();

			foreach (var Picturefile in Picturefiles)
			{
				if (Picturefile != null && Picturefile.Length > 0)
				{
					tasks.Add(UploadPictureAsync(Picturefile, folderName));
				}
			}

			var uploadResults = await Task.WhenAll(tasks);

			return uploadResults;
		}



	}

	[Serializable]
	internal class NotFoundException : Exception
	{
		public NotFoundException()
		{
		}

		public NotFoundException(string? message) : base(message)
		{
		}

		public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
