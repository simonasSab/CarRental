using Uzduotis01;
using System.Text.Json;

namespace CarRental_FrontEnd
{
    public class CarRentalAPIService : ICarRentalAPIService
    {
        private readonly string _apiBase;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public CarRentalAPIService(string apiBase)
        {
            _apiBase = apiBase;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_apiBase);
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public IEnumerable<Vehicle>? GetAllVehicles()
        {
            string path = "api/RentService/GetAllVehicles";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<IEnumerable<Vehicle>?>(jsonResponse, jsonSerializerOptions);
            }
            return null;
        }
        public IEnumerable<ElectricVehicle>? GetAllElectricVehicles()
        {
            string path = "api/RentService/GetAllElectricVehicles";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<IEnumerable<ElectricVehicle>?>(jsonResponse, jsonSerializerOptions);
            }
            return null;
        }
        public IEnumerable<FossilFuelVehicle>? GetAllFossilFuelVehicles()
        {
            string path = "api/RentService/GetAllFossilFuelVehicles";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<IEnumerable<FossilFuelVehicle>?>(jsonResponse, jsonSerializerOptions);
            }
            return null;
        }
        public IEnumerable<Client>? GetAllClients()
        {
            string path = "api/RentService/GetAllClients";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<IEnumerable<Client>?>(jsonResponse, jsonSerializerOptions);
            }
            return null;
        }
        public IEnumerable<Rent>? GetAllRents()
        {
            string path = "api/RentService/GetAllRents";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<IEnumerable<Rent>?>(jsonResponse, jsonSerializerOptions);
            }
            return null;
        }



        public Vehicle GetVehicle(int id)
        {
            throw new NotImplementedException();
        }
        public Client GetClient(int id)
        {
            throw new NotImplementedException();
        }
        public Rent GetRent(int id)
        {
            throw new NotImplementedException();
        }


        public bool VehiclesIDExists(int id)
        {
            throw new NotImplementedException();
        }
        public bool ClientsIDExists(int id)
        {
            throw new NotImplementedException();
        }
        public bool RentsIDExists(int id)
        {
            throw new NotImplementedException();
        }


        public bool RegisterVehicle(ElectricVehicle electricVehicle)
        {
            throw new NotImplementedException();
        }
        public bool RegisterVehicle(FossilFuelVehicle fossilFuelVehicle)
        {
            throw new NotImplementedException();
        }
        public bool RegisterClient(Client client)
        {
            throw new NotImplementedException();
        }
        public bool RegisterRent(Rent rent)
        {
            throw new NotImplementedException();
        }

        public bool DeleteClient(int id)
        {
            throw new NotImplementedException();
        }
        public bool DeleteRent(int id)
        {
            throw new NotImplementedException();
        }
        public bool DeleteVehicle(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateElectricVehicle(ElectricVehicle? vehicle)
        {
            throw new NotImplementedException();
        }
        public bool UpdateFossilFUelVehicle(FossilFuelVehicle? vehicle)
        {
            throw new NotImplementedException();
        }
        public bool UpdateClient(Client? client)
        {
            throw new NotImplementedException();
        }
        public bool UpdateRent(Rent? rent)
        {
            throw new NotImplementedException();
        }
    }
}
