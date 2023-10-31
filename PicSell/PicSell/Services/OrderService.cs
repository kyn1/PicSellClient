using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicSell.Data;
using PicSell.Models;
using System;

namespace PicSell.Services
{
	public class OrderService : IOrderService
	{
		private readonly ApiContext _context;

		public OrderService(ApiContext context)
		{
			_context = context;
		}


			
		public async Task CreateOrderAsync(Order order)
		{
			

			if (order.OrderId == 0)
			{
				_context.Orders.Add(order);
			}
			else
			{
				var existingOrder = _context.Orders.Find(order.OrderId);
				if (existingOrder == null)
				{
					throw new DirectoryNotFoundException("order not found");
				}

			}
			await _context.SaveChangesAsync();
		}


		public async Task UpdateOrderAsync(int id, Order order)
		{
			var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

			if (existingOrder == null)
			{
				throw new NotImplementedException("order not found"); 
			}

			existingOrder.CustomerId = order.CustomerId;
			existingOrder.PictureId = order.PictureId;
			existingOrder.OrderDate = order.OrderDate;

			await _context.SaveChangesAsync();
		}

		public async Task DeleteOrderAsync(int orderId)
		{
			var order =  await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

			if (order == null)
			{
				throw new NotImplementedException("order not found");
			}

			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
		}

		public Order GetOrderById(int orderId)
		{
			var result =  _context.Orders.SingleOrDefault(o => o.OrderId == orderId);
			if (result == null)
			{
				throw new NotImplementedException("order not foound");

			}
			return result;
		}



		public IEnumerable<Order> GetOrders()
		{
			return _context.Orders.ToList();
		}
	}
}
