namespace Uzduotis01
{
    public interface IMongoDBRepository
    {
        Task TruncateDatabaseStart(int cachePeriod);
        Task TruncateDatabaseStop();
        Task TruncateCollection(MongoDBCollectionName collectionNameEnum);

        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<IEnumerable<ElectricVehicle>> GetAllElectricVehiclesAsync();
        Task<IEnumerable<FossilFuelVehicle>> GetAllFossilFuelVehiclesAsync();
        Task<List<Client>> GetAllClientsAsync();
        Task<IEnumerable<Rent>> GetAllRentsAsync();

        Task ImportVehiclesAsync(IEnumerable<ElectricVehicle> electricVehicles, IEnumerable<FossilFuelVehicle> fossilFuelVehicles);
        Task ImportVehiclesAsync(IEnumerable<ElectricVehicle> electricVehicles);
        Task ImportVehiclesAsync(IEnumerable<FossilFuelVehicle> fossilFuelVehicles);
        Task ImportClientsAsync(IEnumerable<Client> clients);
        Task ImportRentsAsync(IEnumerable<Rent> rents);


        Task<bool> InsertVehicleAsync(ElectricVehicle vehicle);
        Task<bool> InsertVehicleAsync(FossilFuelVehicle vehicle);
        Task<bool> InsertClientAsync(Client client);
        Task<bool> InsertRentAsync(Rent rent);
    }
}
