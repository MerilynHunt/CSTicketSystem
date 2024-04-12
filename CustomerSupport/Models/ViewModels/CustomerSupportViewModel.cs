using System.Collections.Generic;

namespace CustomerSupport.Models.ViewModels
{
    public class CustomerSupportViewModel
    {
        public List<CustomerSupportItem> TicketList { get; set; }

        public CustomerSupportItem Ticket { get; set; }
    }
}
