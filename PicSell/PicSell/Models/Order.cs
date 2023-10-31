using System;
using System.ComponentModel.DataAnnotations;

namespace PicSell.Models
{
	public class Order
	{
		public int OrderId { get; set; }

		public int CustomerId { get; set; }
		public int PictureId { get; set; }

		public DateTime OrderDate { get; set; }
		
	}

}
