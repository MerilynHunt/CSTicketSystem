using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CustomerSupport.Models;
using CustomerSupport.Models.ViewModels;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Data.Sqlite;
using System.Net.Sockets;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using CustomerSupport.DataAccess;

namespace CustomerSupport.Controllers
{

    public class HomeController : Controller, IControllers
    {
        private readonly ILogger<HomeController> _logger;
		private readonly ITicketDataAccess _ticketDataAccess;

		public HomeController(ILogger<HomeController> logger, ITicketDataAccess ticketDataAccess)
        {
            _logger = logger;
			_ticketDataAccess = ticketDataAccess;
		}

        public IActionResult Index()
        {
			/*var CustomerSupportViewModel = GetTickets();
            return View(CustomerSupportViewModel);*/
			var tickets = _ticketDataAccess.GetAllTickets();
			var viewModel = new CustomerSupportViewModel { TicketList = tickets };
			return View(viewModel);
		}

        public CustomerSupportViewModel GetTickets()
        {
			var tickets = _ticketDataAccess.GetAllTickets();
			return new CustomerSupportViewModel { TicketList = tickets };

		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public RedirectResult Insert(CustomerSupportItem ticket)
        {
            _ticketDataAccess.InsertTicket(ticket);
            return Redirect("/Home");
        }

		[HttpPost]
		public JsonResult Delete(int id)
		{
			var success = _ticketDataAccess.DeleteTicketById(id);
			return Json(new { success });
		}

		[HttpGet]
		public CustomerSupportItem GetById(int id)
		{
			var ticket = _ticketDataAccess.GetTicketById(id);
			return ticket;
		}

		[HttpGet]
		public JsonResult PopulateForm(int id)
		{
			var ticket = _ticketDataAccess.GetTicketById(id);
			return Json(ticket);
		}


		[HttpPost]
		public RedirectResult Update(CustomerSupportItem ticket)
		{
			var success = _ticketDataAccess.UpdateTicket(ticket);
			if (success)
			{
				return Redirect("/Home");
			}
			else
			{
				return Redirect("/Error");
			}
		}
	}
}
