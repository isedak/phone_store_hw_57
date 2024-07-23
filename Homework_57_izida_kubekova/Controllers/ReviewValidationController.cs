using Homework_57_izida_kubekova.Models;
using Homework_57_izida_kubekova.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Controllers
{
    public class ReviewValidationController : Controller
    {
        public bool CheckInput(CustomerReview customerReview)
        {
            if (string.IsNullOrWhiteSpace(customerReview.Review))
                return false;
            return true;
        }
    }
}