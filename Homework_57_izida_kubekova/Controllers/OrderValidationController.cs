using Homework_57_izida_kubekova.Models;
using Homework_57_izida_kubekova.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Controllers
{
    public class OrderValidationController : Controller
    {
        public bool CheckAddress(Order order)
        {
            if (string.IsNullOrWhiteSpace(order.Address))
                return false;
            return true;
        }
        public bool CheckContact(Order order)
        {
            if (string.IsNullOrWhiteSpace(order.ContactPhone))
                return false;
            return true;
        }
    }
}