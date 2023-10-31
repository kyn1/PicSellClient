using Microsoft.EntityFrameworkCore;
using PicSell.Models;

namespace PicSell.Data
{
	public class ApiContext : DbContext
	{
		public DbSet<Picture> Pictures { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }

		public ApiContext(DbContextOptions<ApiContext> options)
			: base(options)
		{ 

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
		}
	}
}
