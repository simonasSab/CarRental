using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Linq;
using Serilog;

namespace Uzduotis01
{
    // Turi būti sukurtas NuomaService (RentService), kuriame būtų įgyvendinamas programos funkcionalumas:
    // Automobilių registracija ir jų tipų priskyrimas.
    // Automobilių sąrašo gavimas su galimybe filtruoti pagal tipą.
    // Automobilių informacijos atnaujinimas.
    // Automobilių ištrinimas.
    // Automobilio išnuomavimas pasirinkus automobilį ir pasirinkus klientą, bei priskyrus laiką.
    // Jeigu Automobilis jau yra kažkam tuo laikotarpiu išnuomotas, turi būti metama klaida, jog automobilio išnuomuoti negalima.
    //
    // (NuomaConsoleUI turi galimybę valdyti NuomaService) -> NuomaService turi turėti galimybę valdyti IDatabaseRepository.
    // Pats NuomaService turi priimti IDatabaseRepository, per kurį jis atlikinės visus veiksmus su duomenų baze.
    public class RentService : IRentService
    {
        private IDatabaseRepository DatabaseRepo { get; set; }
        private IMongoDBRepository MongoDBRepo { get; set; }
        private bool CacheCleaningON { get; set; } = false;

        public RentService(IDatabaseRepository databaseRepository, IMongoDBRepository mongoDBRepository)
        {
            DatabaseRepo = databaseRepository;
            MongoDBRepo = mongoDBRepository;
        }

        // Cache cleaning
        public bool GetCacheCleaningON()
        {
            return CacheCleaningON;
        }
        public void ToggleCacheCleaning(int cachePeriod)
        {
            if (!CacheCleaningON)
            {
                Log.Information($"Cache Cleaning: ON ({cachePeriod} s)\n");
                CacheCleaningON = true;
                MongoDBRepo.TruncateDatabaseStart(cachePeriod * 1000);
            }
            else
            {
                Log.Information("Cache Cleaning: OFF\n");
                MongoDBRepo.TruncateDatabaseStop();
                CacheCleaningON = false;
            }
        }

        // Insert items into DB and cache
        public async Task<bool> RegisterVehicle(ElectricVehicle electricVehicle)
        {
            if (DatabaseRepo.InsertVehicle(electricVehicle, out ElectricVehicle newVehicle))
            {
                Log.Information($"New electric vehicle: {newVehicle.ToString()}");

                if (await MongoDBRepo.InsertVehicleAsync(newVehicle))
                    Log.Information($"Inserted to MongoDB successfully.\n");
                else
                    Log.Error($"Failed to insert to MongoDB.\n");

                return true;
            }
            return false;
        }
        public async Task<bool> RegisterVehicle(FossilFuelVehicle fossilFuelVehicle)
        {
            if (DatabaseRepo.InsertVehicle(fossilFuelVehicle, out FossilFuelVehicle newVehicle))
            {
                Log.Information($"New fossil fuel vehicle: {newVehicle.ToString()}");

                if (await MongoDBRepo.InsertVehicleAsync(newVehicle))
                    Log.Information($"Inserted to MongoDB successfully.\n");
                else
                    Log.Error($"Failed to insert to MongoDB.\n");

                return true;
            }
            return false;
        }
        public async Task<bool> RegisterRent(Rent rent)
        {
            if (RentIsPossible(rent))
            {
                if (DatabaseRepo.InsertRent(rent, out Rent newRent))
                {
                    Log.Information($"New rent agreement: {newRent.ToString()}\n");

                    if (await MongoDBRepo.InsertRentAsync(newRent))
                        Log.Information($"Inserted to MongoDB successfully.\n");
                    else
                        Log.Error($"Failed to insert to MongoDB.\n");

                    return true;
                }
                return false;
            }
            else
            {
                Log.Information("Vehicle is occupied now or in the desired date range.\n");
            }
            return false;
        }

        // RegisterClient NOT Entity Framework:
        //public async Task<bool> RegisterClient(Client client)
        //{
        //    if (DatabaseRepo.InsertClient(client, out Client newClient))
        //    {
        //        Log.Information($"New client: {newClient.ToString()}");

