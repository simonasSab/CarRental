using MongoDB.Bson;
using MongoDB.Driver.Core.Configuration;
using System.Data.SqlClient;
using System.Data;
using MongoDB.Driver;
using System.Collections.Generic;
using Amazon.Auth.AccessControlPolicy;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Serilog;

namespace Uzduotis01
{
    public class MongoDBRepository : IMongoDBRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;
        private readonly IMongoCollection<Bicycle> _bicycles;
        private readonly IMongoCollection<Client> _clients;
        private readonly IMongoCollection<Rent> _rents;

        private CancellationTokenSource CacheCleanCTSource { get; set; }
        private CancellationToken CToken { get; set; }
        private IMongoDatabase MongoDatabase { get; set; }

        public MongoDBRepository(IMongoClient mongoClient)
        {
            MongoDatabase = mongoClient.GetDatabase("CarRental");

            _vehicles = MongoDatabase.GetCollection<Vehicle>("Vehicles");
            _bicycles = MongoDatabase.GetCollection<Bicycle>("Bicycles");
            _clients = MongoDatabase.GetCollection<Client>("Clients");
            _rents = MongoDatabase.GetCollection<Rent>("Rents");

            CacheCleanCTSource = new();
            // use this? --------------> CacheCleanCTSource.CancelAfter(TimeSpan.FromSeconds(1));
            CToken = CacheCleanCTSource.Token;
        }

        // Cleaning
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
                Log.Information($"{DateTime.Now} --- MongoDB cache cleared!\n");
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

            Log.Information($"Deleted all items in MongoDB collection \"{collectionNameEnum.ToString()}\"\n");
        }

        // Import into cache
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
        public async Task ImportBicyclesAsync(IEnumerable<Bicycle> bicycles)
        {
            await _bicycles.InsertManyAsync(bicycles);
        }
        public async Task ImportClientsAsync(IEnumerable<Client> clients)
        {
            await _clients.InsertManyAsync(clients);
        }
        public async Task ImportRentsAsync(IEnumerable<Rent> rents)
        {
            await _rents.InsertManyAsync(rents);
        }

        // Get all from cache or search by phrase
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicles.Find(_ => true).ToListAsync();
        }
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync(string phrase)
        {
            FilterDefinition<Vehicle> filter = Builders<Vehicle>.Filter.Regex(Regex.Replace("Make", @"\s+", ""), new BsonRegularExpression(Regex.Replace(phrase, @"\s+", ""), "i"));
            filter |= Builders<Vehicle>.Filter.Regex(Regex.Replace("Model", @"\s+", ""), new BsonRegularExpression(Regex.Replace(phrase, @"\s+", ""), "i"));

            Log.Information($"Trying to find \"{phrase}\" in Vehicles...\n");

            return await _vehicles.Find(filter).SortBy(x => x.Make).ThenBy(x => x.Model).ToListAsync();
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
        public async Task<IEnumerable<Bicycle>> GetAllBicyclesAsync()
        {
            return await _bicycles.Find(_ => true).ToListAsync();
        }
        public async Task<IEnumerable<Bicycle>> GetAllBicyclesAsync(string phrase)
        {
            FilterDefinition<Bicycle> filter = Builders<Bicycle>.Filter.Regex(Regex.Replace("Name", @"\s+", ""), new BsonRegularExpression(Regex.Replace(phrase, @"\s+", ""), "i"));

            Log.Information($"Trying to find \"{phrase}\" in Bicycles...\n");

            return await _bicycles.Find(filter).SortBy(x => x.Name).ToListAsync();
        }
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _clients.Find(_ => true).ToListAsync();
        }
        public async Task<IEnumerable<Client>> GetAllClientsAsync(string phrase)
        {
            FilterDefinition<Client> filter = Builders<Client>.Filter.Regex(Regex.Replace("FullName", @"\s+", ""), new BsonRegularExpression(Regex.Replace(phrase, @"\s+", ""), "i"));

            Log.Information($"Trying to find \"{phrase}\" in Clients...\n");

            return await _clients.Find(filter).SortBy(x => x.FullName).ToListAsync();
        }
        public async Task<IEnumerable<Rent>> GetAllRentsAsync()
        {
            return await _rents.Find(_ => true).ToListAsync();
        }
        public async Task<IEnumerable<Rent>> GetAllRentsAsync(int vehicleID)
        {
            return await _rents.Find(_ => true).ToListAsync();
        }

        // Import single item into cache
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
        public async Task<bool> InsertBicycleAsync(Bicycle bicycle)
        {
            await _bicycles.InsertOneAsync(bicycle);
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
