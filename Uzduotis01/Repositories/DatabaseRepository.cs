using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using Serilog;

namespace Uzduotis01
{
    // Tai, ką laikėme DatabaseService, turi tapti DatabaseRepository ir turi būti laikoma aplanke Repositories.
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString;
        private RentDBContext _dbContext;
        public DatabaseRepository(string connectionString)
        {
            _connectionString = connectionString;
            _dbContext = new();
        }

        private void DeleteOldRents()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string today = DateTime.Today.ToString("yyyy-MM-dd");
                const string sql2 = @"DELETE FROM Rents WHERE (DateTo < @today);";
                db.Execute(sql2, new { today });
            }
        }

        public bool GetAllVehicles(out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql1 = @"
                    SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN, e.BatteryCapacity
                    FROM Vehicles v INNER JOIN ElectricVehicles e ON v.ID = e.ID
                    ORDER BY v.ID;";
                const string sql2 = @"
                    SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN, f.TankCapacity
                    FROM Vehicles v INNER JOIN FossilFuelVehicles f ON v.ID = f.ID
                    ORDER BY v.ID;";

                electricVehicles = db.Query<ElectricVehicle>(sql1);
                fossilFuelVehicles = db.Query<FossilFuelVehicle>(sql2);

                if (electricVehicles.Count() < 1 && fossilFuelVehicles.Count() < 1)
                {
                    return false;
                }
                return true;
            }
        }
        public bool GetAllVehicles(string phrase, out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql1 = @"
                    SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN, e.BatteryCapacity
                    FROM Vehicles v INNER JOIN ElectricVehicles e ON v.ID = e.ID
                    WHERE v.Make LIKE(@phrase) OR v.Model LIKE(@phrase)
                    ORDER BY v.ID;";
                const string sql2 = @"
                    SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN, f.TankCapacity
                    FROM Vehicles v INNER JOIN FossilFuelVehicles f ON v.ID = f.ID
                    WHERE v.Make LIKE('%@phrase%') OR v.Model LIKE('%@phrase%')
                    ORDER BY v.ID;";

                electricVehicles = db.Query<ElectricVehicle>(sql1, new { phrase = $"%{phrase}%" });
                fossilFuelVehicles = db.Query<FossilFuelVehicle>(sql2, new { phrase = $"%{phrase}%" });

                if (electricVehicles.Count() < 1 && fossilFuelVehicles.Count() < 1)
                {
                    return false;
                }
                return true;
            }
        }
        public IEnumerable<Vehicle>? GetAllVehicles(bool isElectric)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (isElectric)
                {
                    const string sql = @"
                    SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN, e.BatteryCapacity
                    FROM Vehicles v INNER JOIN ElectricVehicles e ON v.ID = e.ID
                    ORDER BY v.ID;";

                    IEnumerable<ElectricVehicle>? vehicles = db.Query<ElectricVehicle>(sql);
                    if (vehicles.Count() < 1)
                    {
                        return null;
                    }
                    return vehicles;
                }
                else
                {
                    const string sql = @"
                    SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN, f.TankCapacity
                    FROM Vehicles v INNER JOIN FossilFuelVehicles f ON v.ID = f.ID
                    ORDER BY v.ID;";

                    IEnumerable<FossilFuelVehicle>? vehicles = db.Query<FossilFuelVehicle>(sql);
                    if (vehicles.Count() < 1)
                    {
                        return null;
                    }
                    return vehicles;
                }
            }
        }
        public IEnumerable<Client>? GetAllClients()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                SELECT * FROM Clients ORDER BY ID";

                IEnumerable<Client>? clients = db.Query<Client>(sql);
                if (clients.Count() < 1)
                {
                    return null;
                }
                return clients;
            }
        }
        public IEnumerable<Client>? GetAllClients(string phrase)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                SELECT * FROM Clients
                WHERE FullName LIKE (@phrase)
                ORDER BY ID";

                IEnumerable<Client>? clients = db.Query<Client>(sql, new { phrase = $"%{phrase}%" });
                if (clients.Count() < 1)
                {
                    return null;
                }
                return clients;
            }
        }
        public IEnumerable<Rent> GetAllRents()
        {
            DeleteOldRents();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                SELECT * FROM Rents ORDER BY ID";
                IEnumerable<Rent> rents = db.Query<Rent>(sql);
                return rents;
            }
        }
        public IEnumerable<Rent>? GetAllRents(int? itemID, bool vehicleTrueBicycleFalse)
        {
            DeleteOldRents();
            if (vehicleTrueBicycleFalse)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
                    SELECT * FROM Rents WHERE VehicleID = @itemID";

                    IEnumerable<Rent>? rents = db.Query<Rent>(sql, new { itemID });
                    if (rents.Count() < 1)
                    {
                        return null;
                    }
                    return rents;
                }
            }
            else
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
                    SELECT * FROM Rents WHERE BicycleID = @itemID";

                    IEnumerable<Rent>? rents = db.Query<Rent>(sql, new { itemID });
                    if (rents.Count() < 1)
                    {
                        return null;
                    }
                    return rents;
                }
            }
            
        }

        public Vehicle GetVehicle(int ID)
        {
            using (IDbConnection db1 = new SqlConnection(_connectionString))
            {
                const string sql1 = @"
                        SELECT * FROM Vehicles WHERE ID = @ID";

                Vehicle vehicle = db1.QueryFirst<Vehicle>(sql1, new { ID });

                return vehicle;
            }
        }
        public Client? GetClient(int ID)
        {
            using (IDbConnection db1 = new SqlConnection(_connectionString))
            {
                const string sql1 = @"
                        SELECT * FROM Clients WHERE ID = @ID";

                Client client = db1.QueryFirst<Client>(sql1, new { ID });

                return client;
            }
        }
        public Rent? GetRent(int ID)
        {
            using (IDbConnection db1 = new SqlConnection(_connectionString))
            {
                const string sql1 = @"
                        SELECT * FROM Clients WHERE ID = @ID";

                Rent rent = db1.QueryFirst<Rent>(sql1, new { ID });

                return rent;
            }
        }

        public bool InsertVehicle(ElectricVehicle vehicle, out ElectricVehicle newVehicle)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                newVehicle = vehicle;
                string make = vehicle.GetMake();
                string model = vehicle.GetModel();
                int productionYear = vehicle.GetProductionYear();

                const string sql1 = @"INSERT INTO Vehicles (Make, Model, ProductionYear) VALUES (@make, @model, @productionYear);";
                if (db.Execute(sql1, new { make, model, productionYear }) == 0)
                    return false;

                const string sql2 = @"SELECT TOP(1) * FROM Vehicles ORDER BY ID DESC;";
                newVehicle = db.QueryFirst<ElectricVehicle>(sql2);
                double batteryCapacity = vehicle.GetBatteryCapacity();
                int id = newVehicle.GetID();
                newVehicle.SetBatteryCapacity(batteryCapacity);

                const string sql3 = @"INSERT INTO ElectricVehicles (ID, BatteryCapacity) VALUES (@id, @batteryCapacity);";
                if (db.Execute(sql3, new { id, batteryCapacity }) > 0)
                    return true;
            }
            Log.Error("Unexpected error: insertion not performed\n");
            return false;
        }
        public bool InsertVehicle(FossilFuelVehicle vehicle, out FossilFuelVehicle newVehicle)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                newVehicle = vehicle;
                string make = vehicle.GetMake();
                string model = vehicle.GetModel();
                int productionYear = vehicle.GetProductionYear();

                const string sql1 = @"INSERT INTO Vehicles (Make, Model, ProductionYear) VALUES (@make, @model, @productionYear);";
                if (db.Execute(sql1, new { make, model, productionYear }) == 0)
                    return false;

                const string sql2 = @"SELECT TOP(1) * FROM Vehicles ORDER BY ID DESC;";
                newVehicle = db.QueryFirst<FossilFuelVehicle>(sql2);
                double tankCapacity = vehicle.GetTankCapacity();
                int id = newVehicle.GetID();
                newVehicle.SetTankCapacity(tankCapacity);

                const string sql3 = @"INSERT INTO FossilFuelVehicles (ID, TankCapacity) VALUES (@id, @tankCapacity);";
                if (db.Execute(sql3, new { id, tankCapacity }) > 0)
                    return true;
            }
            Log.Error("Unexpected error: insertion not performed\n");
            return false;
        }
        public bool InsertClient(Client client, out Client newClient)
        {
            newClient = client;
            string fullName = client.GetFullName();
            long personalID = client.GetPersonalID();

            if (personalID != 0)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"INSERT INTO Clients (FullName, PersonalID) VALUES (@fullName, @personalID);";
                    if (db.Execute(sql, new { fullName, personalID }) > 0)
                    {
                        const string sql2 = @"SELECT TOP(1) * FROM Clients ORDER BY ID DESC;";
                        newClient = db.QueryFirst<Client>(sql2);
                        return true;
                    }
                }
                Log.Error("Unexpected error: insertion not performed\n");
                return false;
            }

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"INSERT INTO Clients (FullName) VALUES (@fullName);";
                if (db.Execute(sql, new { fullName } ) > 0)
                {
                    const string sql2 = @"SELECT TOP(1) * FROM Clients ORDER BY ID DESC;";
                    newClient = db.QueryFirst<Client>(sql2);
                    return true;
                }
            }
            Log.Error("Unexpected error: insertion not performed\n");
            return false;
        }
        public bool InsertRent(Rent rent, out Rent newRent)
        {
            newRent = rent;

            DeleteOldRents();
            if (rent.GetDateTo() == null)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"INSERT INTO Rents (VehicleID, ClientID, DateFrom) VALUES (@VehicleID, @ClientID, @DateFrom);";
                    if (db.Execute(sql, new { VehicleID = rent.GetVehicleID(), ClientID = rent.GetClientID(), DateFrom = rent.GetDateFrom() }) > 0)
                    {
                        const string sql2 = @"SELECT TOP(1) * FROM Rents ORDER BY ID DESC;";
                        newRent = db.QueryFirst<Rent>(sql2);
                        return true;
                    }
                }
            }
            else
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"INSERT INTO Rents (VehicleID, ClientID, DateFrom, DateTo) VALUES (@VehicleID, @ClientID, @DateFrom, @DateTo);";
                    if (db.Execute(sql, new { VehicleID = rent.GetVehicleID(), ClientID = rent.GetClientID(), DateFrom = rent.GetDateFrom(),DateTo = rent.GetDateTo()}) > 0)
                    {
                        const string sql2 = @"SELECT TOP(1) * FROM Rents ORDER BY ID DESC;";
                        newRent = db.QueryFirst<Rent>(sql2);
                        return true;
                    }
                }
            }
            Log.Error("Unexpected error: insertion not performed\n");
            return false;
        }

        public bool DeleteVehicle(int ID)
        {
            IEnumerable<Rent>? rents = GetAllRents();
            if (rents != null)
            {
                foreach (Rent rent in rents)
                {
                    if (rent.GetVehicleID() == ID)
                    {
                        Log.Error("The car is still being rented and cannot be deleted\n");
                        return false;
                    }
                }
            }

            foreach (ElectricVehicle electricVehicle in GetAllVehicles(true))
            {
                if (electricVehicle.GetID() == ID)
                {
                    using (IDbConnection db1 = new SqlConnection(_connectionString))
                    {
                        const string sql1 = @"
                        DELETE FROM ElectricVehicles WHERE ID = @ID";
                        if (db1.Execute(sql1, new { ID }) == 0)
                        {
                            Log.Error("ID was not deleted from Electric Vehicles\n"); // just in case, shouldn't happen
                            return false;
                        }
                    }
                    using (IDbConnection db2 = new SqlConnection(_connectionString))
                    {
                        const string sql2 = @"
                                DELETE FROM Vehicles WHERE ID = @ID";
                        if (db2.Execute(sql2, new { ID }) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            Log.Error("ERROR: ID was not deleted from Vehicles\n");
                            return false;
                        }
                    }
                }
            }

            foreach (FossilFuelVehicle fossilFuelVehicle in GetAllVehicles(false))
            {
                if (fossilFuelVehicle.GetID() == ID)
                {
                    using (IDbConnection db1 = new SqlConnection(_connectionString))
                    {
                        const string sql1 = @"
                        DELETE FROM FossilFuelVehicles WHERE ID = @ID";
                        if (db1.Execute(sql1, new { ID }) == 0)
                        {
                            Log.Error("ID was not deleted from Fossil Fuel Vehicles\n"); // just in case, shouldn't happen
                            return false;
                        }
                    }
                    using (IDbConnection db2 = new SqlConnection(_connectionString))
                    {
                        const string sql2 = @"
                                DELETE FROM Vehicles WHERE ID = @ID";
                        if (db2.Execute(sql2, new { ID }) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            Log.Error("ERROR: ID was not deleted from Vehicles\n");
                            return false;
                        }
                    }
                }
            }
            Log.Error("Unexpected error: deletion not performed\n");
            return false;
        }
        public bool DeleteClient(int ID)
        {
            IEnumerable<Rent>? rents = GetAllRents();
            if (rents != null)
            {
                foreach (Rent rent in rents)
                {
                    if (rent.GetClientID() == ID)
                    {
                        Log.Error("The client is still renting a car and cannot be deleted\n");
                        return false;
                    }
                }
            }

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                                DELETE FROM Clients WHERE ID = @ID";
                if (db.Execute(sql, new { ID }) > 0)
                    return true;
                else
                    Log.Error("ERROR: ID was not deleted from Clients\n");
            }
            Log.Error("Unexpected error: deletion not performed\n");
            return false;
        }
        public bool DeleteRent(int ID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                                DELETE FROM Rents WHERE ID = @ID";
                if (db.Execute(sql, new { ID }) > 0)
                    return true;
                else
                    Log.Error("ID was not deleted from Rents\n");
            }
            Log.Error("Unexpected error: deletion not performed\n");
            return false;
        }

        public bool UpdateVehicle(object? vehicle, out object updatedVehicle)
        {
            updatedVehicle = vehicle;
            if (vehicle == null)
                return false;

            if (vehicle is ElectricVehicle)
            {
                ElectricVehicle electricVehicle = (ElectricVehicle)vehicle;
                int id = electricVehicle.GetID();

                using (IDbConnection db1 = new SqlConnection(_connectionString))
                {
                    string make = electricVehicle.GetMake();
                    string model = electricVehicle.GetModel();
                    int productionYear = electricVehicle.GetProductionYear();
                    string vin = electricVehicle.GetVIN();
                    const string sql1 = @"UPDATE Vehicles 
                        SET Make = @make, Model = @model, ProductionYear = @productionYear, VIN = @vin
                        WHERE ID = @id";
                    if (db1.Execute(sql1, new { make, model, productionYear, vin, id }) == 0)
                    {
                        Log.Error("ERROR: Vehicle was not updated\n");
                        return false;
                    }
                }
                using (IDbConnection db2 = new SqlConnection(_connectionString))
                {
                    double batteryCapacity = electricVehicle.GetBatteryCapacity();
                    const string sql2 = @"UPDATE ElectricVehicles 
                        SET BatteryCapacity = @batteryCapacity WHERE ID = @id";

                    if (db2.Execute(sql2, new { batteryCapacity, id }) == 0)
                    {
                        Log.Error("ERROR: Electric vehicle was not updated\n");
                        return false;
                    }
                }
                using (IDbConnection db3 = new SqlConnection(_connectionString))
                {
                    const string sql3 = @"SELECT * FROM Vehicles v
                        INNER JOIN ElectricVehicles e ON v.ID = e.ID WHERE v.ID = @id;";
                    updatedVehicle = db3.QueryFirst<ElectricVehicle>(sql3, new { id });
                    return true;
                }
            }
            else if (vehicle is FossilFuelVehicle)
            {
                FossilFuelVehicle fossilFuelVehicle = (FossilFuelVehicle)vehicle;
                int id = fossilFuelVehicle.GetID();

                using (IDbConnection db1 = new SqlConnection(_connectionString))
                {
                    string make = fossilFuelVehicle.GetMake();
                    string model = fossilFuelVehicle.GetModel();
                    int productionYear = fossilFuelVehicle.GetProductionYear();
                    string vin = fossilFuelVehicle.GetVIN();
                    const string sql1 = @"UPDATE Vehicles 
                        SET Make = @make, Model = @model, ProductionYear = @productionYear, VIN = @vin
                        WHERE ID = @id";
                    if (db1.Execute(sql1, new { make, model, productionYear, vin, id }) == 0)
                    {
                        Log.Error("ERROR: Vehicle was not updated\n");
                        return false;
                    }
                }
                using (IDbConnection db2 = new SqlConnection(_connectionString))
                {
                    double tankCapacity = fossilFuelVehicle.GetTankCapacity();

                    const string sql2 = @"
                    UPDATE FossilFuelVehicles 
                    SET TankCapacity = @tankCapacity
                    WHERE ID = @id";
                    if (db2.Execute(sql2, new { tankCapacity, id }) == 0)
                    {
                        Log.Error("ERROR: Vehicle was not updated\n");
                        return false;
                    }
                }
                using (IDbConnection db3 = new SqlConnection(_connectionString))
                {
                    const string sql3 = @"SELECT * FROM Vehicles v" +
                        "INNER JOIN FossilFuelVehicles f ON v.ID = f.ID WHERE v.ID = @id;";
                    updatedVehicle = db3.QueryFirst<FossilFuelVehicle>(sql3, new { id });
                return true;
                }
            }
            Log.Error("ERROR: Vehicle was neither Electric nor Fossil Fuel.\n");
            return false;
        }
        public bool UpdateClient(Client? client, out Client updatedClient)
        {
            updatedClient = client;
            if (client == null)
                return false;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int id = client.GetID();
                const string sql = @"
                UPDATE Clients 
                SET FullName = @FullName, PersonalID = @PersonalID
                WHERE ID = @ID";
                if (db.Execute(sql, new { fullName = client.GetFullName(), personalID = client.GetPersonalID(), id }) == 0)
                {
                    Log.Error("ERROR: Client was not updated\n");
                    return false;
                }

                const string sql3 = @"SELECT * FROM Clients WHERE ID = @id;";
                updatedClient = db.QueryFirst<Client>(sql3, new { id });
                return true;
            }
        }
        public bool UpdateRent(Rent? rent, out Rent updatedRent)
        {
            updatedRent = rent;
            if (rent == null)
                return false;

            DeleteOldRents();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                DateTime dateFrom = rent.GetDateFrom();
                DateTime? dateTo = rent.GetDateTo();
                int id = rent.GetID();
                const string sql = @"
                UPDATE Rents 
                SET DateFrom = @dateFrom, DateTo = @dateTo
                WHERE ID = @id";
                if (db.Execute(sql, new { dateFrom, dateTo, id }) == 0)
                {
                    Log.Error("ERROR: Rent was not updated\n");
                    return false;
                }

                const string sql3 = @"SELECT * FROM Rents WHERE ID = @id;";
                updatedRent = db.QueryFirst<Rent>(sql3, new { id });
                return true;
            }
        }


        // Entity Framework Client insert, update, delete, get by ID, get all, search
        public bool InsertClientEF(Client? client, out Client newClient)
        {
            newClient = client;
            if (client == null)
                return false;

            _dbContext.Add(client);
            _dbContext.SaveChanges();
            if (client.ID > 0)
            {
                newClient = client;
                return true;
            }
            _dbContext.Remove(client);
            _dbContext.SaveChanges();
            return false;
        }
        public bool UpdateClientEF(Client? client, out Client updatedClient)
        {
            // Keep updated object as updatedClient
            updatedClient = client;
            if (client == null)
                return false;
            // Find current object from DB and keep as client
            client = _dbContext.Clients.Find(client.ID);
            // Update value in DB
            _dbContext.Update(updatedClient);
            _dbContext.SaveChanges();
            // Check if updatedClient (returned from DB) was updated
            if (!updatedClient.Equals(client))
                return true;
            return false;
        }
        public bool DeleteClientEF(int ID)
        {
            IEnumerable<Rent>? rents = GetAllRents();
            if (rents != null)
            {
                foreach (Rent rent in rents)
                {
                    if (rent.GetClientID() == ID)
                    {
                        Log.Error("The client is still renting something and cannot be deleted\n");
                        return false;
                    }
                }
            }

            Client? client = GetClientEF(ID);

            _dbContext.Clients.Remove(client);
            _dbContext.SaveChanges();

            if (!_dbContext.Clients.Any(x => x.ID == ID))
                return true;
            else
                Log.Error("ERROR: ID was not deleted from Clients\n");
            return false;
        }
        public Client? GetClientEF(int ID)
        {
            return _dbContext.Clients.Find(ID);
        }
        public IEnumerable<Client> GetAllClientsEF()
        {
            return _dbContext.Clients.ToList();
        }
        public IEnumerable<Client> GetAllClientsEF(string phrase)
        {
            Regex? rx = new("*phrase*", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            return _dbContext.Clients.Where(x => rx.IsMatch(Regex.Replace(x.FullName, @"\s+", ""))).ToList();
        }


        // Entity Framework Bicycle insert, update, get by ID, get all, search
        public bool InsertBicycleEF(Bicycle? bicycle, out Bicycle newBicycle)
        {
            newBicycle = bicycle;
            if (bicycle == null)
                return false;

            var changes = _dbContext.Add(bicycle);
            _dbContext.SaveChanges();
            if (bicycle.ID > 0)
            {
                newBicycle = bicycle;
                return true;
            }
            return false;
        }
        public bool UpdateBicycleEF(Bicycle bicycle, out Bicycle updatedBicycle)
        {
            // Keep updated object as updatedBicycle
            updatedBicycle = bicycle;
            if (bicycle == null)
                return false;
            // Find current object from DB and keep as bicycle
            bicycle = _dbContext.Bicycles.Find(bicycle.ID);
            // Update value in DB
            _dbContext.Update(updatedBicycle);
            _dbContext.SaveChanges();
            // Check if updatedBicycle (returned from DB) was updated
            if (!updatedBicycle.Equals(bicycle))
                return true;
            return false;
        }
        public bool DeleteBicycleEF(int ID)
        {
            IEnumerable<Rent>? rents = GetAllRents();
            if (rents != null)
            {
                foreach (Rent rent in rents)
                {
                    if (rent.GetBicycleID() == ID)
                    {
                        Log.Error("The bicycle is still being rented cannot be deleted\n");
                        return false;
                    }
                }
            }

            Bicycle? bicycle = GetBicycleEF(ID);

            _dbContext.Bicycles.Remove(bicycle);
            _dbContext.SaveChanges();

            if (!_dbContext.Bicycles.Any(x => x.ID == ID))
                return true;
            else
                Log.Error("ERROR: ID was not deleted from Clients\n");
            return false;
        }
        public Bicycle? GetBicycleEF(int ID)
        {
            return _dbContext.Bicycles.Find(ID);
        }
        public IEnumerable<Bicycle> GetAllBicyclesEF()
        {
            return _dbContext.Bicycles.ToList();
        }
        public IEnumerable<Bicycle> GetAllBicyclesEF(string phrase)
        {
            Regex? rx = new(Regex.Replace("*phrase*", @"\s+", ""), RegexOptions.IgnoreCase);
            return _dbContext.Bicycles.Where(x => rx.IsMatch(Regex.Replace(x.Name, @"\s+", ""))).ToList();
        }
    }
}