        //        if (await MongoDBRepo.InsertClientAsync(newClient))
        //            Log.Information($"Inserted to MongoDB successfully.\n");
        //        else
        //            Log.Error($"Failed to insert to MongoDB.\n");

        //        return true;
        //    }
        //    return false;
        //}
        public async Task<bool> RegisterClient(Client? client) // Entity Framework
        {
            if (DatabaseRepo.InsertClientEF(client, out Client newClient))
            {
                Log.Information($"New client: {newClient.ToString()}");

                if (await MongoDBRepo.InsertClientAsync(newClient))
                    Log.Information($"Inserted to MongoDB successfully.\n");
                else
                    Log.Error($"Failed to insert to MongoDB.\n");

                return true;
            }
            Log.Error($"Something went wrong while inserting into DB.\n");
            return false;
        }
        public async Task<bool> RegisterBicycle(Bicycle bicycle) // Entity Framework
        {
            if (DatabaseRepo.InsertBicycleEF(bicycle, out Bicycle newBicycle))
            {
                Log.Information($"New bicycle: {newBicycle.ToString()}");

                if (await MongoDBRepo.InsertBicycleAsync(newBicycle))
                    Log.Information($"Inserted to MongoDB successfully.\n");
                else
                    Log.Error($"Failed to insert to MongoDB.\n");

                return true;
            }
            return false;
        }

