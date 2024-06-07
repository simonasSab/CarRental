using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Uzduotis01;

namespace CarRental_FrontEnd
{
    public class RentsModel : PageModel
    {
        private readonly ICarRentalAPIService _apiService;
        [BindProperty] public IEnumerable<Rent>? Rents { get; set; }

        public RentsModel(ICarRentalAPIService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
            Rents = _apiService.GetAllRents();
        }
        //public bool VehiclesIDExists(int id)
        //{
        //    if (_apiService.VehiclesIDExists(id))
        //        return true;
        //    return false;
        //}

        //public bool ClientsIDExists(int id)
        //{
        //    if (_apiService.ClientsIDExists(id))
        //        return true;
        //    return false;
        //}

    }
}
