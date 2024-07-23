using System.Collections.Generic;

namespace Homework_57_izida_kubekova.Models.ViewModels
{
    public class AddAndEditViewModel
    {
        public AddAndEditViewModel(Phone phone, List<Brand> brands)
        {
            Phone = phone;
            Brands = brands;
        }

        public AddAndEditViewModel()
        {
        }

        public Phone Phone { get; set; }
        public List<Brand> Brands { get; set; }
    }
}