        public async Task<int> DisplayAllVehiclesAsync()
        {
            int count = 0;
            List<Vehicle>? vehicles = (await GetAllVehicles()).ToList();

            if (vehicles == null)
                return 0;

            foreach (Vehicle vehicle in vehicles)
            {
                Console.WriteLine(vehicle.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public async Task<int> DisplayAllElectricVehiclesAsync()
        {
            int count = 0;
            List<ElectricVehicle>? vehicles = (List<ElectricVehicle>?)(await GetAllElectricVehicles());

            if (vehicles == null)
                return 0;

            foreach (ElectricVehicle vehicle in vehicles)
            {
                Console.WriteLine(vehicle.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public async Task<int> DisplayAllFossilFuelVehiclesAsync()
        {
            int count = 0;
            List<FossilFuelVehicle>? vehicles = (List<FossilFuelVehicle>?)(await GetAllFossilFuelVehicles());

            if (vehicles == null)
                return 0;

            foreach (FossilFuelVehicle vehicle in vehicles)
            {
                Console.WriteLine(vehicle.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public async Task<int> DisplayAllBicyclesAsync()
        {
            int count = 0;
            List<Bicycle>? bicycles = (List<Bicycle>?)(await GetAllBicycles());

            if (bicycles == null)
                return 0;

            foreach (Bicycle bicycle in bicycles)
            {
                Console.WriteLine(bicycle.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public async Task<int> DisplayAllClientsAsync()
        {
            int count = 0;
            List<Client>? clients = (List<Client>?)(await GetAllClients());

            if (clients == null)
                return 0;

            foreach (Client client in clients)
            {
                Console.WriteLine(client.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public async Task<int> DisplayAllRentsAsync()
        {
            int count = 0;
            List<Rent>? rents = (await GetAllRents()).ToList();

            if (rents == null)
                return 0;

            foreach (Rent rent in rents)
            {
                Console.WriteLine(rent.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public int DisplayAllRents(int? itemID, bool vehicleTrueBicycleFalse)
        {
            int count = 0;
            IEnumerable<Rent>? rents = DatabaseRepo.GetAllRents(itemID, vehicleTrueBicycleFalse);

            if (rents == null)
            {
                if (vehicleTrueBicycleFalse)
                    Log.Information("There are no rents of this vehicle in the database\n");
                else
                    Log.Information("There are no rents of this bicycle in the database\n");

                return 0;
            }

            foreach (Rent rent in rents)
            {
                Console.WriteLine(rent.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }

        public Vehicle? GetVehicle(int ID)
        {
            if (VehiclesIDExists(ID))
            {
                return DatabaseRepo.GetVehicle(ID);
            }
            else
            {
                Log.Information($"Vehicle not found with ID {ID:000}");
                return null;
            }
        }
        public Bicycle? GetBicycle(int ID) // Entity Framework
        {
            if (BicyclesIDExists(ID))
            {
                return DatabaseRepo.GetBicycleEF(ID);
            }
            else
            {
                Log.Information($"Bicycle not found with ID {ID:000}");
                return null;
            }
        }
        // GetClient Not Entity Framework:
        //public Client? GetClient(int ID)
        //{
        //    if (ClientsIDExists(ID))
        //    {
        //        return DatabaseRepo.GetClient(ID);
        //    }
        //    else
        //    {
        //        Log.Information($"Client not found with ID {ID:000}");
        //        return null;
        //    }
        //}
        public Client? GetClient(int ID) // Entity Framework
        {
            if (ClientsIDExists(ID))
            {
                return DatabaseRepo.GetClientEF(ID);
            }
            else
            {
                Log.Information($"Client not found with ID {ID:000}");
                return null;
            }
        }
        public Rent? GetRent(int ID)
        {
            if (RentsIDExists(ID))
            {
                return DatabaseRepo.GetRent(ID);
            }
            else
            {
                Log.Information($"Rent not found with ID {ID:000}");
                return null;
            }
        }


        // Get all items
        public async Task<IEnumerable<Vehicle>>? GetAllVehicles()
        {
            IEnumerable<Vehicle> vehicles = await MongoDBRepo.GetAllVehiclesAsync();
            int mongoCount = vehicles.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Successfully retrieved {mongoCount} vehicles from MongoDB cache\n");
                return vehicles;
            }
            else
            {
                Log.Information($"MongoDB cache is empty, synchronizing...\n");
            }

            bool hasEntries = DatabaseRepo.GetAllVehicles(out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles);

            if (!hasEntries)
            {
                Log.Information("There are no vehicles in the database\n");
                return null;
            }

            if (fossilFuelVehicles.Count() == 0)
            {
                await MongoDBRepo.ImportVehiclesAsync(electricVehicles);
                return electricVehicles;
            }
            else if (electricVehicles.Count() == 0)
            {
                await MongoDBRepo.ImportVehiclesAsync(fossilFuelVehicles);
                return fossilFuelVehicles;
            }
            else
            {
                await MongoDBRepo.ImportVehiclesAsync(electricVehicles, fossilFuelVehicles);
                vehicles = electricVehicles.Concat((IEnumerable<Vehicle>)fossilFuelVehicles);
                return vehicles;
            }
        }
        public async Task<IEnumerable<Vehicle>>? GetAllVehicles(string phrase) // Search
        {
            IEnumerable<Vehicle> vehicles = await MongoDBRepo.GetAllVehiclesAsync(phrase);
            int mongoCount = vehicles.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Found {mongoCount} vehicles by phrase \"{phrase}\" from MongoDB cache\n");
                return vehicles;
            }
            else
            {
                Log.Information("Nothing found in MongoDB, trying SQL Database...\n");
            }

            bool hasEntries = DatabaseRepo.GetAllVehicles(phrase, out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles);

            if (!hasEntries)
            {
                Log.Information($"No vehicles found by phrase \"{phrase}\" in the database\n");
                return null;
            }

            if (fossilFuelVehicles.Count() == 0)
            {
                await MongoDBRepo.ImportVehiclesAsync(electricVehicles);
                return electricVehicles;
            }
            else if (electricVehicles.Count() == 0)
            {
                await MongoDBRepo.ImportVehiclesAsync(fossilFuelVehicles);
                return fossilFuelVehicles;
            }
            else
            {
                await MongoDBRepo.ImportVehiclesAsync(electricVehicles, fossilFuelVehicles);
                vehicles = electricVehicles.Concat((IEnumerable<Vehicle>)fossilFuelVehicles);
                return vehicles;
            }
        }
        public async Task<IEnumerable<ElectricVehicle>?> GetAllElectricVehicles()
        {
            IEnumerable<ElectricVehicle> vehiclesMongoDB = await MongoDBRepo.GetAllElectricVehiclesAsync();
            int mongoCount = vehiclesMongoDB.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Successfully retrieved {mongoCount} electric vehicles from MongoDB cache\n");
                return vehiclesMongoDB;
            }
            else
            {
                Log.Information($"MongoDB cache is empty, synchronizing...\n");
            }

            IEnumerable<ElectricVehicle> vehicles = (IEnumerable<ElectricVehicle>)DatabaseRepo.GetAllVehicles(true);

            if (vehicles.Count() < 1)
            {
                Log.Information("There are no electric vehicles in the database\n");
                return null;
            }

            await MongoDBRepo.ImportVehiclesAsync(vehicles);
            return vehicles;
        }
        public async Task<IEnumerable<FossilFuelVehicle>?> GetAllFossilFuelVehicles()
        {
            IEnumerable<FossilFuelVehicle> vehiclesMongoDB = await MongoDBRepo.GetAllFossilFuelVehiclesAsync();
            int mongoCount = vehiclesMongoDB.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Successfully retrieved {mongoCount} fossil fuel vehicles from MongoDB cache\n");
                return vehiclesMongoDB;
            }
            else
            {
                Log.Information($"MongoDB cache is empty, synchronizing...\n");
            }

            IEnumerable<FossilFuelVehicle> vehicles = (IEnumerable<FossilFuelVehicle>)DatabaseRepo.GetAllVehicles(false);

            if (vehicles.Count() < 1)
            {
                Log.Information("There are no fossil fuel vehicles in the database\n");
                return null;
            }

            await MongoDBRepo.ImportVehiclesAsync(vehicles);
            return vehicles;
        }
        public async Task<IEnumerable<Bicycle>?> GetAllBicycles() // Entity Framework
        {
            List<Bicycle>? bicyclesMongoDB = (await MongoDBRepo.GetAllBicyclesAsync()).ToList();
            int mongoCount = bicyclesMongoDB.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Successfully retrieved {mongoCount} bicycles from MongoDB cache\n");
                return bicyclesMongoDB;
            }
            else
            {
                Log.Information($"MongoDB cache is empty, synchronizing...\n");
            }

            IEnumerable<Bicycle> bicycles = DatabaseRepo.GetAllBicyclesEF();

            if (bicycles.Count() < 1)
            {
                Log.Information("There are no bicycles in the database\n");
                return null;
            }

            await MongoDBRepo.ImportBicyclesAsync(bicycles);
            return bicycles;
        }
        public async Task<IEnumerable<Bicycle>?> GetAllBicycles(string phrase) // Search - Entity Framework
        {
            List<Bicycle>? bicyclesMongoDB = (await MongoDBRepo.GetAllBicyclesAsync(phrase)).ToList();
            int mongoCount = bicyclesMongoDB.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Found {mongoCount} bicycles by phrase \"{phrase}\" from MongoDB cache\n");
                return bicyclesMongoDB;
            }
            else
            {
                Log.Information("Nothing found in MongoDB, trying SQL Database...\n");
            }

            IEnumerable<Bicycle> bicycles = DatabaseRepo.GetAllBicyclesEF(phrase);

            if (bicycles.Count() < 1)
            {
                Log.Information($"No bicycles found by phrase \"{phrase}\" in the database\n");
                return null;
            }

            await MongoDBRepo.ImportBicyclesAsync(bicycles);
            return bicycles;
        }
        // GetAllClients Not Entity Framework
        //public async Task<IEnumerable<Client>?> GetAllClients()
        //{
        //    List<Client>? clientsMongoDB = (await MongoDBRepo.GetAllClientsAsync()).ToList();
        //    int mongoCount = clientsMongoDB.Count();

        //    if (mongoCount > 0)
        //    {
        //        Log.Information($"Successfully retrieved {mongoCount} clients from MongoDB cache\n");
        //        return clientsMongoDB;
        //    }
        //    else
        //    {
        //        Log.Information($"MongoDB cache is empty, synchronizing...\n");
        //    }

        //    IEnumerable<Client> clients = DatabaseRepo.GetAllClients();

        //    if (clients.Count() < 1)
        //    {
        //        Log.Information("There are no clients in the database\n");
        //        return null;
        //    }

        //    await MongoDBRepo.ImportClientsAsync(clients);
        //    return clients;
        //}
        public async Task<IEnumerable<Client>?> GetAllClients() // Entity Framework
        {
            List<Client>? clientsMongoDB = (await MongoDBRepo.GetAllClientsAsync()).ToList();
            int mongoCount = clientsMongoDB.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Successfully retrieved {mongoCount} clients from MongoDB cache\n");
                return clientsMongoDB;
            }
            else
            {
                Log.Information($"MongoDB cache is empty, synchronizing...\n");
            }

            IEnumerable<Client> clients = DatabaseRepo.GetAllClientsEF();

            if (clients.Count() < 1)
            {
                Log.Information("There are no clients in the database\n");
                return null;
            }

            await MongoDBRepo.ImportClientsAsync(clients);
            return clients;
        }
        // GetAllClients Not Entity Framework
        //public async Task<IEnumerable<Client>?> GetAllClients(string phrase)
        //{
        //    List<Client>? clientsMongoDB = (await MongoDBRepo.GetAllClientsAsync(phrase)).ToList();
        //    int mongoCount = clientsMongoDB.Count();

        //    if (mongoCount > 0)
        //    {
        //        Log.Information($"Found {mongoCount} clients by phrase \"{phrase}\" from MongoDB cache\n");
        //        return clientsMongoDB;
        //    }
        //    else
        //    {
        //        Log.Information("Nothing found in MongoDB, trying SQL Database...\n");
        //    }

        //    IEnumerable<Client> clients = DatabaseRepo.GetAllClients(phrase);

        //    if (clients.Count() < 1)
        //    {
        //        Log.Information($"No clients found by phrase \"{phrase}\" in the database\n");
        //        return null;
        //    }

        //    await MongoDBRepo.ImportClientsAsync(clients);
        //    return clients;
        //}
        public async Task<IEnumerable<Client>?> GetAllClients(string phrase) // Search - Entity Framework
        {
            List<Client>? clientsMongoDB = (await MongoDBRepo.GetAllClientsAsync(phrase)).ToList();
            int mongoCount = clientsMongoDB.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Found {mongoCount} clients by phrase \"{phrase}\" from MongoDB cache\n");
                return clientsMongoDB;
            }
            else
            {
                Log.Information("Nothing found in MongoDB, trying SQL Database...\n");
            }

            IEnumerable<Client> clients = DatabaseRepo.GetAllClientsEF(phrase);

            if (clients.Count() < 1)
            {
                Log.Information($"No clients found by phrase \"{phrase}\" in the database\n");
                return null;
            }

            await MongoDBRepo.ImportClientsAsync(clients);
            return clients;
        }
        public async Task<IEnumerable<Rent>?> GetAllRents()
        {
            IEnumerable<Rent> rentsMongoDB = await MongoDBRepo.GetAllRentsAsync();
            int mongoCount = rentsMongoDB.Count();

            if (mongoCount > 0)
            {
                Log.Information($"Successfully retrieved {mongoCount} rents from MongoDB cache\n");
                return rentsMongoDB;
            }
            else
            {
                Log.Information($"MongoDB cache is empty, synchronizing...\n");
            }

            IEnumerable<Rent> rents = DatabaseRepo.GetAllRents();

            if (rents == null)
            {
                Log.Information("There are no rents in the database\n");
                return null;
            }

            await MongoDBRepo.ImportRentsAsync(rents);
            return rents;
        }


        public bool DeleteVehicle(int ID)
        {
            if (DatabaseRepo.DeleteVehicle(ID))
            {
                Log.Information($"Deleted vehicle with ID:{ID}");
                return true;
            }
            return false;
        }
        public bool DeleteBicycle(int ID) // Entity Framework
        {
            if (DatabaseRepo.DeleteBicycleEF(ID))
            {
                Log.Information($"Deleted bicycle with ID:{ID}");
                return true;
            }
            return false;
        }
        public bool DeleteClient(int ID) // Entity Framework
        {
            if (DatabaseRepo.DeleteClientEF(ID))
            {
                Log.Information($"Deleted client with ID:{ID}");
                return true;
            }
            return false;
        }
        public bool DeleteRent(int ID)
        {
            if (DatabaseRepo.DeleteRent(ID))
            {
                Log.Information($"Deleted rent with ID:{ID}");
                return true;
            }
            return false;
        }

        public bool UpdateVehicle(object? vehicle)
        {
            if (vehicle == null)
                return false;

            if (DatabaseRepo.UpdateVehicle(vehicle, out object updatedVehicle))
            {
                if (vehicle is ElectricVehicle)
                {
                    Log.Information($"Updated vehicle: {((ElectricVehicle)updatedVehicle).ToString()}");
                    return true;
                }
                else if (vehicle is FossilFuelVehicle)
                {
                    Log.Information($"Updated vehicle: {((FossilFuelVehicle)updatedVehicle).ToString()}");
                    return true;
                }
            }
            return false;
        }
        public bool UpdateClient(Client? client) // Entity Framework
        {
            if (client == null)
                return false;

            if (DatabaseRepo.UpdateClientEF(client, out Client updatedClient))
            {
                Log.Information($"Updated client: {updatedClient.ToString()}");
                return true;
            }
            return false;
        }
        public bool UpdateBicycle(Bicycle? bicycle) // Entity Framework
        {
            if (bicycle == null)
                return false;

            if (DatabaseRepo.UpdateBicycleEF(bicycle, out Bicycle updatedBicycle))
            {
                Log.Information($"Updated bicycle: {updatedBicycle.ToString()}");
                return true;
            }
            return false;
        }
        public bool UpdateRent(Rent? rent)
        {
            if (rent == null)
                return false;

            bool vehicleTrueBicycleFalse = true;
            if (rent.VehicleID == null)
                vehicleTrueBicycleFalse = false;

            DisplayAllRents(rent.GetVehicleID(), vehicleTrueBicycleFalse);

            if (RentUpdateIsPossible(rent))
            {
                if (DatabaseRepo.UpdateRent(rent, out Rent newRent))
                {
                    Log.Information($"Updated rent: {newRent.ToString()}");
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool VehiclesIDExists(int id)
        {
            DatabaseRepo.GetAllVehicles(out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles);
            List<Vehicle> vehicles = [.. fossilFuelVehicles, .. electricVehicles];

            if (vehicles.Count > 0)
            {
                for (int i = 0; i < vehicles.Count; i++)
                {
                    if (vehicles[i].GetID() == id)
                        return true;
                }
                return false;
            }
            else
            {
                Log.Information("List is empty.\n");
                return false;
            }
        }
        public bool VehiclesIDExists(int id, out bool isElectric)
        {
            DatabaseRepo.GetAllVehicles(out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles);
            List<Vehicle> vehicles = [.. fossilFuelVehicles, .. electricVehicles];

            isElectric = false;

            if (vehicles.Count > 0)
            {
                for (int i = 0; i < vehicles.Count; i++)
                {
                    if (vehicles[i].GetID() == id)
                    {
                        if (vehicles[i] is ElectricVehicle)
                            isElectric = true;
                        else if (vehicles[i] is FossilFuelVehicle)
                            isElectric = false;

                        return true;
                    }
                }
                return false;
            }
            else
            {
                Log.Information("List is empty.\n");
                return false;
            }
        }
        public bool ClientsIDExists(int id)
        {
            List<Client> client = new(DatabaseRepo.GetAllClients());

            if (client.Count > 0)
            {
                for (int i = 0; i < client.Count; i++)
                {
                    if (client[i].GetID() == id)
                        return true;
                }
                return false;
            }
            else
            {
                Log.Information("List is empty.\n");
                return false;
            }
        }
        public bool BicyclesIDExists(int id)
        {
            List<Bicycle> bicycle = new(DatabaseRepo.GetAllBicyclesEF());

            if (bicycle.Count > 0)
            {
                for (int i = 0; i < bicycle.Count; i++)
                {
                    if (bicycle[i].ID == id)
                        return true;
                }
                return false;
            }
            else
            {
                Log.Information("List is empty.\n");
                return false;
            }
        }

        public bool RentsIDExists(int id)
        {
            List<Rent> rents = new(DatabaseRepo.GetAllRents());

            if (rents.Count > 0)
            {
                for (int i = 0; i < rents.Count; i++)
                {
                    if (rents[i].GetID() == id)
                        return true;
                }
                return false;
            }
            else
            {
                Log.Information("List is empty.\n");
                return false;
            }
        }

        // Rent starts on specific date (client picks up item at any time from 00:00:00).
        // Rent ends on specific date (client returns item at any time before 23:59:59). 
        public bool RentIsPossible(Rent newRent)
        {
            List<Rent> rents = DatabaseRepo.GetAllRents().ToList();
            if (rents.Count == 0)
            {
                Log.Information("Rent list is empty.\n");
                return true;
            };

            if (newRent.GetBicycleID() == null)
            {
                for (int i = 0; i < rents.Count; i++)
                {
                    // There is another rent of the desired vehicle
                    if (rents[i].GetVehicleID() == newRent.GetVehicleID())
                    {
                        // There is no desired to-date, only from-date
                        if (newRent.GetDateTo() == null)
                        {
                            // Another rent is without specified to-date
                            if (rents[i].GetDateTo() == null)
                                return false;
                            // Another rent does not end BEFORE desired from-date
                            if (rents[i].GetDateTo() > newRent.GetDateFrom())
                                return false;
                        }
                        // Another rent is without specified to-date
                        if (rents[i].GetDateTo() == null)
                        {
                            // Another rent's from-date is BEFORE OR ON desired to-date
                            if (rents[i].GetDateFrom() <= newRent.GetDateTo())
                                return false;
                            // Another rent's from-date is AFTER desired to-date
                            return true;
                        }
                        // Another rent is during the desired date range (inclusive)
                        if ((rents[i].GetDateFrom() <= newRent.GetDateFrom() && rents[i].GetDateTo() >= newRent.GetDateFrom() ||
                            (rents[i].GetDateFrom() <= newRent.GetDateTo() && rents[i].GetDateTo() >= newRent.GetDateTo())))
                            return false;
                        // Date ranges of existing and desired rents do not overlap
                        return true;
                    }
                }
                return true;
            }
            else
            {
                for (int i = 0; i < rents.Count; i++)
                {
                    // There is another rent of the desired bicycle
                    if (rents[i].GetBicycleID() == newRent.GetBicycleID())
                    {
                        // There is no desired to-date, only from-date
                        if (newRent.GetDateTo() == null)
                        {
                            // Another rent is without specified to-date
                            if (rents[i].GetDateTo() == null)
                                return false;
                            // Another rent does not end BEFORE desired from-date
                            if (rents[i].GetDateTo() > newRent.GetDateFrom())
                                return false;
                        }
                        // Another rent is without specified to-date
                        if (rents[i].GetDateTo() == null)
                        {
                            // Another rent's from-date is BEFORE OR ON desired to-date
                            if (rents[i].GetDateFrom() <= newRent.GetDateTo())
                                return false;
                            // Another rent's from-date is AFTER desired to-date
                            return true;
                        }
                        // Another rent is during the desired date range (inclusive)
                        if ((rents[i].GetDateFrom() <= newRent.GetDateFrom() && rents[i].GetDateTo() >= newRent.GetDateFrom() ||
                            (rents[i].GetDateFrom() <= newRent.GetDateTo() && rents[i].GetDateTo() >= newRent.GetDateTo())))
                            return false;
                        // Date ranges of existing and desired rents do not overlap
                        return true;
                    }
                }
                return true;
            }
        }
        // Rent update is possible for from- and to-dates
        public bool RentUpdateIsPossible(Rent rentUpdate)
        {
            List<Rent> rents = new(DatabaseRepo.GetAllRents());

            if (rentUpdate.GetBicycleID() == null)
            {
                for (int i = 0; i < rents.Count; i++)
                {
                    // There is another rent (different unique ID) of the same vehicle
                    if (rents[i].GetVehicleID() == rentUpdate.GetVehicleID())
                    {
                        // There is no desired to-date, only from-date
                        if (rentUpdate.GetDateTo() == null)
                        {
                            // Another rent is without specified to-date
                            if (rents[i].GetDateTo() == null)
                                return false;
                            // Another rent does not end BEFORE desired from-date
                            if (rents[i].GetDateTo() > rentUpdate.GetDateFrom())
                                return false;
                        }
                        // Another rent is without specified to-date
                        if (rents[i].GetDateTo() == null)
                        {
                            // Another rent's from-date is BEFORE OR ON desired to-date
                            if (rents[i].GetDateFrom() <= rentUpdate.GetDateTo())
                                return false;
                            // Another rent's from-date is AFTER desired to-date
                            return true;
                        }
                        // Another rent is during the desired date range (inclusive)
                        if ((rents[i].GetDateFrom() <= rentUpdate.GetDateFrom() && rents[i].GetDateTo() >= rentUpdate.GetDateFrom() ||
                            (rents[i].GetDateFrom() <= rentUpdate.GetDateTo() && rents[i].GetDateTo() >= rentUpdate.GetDateTo())))
                            return false;
                        // Date ranges of existing and desired rents do not overlap
                        return true;
                    }
                }
                return true;
            }
            else
            {
                for (int i = 0; i < rents.Count; i++)
                {
                    // There is another rent (different unique ID) of the same bicycle
                    if (rents[i].GetBicycleID() == rentUpdate.GetBicycleID())
                    {
                        // There is no desired to-date, only from-date
                        if (rentUpdate.GetDateTo() == null)
                        {
                            // Another rent is without specified to-date
                            if (rents[i].GetDateTo() == null)
                                return false;
                            // Another rent does not end BEFORE desired from-date
                            if (rents[i].GetDateTo() > rentUpdate.GetDateFrom())
                                return false;
                        }
                        // Another rent is without specified to-date
                        if (rents[i].GetDateTo() == null)
                        {
                            // Another rent's from-date is BEFORE OR ON desired to-date
                            if (rents[i].GetDateFrom() <= rentUpdate.GetDateTo())
                                return false;
                            // Another rent's from-date is AFTER desired to-date
                            return true;
                        }
                        // Another rent is during the desired date range (inclusive)
                        if ((rents[i].GetDateFrom() <= rentUpdate.GetDateFrom() && rents[i].GetDateTo() >= rentUpdate.GetDateFrom() ||
                            (rents[i].GetDateFrom() <= rentUpdate.GetDateTo() && rents[i].GetDateTo() >= rentUpdate.GetDateTo())))
                            return false;
                        // Date ranges of existing and desired rents do not overlap
                        return true;
                    }
                }
                return true;
            }
        }

        //public void PrintCollection(IEnumerable<object> collection)
        //{
        //    int count = collection.Count();
        //    if (count == 0)
        //    {
        //        Log.Information("The collection is empty.\n");
        //        return;
        //    }

        //    if (collection.FirstOrDefault() is ElectricVehicle)
        //    {
        //        foreach (object obj in collection)
        //        {
        //            ElectricVehicle electricVehicle = (ElectricVehicle)obj;
        //            Console.WriteLine(electricVehicle.ToString());
        //        }
        //        Console.WriteLine();
        //    }
        //    else if (collection.FirstOrDefault() is FossilFuelVehicle)
        //    {
        //        foreach (object obj in collection)
        //        {
        //            FossilFuelVehicle fossilFuelVehicle = (FossilFuelVehicle)obj;
        //            Console.WriteLine(fossilFuelVehicle.ToString());
        //        }
        //        Console.WriteLine();
        //    }
        //    else if (collection.FirstOrDefault() is Client)
        //    {
        //        foreach (object obj in collection)
        //        {
        //            Client client = (Client)obj;
        //            Console.WriteLine($"ID:{client.GetID()} {client.ToString()}");
        //        }
        //        Console.WriteLine();
        //    }
        //    else if (collection.FirstOrDefault() is Rent)
        //    {
        //        foreach (object obj in collection)
        //        {
        //            Rent rent = (Rent)obj;
        //            Console.WriteLine($"ID:{rent.GetID()} {rent.ToString()}");
        //        }
        //        Console.WriteLine();
        //    }
        //    else
        //    {
        //        Log.Error("Collection object is not ElectricVehicle, FossilFuelVehicle, Client or Rent\n");
        //        return;
        //    }
        //}

        //public int GetIndexFromID(int id)
        //{
        //    if (Vehicles.Count > 0)
        //    {
        //        for (int i = 0; i < Vehicles.Count; i++)
        //        {
        //            if (Vehicles[i] != null && Vehicles[i].GetID() == id)
        //                return i;
        //        }
        //        Log.Information("\nError: vehicle ID not found.\n");
        //        return -1;
        //    }
        //    else
        //    {
        //        Log.Information("Fleet is empty.\n");
        //        return -1;
        //    }
        //}
    }
}
