using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Uzduotis01;

namespace CarRental_FrontEnd
{
    public class ClientsModel : PageModel
    {
        private readonly ICarRentalAPIService _apiService;
        [BindProperty] public IEnumerable<Client>? Clients { get; set; }

        public ClientsModel(ICarRentalAPIService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
            Clients = _apiService.GetAllClients();
        }
    }
}
