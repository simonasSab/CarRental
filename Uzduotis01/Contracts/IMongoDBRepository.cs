namespace Uzduotis01
{
    public interface IMongoDBRepository
    {
        // Cleaning
        Task TruncateDatabaseStop();
        Task TruncateDatabaseStart(int cachePeriod);
        Task TruncateCollection(MongoDBCollectionName collectionNameEnum);

        // Import to cache
        Task ImportVehiclesAsync(IEnumerable<ElectricVehicle> electricVehicles, IEnumerable<FossilFuelVehicle> fossilFuelVehicles);
        Task ImportVehiclesAsync(IEnumerable<ElectricVehicle> electricVehicles);
        Task ImportVehiclesAsync(IEnumerable<FossilFuelVehicle> fossilFuelVehicles);
        Task ImportBicyclesAsync(IEnumerable<Bicycle> bicycles);
        Task ImportClientsAsync(IEnumerable<Client> clients);
        Task ImportRentsAsync(IEnumerable<Rent> rents);

        // Get all from cache or search by phrase
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync(string phrase);
        Task<IEnumerable<ElectricVehicle>> GetAllElectricVehiclesAsync();
        Task<IEnumerable<FossilFuelVehicle>> GetAllFossilFuelVehiclesAsync();
        Task<IEnumerable<Bicycle>> GetAllBicyclesAsync();
        Task<IEnumerable<Bicycle>> GetAllBicyclesAsync(string phrase);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<IEnumerable<Client>> GetAllClientsAsync(string phrase);
        Task<IEnumerable<Rent>> GetAllRentsAsync();
        Task<IEnumerable<Rent>> GetAllRentsAsync(int vehicleID);


        // Import single item into MongoDB collection
        Task<bool> InsertVehicleAsync(ElectricVehicle vehicle);
        Task<bool> InsertVehicleAsync(FossilFuelVehicle vehicle);
        Task<bool> InsertBicycleAsync(Bicycle bicycle);
        Task<bool> InsertClientAsync(Client client);
        Task<bool> InsertRentAsync(Rent rent);
    }
}
