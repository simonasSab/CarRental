using Uzduotis01;

namespace CarRental_FrontEnd;

public interface ICarRentalAPIService
{
    IEnumerable<Vehicle>? GetAllVehicles();
    IEnumerable<ElectricVehicle>? GetAllElectricVehicles();
    IEnumerable<FossilFuelVehicle>? GetAllFossilFuelVehicles();
    IEnumerable<Client>? GetAllClients();
    IEnumerable<Rent>? GetAllRents();


    Vehicle GetVehicle(int id);
    Client GetClient(int id);
    Rent GetRent(int id);

    bool DeleteVehicle(int id);
    bool DeleteClient(int id);
    bool DeleteRent(int id);

    bool VehiclesIDExists(int id);
    bool ClientsIDExists(int id);
    bool RentsIDExists(int id);

    bool RegisterVehicle(ElectricVehicle electricVehicle);
    bool RegisterVehicle(FossilFuelVehicle fossilFuelVehicle);
    bool RegisterClient(Client client);
    bool RegisterRent(Rent rent);

    bool UpdateElectricVehicle(ElectricVehicle? vehicle);
    bool UpdateFossilFUelVehicle(FossilFuelVehicle? vehicle);
    bool UpdateClient(Client? client);
    bool UpdateRent(Rent? rent);
}