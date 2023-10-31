using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicSell.Models;
using PicSell.Services;
using System;

namespace PicSell.Controllers
{
	[Route("api/customer")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private readonly ICustomerService _customerService;

		public CustomerController(ICustomerService customer)
		{
			_customerService = customer;
		}

		// GET: api/PicSell
		[HttpGet]
		public IActionResult GetCustomers()
		{
			var customer = _customerService.GetCustomers();
			return Ok(customer);
		}


		// GET: api/PicSell/5
		[HttpGet("{id}")]
		public ActionResult Get(int id)
		{
			var customer =  _customerService.GetCustomerById(id);

			if (customer == null)
			{
				return NotFound();
			}

			return Ok(customer);
		}

		// POST: api/PicSell
		[HttpPost]
		public async Task<IActionResult> PostCustomer(Customer customer)
		{
			try
			{
				await _customerService.CreateCustomerAsync(customer);
				return Ok(customer);
			}catch (NotFoundException)
			{
				return NotFound();
			}
		}

		[HttpPut]
		[Route("Update/{customerId:int}")]
		public async Task<IActionResult> Update(int customerId, [FromBody] Customer customr)
		{
			// Implement code to update an existing picture with the given ID.
			try
			{
				customr.CustomerId = customerId;
				await _customerService.UpdateCustomerAsync(customerId,customr);
				return Ok(customr);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
			// Example: return NoContent() or NotFound() if the picture doesn't exist.
		}

		// PUT: api/PicSell/5
		

		// DELETE: api/PicSell/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
	
			try
			{
				await _customerService.DeleteCustomerAsync(id);
				return NoContent();
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
		}
	}
}
