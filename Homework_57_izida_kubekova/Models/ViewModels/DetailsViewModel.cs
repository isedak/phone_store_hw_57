using System.Collections.Generic;

namespace Homework_57_izida_kubekova.Models.ViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel(Phone phone, List<CustomerReview> customerReviews)
        {
            Phone = phone;
            CustomerReviews = customerReviews;
        }

        public Phone Phone { get; set; }
        public List<CustomerReview> CustomerReviews { get; set; }
    }
}