using Uzduotis01;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRental_FrontEnd
{
    public class ElectricVehiclesModel : PageModel
    {
        private readonly ICarRentalAPIService _apiService;
        [BindProperty] public IEnumerable<ElectricVehicle>? Vehicles { get; set; }

        public ElectricVehiclesModel(ICarRentalAPIService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
            Vehicles = _apiService.GetAllElectricVehicles();
        }
    }
}
