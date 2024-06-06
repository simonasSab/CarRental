using Uzduotis01;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CarRental_FrontEnd
{
    public class FossilFuelVehiclesModel : PageModel
    {
        private readonly ICarRentalAPIService _apiService;
        [BindProperty] public IEnumerable<FossilFuelVehicle>? Vehicles { get; set; }

        public FossilFuelVehiclesModel(ICarRentalAPIService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
            Vehicles = _apiService.GetAllFossilFuelVehicles();
        }
    }
}
