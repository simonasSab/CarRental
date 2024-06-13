namespace Uzduotis01
{
    public interface IRentService
    {
        void ToggleCacheCleaning(int cachePeriod);
        bool GetCacheCleaningON();

        Task<bool> RegisterVehicle(ElectricVehicle electricVehicle);
        Task<bool> RegisterVehicle(FossilFuelVehicle fossilFuelVehicle);
        Task<bool> RegisterClient(Client client); // Entity Framework
        Task<bool> RegisterBicycle(Bicycle bicycle); // Entity Framework
        Task<bool> RegisterRent(Rent rent);

        Task<int> DisplayAllVehiclesAsync();
        Task<int> DisplayAllElectricVehiclesAsync();
        Task<int> DisplayAllFossilFuelVehiclesAsync();
        Task<int> DisplayAllBicyclesAsync();
        Task<int> DisplayAllClientsAsync();
        Task<int> DisplayAllRentsAsync();
        int DisplayAllRents(int? itemID, bool vehicleTrueBicycleFalse);

        Vehicle? GetVehicle(int ID);
        Bicycle? GetBicycle(int ID); // Entity Framework
        Client? GetClient(int ID); // Entity Framework
        Rent? GetRent(int ID);

        Task<IEnumerable<Vehicle>>? GetAllVehicles();
        Task<IEnumerable<Vehicle>>? GetAllVehicles(string phrase); // Search
        Task<IEnumerable<ElectricVehicle>?> GetAllElectricVehicles();
        Task<IEnumerable<FossilFuelVehicle>?> GetAllFossilFuelVehicles();
        Task<IEnumerable<Bicycle>?> GetAllBicycles(); // Entity Framework
        Task<IEnumerable<Bicycle>?> GetAllBicycles(string phrase); // Search - Entity Framework
        Task<IEnumerable<Client>?> GetAllClients(); // Entity Framework
        Task<IEnumerable<Client>?> GetAllClients(string phrase); // Search - Entity Framework
        Task<IEnumerable<Rent>?> GetAllRents();

        bool DeleteVehicle(int ID);
        bool DeleteBicycle(int ID); // Entity Framework
        bool DeleteClient(int ID); // Entity Framework
        bool DeleteRent(int ID);

        bool UpdateVehicle(object? vehicle);
        bool UpdateBicycle(Bicycle? bicycle); // Entity Framework
        bool UpdateClient(Client? client); // Entity Framework
        bool UpdateRent(Rent? rent);

        bool VehiclesIDExists(int id);
        bool VehiclesIDExists(int id, out bool isElectric);
        bool BicyclesIDExists(int id);
        bool ClientsIDExists(int id);
        bool RentsIDExists(int id);

        //void PrintCollection(IEnumerable<object> collection);
    }
}
