using CustomerSupport.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.DataAccess
{
	public interface ITicketDataAccess
	{
		public List<CustomerSupportItem> GetAllTickets();
		public bool InsertTicket(CustomerSupportItem ticket);
		bool DeleteTicketById(int id);
		CustomerSupportItem GetTicketById(int id);
		bool UpdateTicket(CustomerSupportItem ticket);
	}
}
