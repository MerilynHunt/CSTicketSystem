using CustomerSupport.Models;
using CustomerSupport.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Controllers
{
	public interface IControllers
	{
		IActionResult Index();
		CustomerSupportViewModel GetTickets();
		RedirectResult Insert(CustomerSupportItem ticket);
		JsonResult Delete(int id);
		CustomerSupportItem GetById(int id);
		JsonResult PopulateForm(int id);
		RedirectResult Update(CustomerSupportItem ticket);

	}
}
