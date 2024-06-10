using MongoDB.Bson;
using MongoDB.Driver.Core.Configuration;
using System.Data.SqlClient;
using System.Data;
using MongoDB.Driver;
using System.Collections.Generic;
using Amazon.Auth.AccessControlPolicy;
using Newtonsoft.Json.Linq;

namespace Uzduotis01
{
    public class MongoDBRepository : IMongoDBRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;
        private readonly IMongoCollection<Client> _clients;
        private readonly IMongoCollection<Rent> _rents;

        private CancellationTokenSource CacheCleanCTSource { get; set; }
        private CancellationToken CToken { get; set; }

        private IMongoDatabase MongoDatabase { get; set; }

        public MongoDBRepository(IMongoClient mongoClient)
        {
            MongoDatabase = mongoClient.GetDatabase("CarRental");

            _vehicles = MongoDatabase.GetCollection<Vehicle>("Vehicles");
            _clients = MongoDatabase.GetCollection<Client>("Clients");
            _rents = MongoDatabase.GetCollection<Rent>("Rents");

            CacheCleanCTSource = new();
            // use this? --------------> CacheCleanCTSource.CancelAfter(TimeSpan.FromSeconds(1));
            CToken = CacheCleanCTSource.Token;
        }

        public async Task TruncateDatabaseStop()
        {
            // Cancel current Cache Clean cycle and dispose of the Token Source
            await CacheCleanCTSource.CancelAsync();
            CacheCleanCTSource.Dispose();
            // Create new token Source and new Token
            CacheCleanCTSource = new();
            // use this? --------------> CacheCleanCTSource.CancelAfter(TimeSpan.FromSeconds(1));
            CToken = CacheCleanCTSource.Token;
        }

        public async Task TruncateDatabaseStart(int cachePeriod)
        {
            await Task.Delay(cachePeriod, CToken);

            if (!CToken.IsCancellationRequested)
            {
                await _vehicles.DeleteManyAsync(new BsonDocument());
                await _clients.DeleteManyAsync(new BsonDocument());
                await _rents.DeleteManyAsync(new BsonDocument());
                Console.WriteLine($"{DateTime.Now} --- MongoDB cache cleared!\n");
                await TruncateDatabaseStart(cachePeriod);
            }
        }

        public async Task TruncateCollection(MongoDBCollectionName collectionNameEnum)
        {
            if (collectionNameEnum.ToString() == "Vehicles")
                await _vehicles.DeleteManyAsync(new BsonDocument());
            else if (collectionNameEnum.ToString() == "Clients")
                await _clients.DeleteManyAsync(new BsonDocument());
            else if (collectionNameEnum.ToString() == "Rents")
                await _rents.DeleteManyAsync(new BsonDocument());

            Console.WriteLine($"Deleted all items in MongoDB collection \"{collectionNameEnum.ToString()}\"\n");
        }

        public async Task ImportVehiclesAsync(IEnumerable<ElectricVehicle> electricVehicles, IEnumerable<FossilFuelVehicle> fossilFuelVehicles)
        {
            await _vehicles.InsertManyAsync(electricVehicles);
            await _vehicles.InsertManyAsync(fossilFuelVehicles);
        }
        public async Task ImportVehiclesAsync(IEnumerable<ElectricVehicle> electricVehicles)
        {
            await _vehicles.InsertManyAsync(electricVehicles);
        }
        public async Task ImportVehiclesAsync(IEnumerable<FossilFuelVehicle> fossilFuelVehicles)
        {
            await _vehicles.InsertManyAsync(fossilFuelVehicles);
        }

        public async Task ImportClientsAsync(IEnumerable<Client> clients)
        {
            await _clients.InsertManyAsync(clients);
        }
        public async Task ImportRentsAsync(IEnumerable<Rent> rents)
        {
            await _rents.InsertManyAsync(rents);
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicles.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<ElectricVehicle>> GetAllElectricVehiclesAsync()
        {
            IEnumerable<ElectricVehicle> vehicles = [];

            foreach (object it in await GetAllVehiclesAsync())
            {
                if (it.GetType().GetProperty("BatteryCapacity") != null)
                {
                    vehicles.Append((ElectricVehicle)it);
                }
            }
            return vehicles;
        }
        public async Task<IEnumerable<FossilFuelVehicle>> GetAllFossilFuelVehiclesAsync()
        {
            IEnumerable<FossilFuelVehicle> vehicles = [];

            foreach (object it in await GetAllVehiclesAsync())
            {
                if (it.GetType().GetProperty("TankCapacity") != null)
                {
                    vehicles.Append((FossilFuelVehicle)it);
                }
            }
            return vehicles;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _clients.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetAllRentsAsync()
        {
            return await _rents.Find(_ => true).ToListAsync();
        }
        public async Task<IEnumerable<Rent>> GetAllRentsAsync(int vehicleID)
        {
            return await _rents.Find(_ => true).ToListAsync();
        }

        public async Task<bool> InsertVehicleAsync(ElectricVehicle vehicle)
        {
            await _vehicles.InsertOneAsync(vehicle);
            return true;
        }
        public async Task<bool> InsertVehicleAsync(FossilFuelVehicle vehicle)
        {
            await _vehicles.InsertOneAsync(vehicle);
            return true;

        }
        public async Task<bool> InsertClientAsync(Client client)
        {
            await _clients.InsertOneAsync(client);
            return true;
        }
        public async Task<bool> InsertRentAsync(Rent rent)
        {
            await _rents.InsertOneAsync(rent);
            return true;
        }
    }
}
