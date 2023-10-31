using Microsoft.AspNetCore.JsonPatch;
using PicSell.Models;

namespace PicSell.Services
{
	public interface IOrderService
	{
		Order GetOrderById(int orderId);
		IEnumerable<Order> GetOrders();
		Task CreateOrderAsync(Order order);
		Task UpdateOrderAsync(int id, Order order);
		Task DeleteOrderAsync(int orderId);
	}
}
