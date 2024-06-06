using Uzduotis01;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRental_FrontEnd
{
    public class VehiclesModel : PageModel
    {
        private readonly ICarRentalAPIService _apiService;
        [BindProperty] public IEnumerable<Vehicle>? Vehicles { get; set; }

        public VehiclesModel(ICarRentalAPIService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
            Vehicles = _apiService.GetAllVehicles();
        }
    }
}
