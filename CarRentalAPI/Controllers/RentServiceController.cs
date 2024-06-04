using Microsoft.AspNetCore.Mvc;
using Uzduotis01;

namespace CarRentalAPI.Controllers
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

        // HttpGet methods

        [HttpGet("GetAllVehicles")]
        public IEnumerable<Vehicle>? GetAllVehicles()
        {
            return _rentService.GetAllVehicles();
        }
        [HttpGet("GetAllElectricVehicles")]
        public IEnumerable<Vehicle>? GetAllElectricVehicles()
        {
            return _rentService.GetAllElectricVehicles();
        }
        [HttpGet("GetAllFossilFuelVehicles")]
        public IEnumerable<Vehicle>? GetAllFossilFuelVehicles()
        {
            return _rentService.GetAllFossilFuelVehicles();
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


        // HttpPost methods

        [HttpPost("RegisterElectricVehicle")]
        public bool RegisterVehicle(ElectricVehicle electricVehicle)
        {
            return _rentService.RegisterVehicle(electricVehicle);
        }
        [HttpPost("RegisterFossilFUelVehicle")]
        public bool RegisterVehicle(FossilFuelVehicle fossilFuelVehicle)
        {
            return _rentService.RegisterVehicle(fossilFuelVehicle);
        }
        [HttpPost("RegisterClient")]
        public bool RegisterClient(Client client)
        {
            return _rentService.RegisterClient(client);
        }
        [HttpPost("RegisterRent")]
        public bool RegisterRent(Rent rent)
        {
            return _rentService.RegisterRent(rent);
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
    }
}
