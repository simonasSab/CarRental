namespace Uzduotis01
{
    public interface IRentService
    {
        void ToggleCacheCleaning(int cachePeriod);
        bool GetCacheCleaningON();

        Task<bool> RegisterVehicle(ElectricVehicle electricVehicle);
        Task<bool> RegisterVehicle(FossilFuelVehicle fossilFuelVehicle);
        Task<bool> RegisterClient(Client client);
        Task<bool> RegisterRent(Rent rent);

        Task<int> DisplayAllVehiclesAsync();
        Task<int> DisplayAllElectricVehiclesAsync();
        Task<int> DisplayAllFossilFuelVehiclesAsync();
        Task<int> DisplayAllClientsAsync();
        Task<int> DisplayAllRentsAsync();
        int DisplayAllRents(int vehicleID);

        Vehicle? GetVehicle(int ID);
        Client? GetClient(int ID);
        Rent? GetRent(int ID);
        Task<IEnumerable<Vehicle>>? GetAllVehicles();
        Task<IEnumerable<Vehicle>>? GetAllVehicles(string phrase);
        Task<IEnumerable<ElectricVehicle>?> GetAllElectricVehicles();
        Task<IEnumerable<FossilFuelVehicle>?> GetAllFossilFuelVehicles();
        Task<IEnumerable<Client>?> GetAllClients();
        Task<IEnumerable<Client>?> GetAllClients(string phrase);
        Task<IEnumerable<Rent>?> GetAllRents();

        bool DeleteVehicle(int ID);
        bool DeleteClient(int ID);
        bool DeleteRent(int ID);

        bool UpdateVehicle(object? vehicle);
        bool UpdateClient(Client? client);
        bool UpdateRent(Rent? rent);

        bool VehiclesIDExists(int id);
        bool VehiclesIDExists(int id, out bool isElectric);
        bool ClientsIDExists(int id);
        bool RentsIDExists(int id);

        //void PrintCollection(IEnumerable<object> collection);
    }
}
