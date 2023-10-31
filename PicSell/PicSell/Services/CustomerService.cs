using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicSell.Controllers;
using PicSell.Data;
using PicSell.Models;

namespace PicSell.Services
{
	//new branch
	public class CustomerService : ICustomerService
	{
		readonly ApiContext _customerService;

		public CustomerService(ApiContext customerService)
		{
			_customerService = customerService;
		}

		public IEnumerable<Customer> GetCustomers()
		{
			return _customerService.Customers.ToList();
		}

		public Customer GetCustomerById(int id)
		{
			var result = _customerService.Customers.Find(id);
			if (result == null)
			{
				throw new NotFoundException("Customer not found");
			}

			return result;
		}

		public async Task CreateCustomerAsync([FromBody] Customer customer)
		{
			if (customer.CustomerId == 0)
			{
				_customerService.Customers.Add(customer);
			}
			else
			{
				var existingCustomer = _customerService.Customers.Find(customer.CustomerId);
				if (existingCustomer == null)
				{
					throw new DirectoryNotFoundException("Customer not found");
				}

				existingCustomer.FirstName = customer.FirstName;
				existingCustomer.LastName = customer.LastName;
			}
			await _customerService.SaveChangesAsync();
		}


		public async Task UpdateCustomerAsync(int customerId,Customer customer)
		{
			var existingRecord = await _customerService.Customers.FindAsync(customer.CustomerId);

			if (existingRecord == null)
			{
				throw new NotFoundException("Customer not found");
			}

			// Update the properties of the existing record with values from the updatedModel

			existingRecord.FirstName = customer.FirstName;
			existingRecord.LastName = customer.LastName;
			existingRecord.Email = customer.Email;
			existingRecord.Phone = customer.Phone;
			// Add more properties as needed

			_customerService.Customers.Update(existingRecord);
			await _customerService.SaveChangesAsync();
		}



		public async Task DeleteCustomerAsync([FromRoute] int id)
		{
			var booking = await _customerService.Customers.FindAsync(id);
			if (booking == null)
			{
				throw new NotFoundException("Customer not found");
			}

			_customerService.Customers.Remove(booking);
			await _customerService.SaveChangesAsync();
		}


		



	}
}

