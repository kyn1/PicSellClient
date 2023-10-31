using Microsoft.AspNetCore.Mvc;
using PicSell.Models;
using PicSell.Services;

namespace PicSell.Controllers
{
	[Route("api/Order")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet]
		public IActionResult GetOrders()
		{
			var orders = _orderService.GetOrders();
			return Ok(orders);
		}

		[HttpGet("{id}")]
		public IActionResult GetOrder(int id)
		{
			var order = _orderService.GetOrderById(id);

			if (order == null)
			{
				return NotFound();
			}

			return Ok(order);
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(Order order)
		{
			try
			{
				await _orderService.CreateOrderAsync(order);
				return Ok(order);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateOrder(int id, Order order)
		{
			try
			{
				await _orderService.UpdateOrderAsync(id, order);
				return Ok(order);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
			
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			try
			{
				await _orderService.DeleteOrderAsync(id);
				return NoContent();
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
		}
	}
	
}
