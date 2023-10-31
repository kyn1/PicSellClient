using PicSell.Models;

namespace PicSell.Services
{
	public interface ICustomerService
	{
		Customer GetCustomerById(int customerId);
		IEnumerable<Customer> GetCustomers();
		Task CreateCustomerAsync(Customer customer);
		Task UpdateCustomerAsync(int customerId, Customer customer);
		Task DeleteCustomerAsync(int customerId);
	}
}
