namespace Uzduotis01
{
    public interface IMongoDBRepository
    {
        // Deletion functions
        Task TruncateDatabaseStart(int cachePeriod);
        Task TruncateDatabaseStop();
        Task TruncateCollection(MongoDBCollectionName collectionNameEnum);

        // Get all collections from MongoDB
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<IEnumerable<ElectricVehicle>> GetAllElectricVehiclesAsync();
        Task<IEnumerable<FossilFuelVehicle>> GetAllFossilFuelVehiclesAsync();
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<IEnumerable<Rent>> GetAllRentsAsync();

        // Get collection items matching search phrase from MongoDB
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync(string phrase);
        Task<IEnumerable<Client>> GetAllClientsAsync(string phrase);

        // Import collection data into MongoDB
        Task ImportVehiclesAsync(IEnumerable<ElectricVehicle> electricVehicles, IEnumerable<FossilFuelVehicle> fossilFuelVehicles);
        Task ImportVehiclesAsync(IEnumerable<ElectricVehicle> electricVehicles);
        Task ImportVehiclesAsync(IEnumerable<FossilFuelVehicle> fossilFuelVehicles);
        Task ImportClientsAsync(IEnumerable<Client> clients);
        Task ImportRentsAsync(IEnumerable<Rent> rents);

        // Import single item into MongoDB collection
        Task<bool> InsertVehicleAsync(ElectricVehicle vehicle);
        Task<bool> InsertVehicleAsync(FossilFuelVehicle vehicle);
        Task<bool> InsertClientAsync(Client client);
        Task<bool> InsertRentAsync(Rent rent);
    }
}
