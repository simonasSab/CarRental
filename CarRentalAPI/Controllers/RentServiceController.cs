using Microsoft.AspNetCore.Mvc;
using Uzduotis01;
using MongoDB.Bson;
using MongoDB.Driver.Core.Configuration;
using System.Data.SqlClient;
using System.Data;
using MongoDB.Driver;
using System.Threading;
using System.Text.RegularExpressions;

namespace CarRentalAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentServiceController : ControllerBase
    {
        private readonly IRentService _rentService;

        public RentServiceController(IRentService rentService)
        {
            _rentService = rentService;
        }

        // Search ____________________________________________________________________________
        // ___________________________________________________________________________________

        [HttpGet("SearchAllVehiclesAsync")]
        public async Task<IEnumerable<Vehicle>?> GetAllVehiclesAsync(string phrase)
        {
            return await _rentService.GetAllVehicles(phrase);
        }
        [HttpGet("SearchAllClientsAsync")]
        public async Task<IEnumerable<Client>?> GetAllClientsAsync(string phrase)
        {
            return await _rentService.GetAllClients(phrase);
        }

        // ___________________________________________________________________________________
        // ___________________________________________________________________________________

        // HttpGet methods

        [HttpGet("GetAllVehiclesAsync")]
        public async Task<IEnumerable<Vehicle>?> GetAllVehiclesAsync()
        {
            return await _rentService.GetAllVehicles();
        }
        [HttpGet("GetAllElectricVehiclesAsync")]
        public async Task<IEnumerable<ElectricVehicle>?> GetAllElectricVehiclesAsync()
        {
            return await _rentService.GetAllElectricVehicles();
        }
        [HttpGet("GetAllFossilFuelVehiclesAsync")]
        public async Task<IEnumerable<FossilFuelVehicle>?> GetAllFossilFuelVehiclesAsync()
        {
            return await _rentService.GetAllFossilFuelVehicles();
        }
        [HttpGet("GetAllClientsAsync")]
        public async Task<IEnumerable<Client>?> GetAllClientsAsync()
        {
            return await _rentService.GetAllClients();
        }
        [HttpGet("GetAllRentsAsync")]
        public async Task<IEnumerable<Rent>?> GetAllRentsAsync()
        {
            return await _rentService.GetAllRents();
        }

        [HttpGet("GetVehicle")]
        public Vehicle GetVehicle(int id)
        {
            return _rentService.GetVehicle(id);
        }
        [HttpGet("GetClient")]
        public Client GetClient(int id)
        {
            return _rentService.GetClient(id);
        }
        [HttpGet("GetRent")]
        public Rent GetRent(int id)
        {
            return _rentService.GetRent(id);
        }


        [HttpGet("DeleteVehicle")]
        public bool DeleteVehicle(int id)
        {
            return _rentService.DeleteVehicle(id);
        }
        [HttpGet("DeleteClient")]
        public bool DeleteClient(int id)
        {
            return _rentService.DeleteClient(id);
        }
        [HttpGet("DeleteRent")]
        public bool DeleteRent(int id)
        {
            return _rentService.DeleteRent(id);
        }


        [HttpGet("VehiclesIDExists")]
        public bool VehiclesIDExists(int id)
        {
            return _rentService.VehiclesIDExists(id);
        }
        [HttpGet("ClientsIDExists")]
        public bool ClientsIDExists(int id)
        {
            return _rentService.ClientsIDExists(id);
        }
        [HttpGet("RentsIDExists")]
        public bool RentsIDExists(int id)
        {
            return _rentService.RentsIDExists(id);
        }

        // HttpPost methods

        [HttpPost("RegisterElectricVehicle")]
        public async Task<bool> RegisterVehicle([FromForm] ElectricVehicle electricVehicle)
        {
            return await _rentService.RegisterVehicle(electricVehicle);
        }
        [HttpPost("RegisterFossilFUelVehicle")]
        public async Task<bool> RegisterVehicle([FromForm] FossilFuelVehicle fossilFuelVehicle)
        {
            return await _rentService.RegisterVehicle(fossilFuelVehicle);
        }
        [HttpPost("RegisterClient")]
        public async Task<bool> RegisterClient([FromForm] Client client)
        {
            return await _rentService.RegisterClient(client);
        }
        [HttpPost("RegisterRent")]
        public async Task<bool> RegisterRent([FromForm] Rent rent)
        {
            return await _rentService.RegisterRent(rent);
        }


        [HttpPost("UpdateElectricVehicle")]
        public bool UpdateElectricVehicle(ElectricVehicle? vehicle)
        {
            return _rentService.UpdateVehicle(vehicle);
        }
        [HttpPost("UpdateFossilFUelVehicle")]
        public bool UpdateFossilFUelVehicle(FossilFuelVehicle? vehicle)
        {
            return _rentService.UpdateVehicle(vehicle);
        }
        [HttpPost("UpdateClient")]
        public bool UpdateClient(Client? client)
        {
            return _rentService.UpdateClient(client);
        }
        [HttpPost("UpdateRent")]
        public bool UpdateRent(Rent? rent)
        {
            return _rentService.UpdateRent(rent);
        }
    }
}
