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
    internal class RentService : IRentService
    {
        private IDatabaseRepository DatabaseRepository { get; set; }

        public RentService(string connectionString)
        {
            DatabaseRepository = new DatabaseRepository(connectionString);
        }

        public bool RegisterVehicle(ElectricVehicle electricVehicle)
        {
            if (DatabaseRepository.InsertVehicle(electricVehicle, out ElectricVehicle newVehicle))
            {
                Console.WriteLine($"New electric vehicle: {newVehicle.ToString()}");
                return true;
            }
            return false;
        }
        public bool RegisterVehicle(FossilFuelVehicle fossilFuelVehicle)
        {
            if (DatabaseRepository.InsertVehicle(fossilFuelVehicle, out FossilFuelVehicle newVehicle))
            {
                Console.WriteLine($"New fossil fuel vehicle: {newVehicle.ToString()}");
                return true;
            }
            return false;
        }
        public bool RegisterClient(Client client)
        {
            if (DatabaseRepository.InsertClient(client, out Client newClient))
            {
                Console.WriteLine($"New client: {newClient.ToString()}");
                return true;
            }
            return false;
        }
        public bool RegisterRent(Rent rent)
        {
            if (RentIsPossible(rent))
            {
                if (DatabaseRepository.InsertRent(rent))
                {
                    Console.WriteLine($"New rent agreement: {rent.ToString()}");
                    return true;
                }
                return false;
            }
            else
            {
                Console.WriteLine("Vehicle is occupied now or in the desired date range.\n");
            }
            return false;
        }

        public int DisplayAllVehicles()
        {
            int items = 0;
            bool notEmpty = DatabaseRepository.GetAllVehicles(out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles);

            if (!notEmpty)
            {
                Console.WriteLine("There are no vehicles in the database\n");
                return 0;
            }

            if (fossilFuelVehicles.Count() > 0)
            {
                foreach (FossilFuelVehicle fossilFuelVehicle in fossilFuelVehicles)
                {
                    Console.WriteLine(fossilFuelVehicle.ToString());
                    items++;
                }
            }
            if (electricVehicles.Count() > 0)
            {
                foreach (ElectricVehicle electricVehicle in electricVehicles)
                {
                    Console.WriteLine(electricVehicle.ToString());
                    items++;
                }
            }
            Console.WriteLine();
            return items;
        }
        //public int DisplayAllVehicles()
        //{
        //    int count = 0;
        //    IEnumerable<Vehicle>? vehicles = (IEnumerable<Vehicle>?)DatabaseRepository.GetAllVehicles();

        //    if (vehicles == null)
        //    {
        //        Console.WriteLine("There are no vehicles in the database\n");
        //        return 0;
        //    }

        //    foreach (Vehicle vehicle in vehicles)
        //    {
        //        Console.WriteLine(vehicle.ToString());
        //        count++;
        //    }
        //    Console.WriteLine();
        //    return count;
        //}
        public int DisplayAllElectricVehicles()
        {
            int count = 0;
            List<ElectricVehicle>? vehicles = (List<ElectricVehicle>?)DatabaseRepository.GetAllVehicles(true);

            if (vehicles == null)
            {
                Console.WriteLine("There are no electric vehicles in the database\n");
                return 0;
            }

            foreach (ElectricVehicle vehicle in vehicles)
            {
                Console.WriteLine(vehicle.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public int DisplayAllFossilFuelVehicles()
        {
            int count = 0;
            IEnumerable<FossilFuelVehicle>? vehicles = (IEnumerable<FossilFuelVehicle>?)DatabaseRepository.GetAllVehicles(false);

            if (vehicles == null)
            {
                Console.WriteLine("There are no fossil fuel vehicles in the database\n");
                return 0;
            }

            foreach (FossilFuelVehicle vehicle in vehicles)
            {
                Console.WriteLine(vehicle.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public int DisplayAllClients()
        {
            int count = 0;
            IEnumerable<Client>? clients = DatabaseRepository.GetAllClients();

            if (clients == null)
            {
                Console.WriteLine("There are no clients in the database\n");
                return 0;
            }

            foreach (Client client in clients)
            {
                Console.WriteLine(client.ToString());
                count++;
            }
            Console.WriteLine();
            return count;
        }
        public int DisplayAllRents()
        {
            int count = 0;

            IEnumerable<Rent>? rents = DatabaseRepository.GetAllRents();

            if (rents == null)
            {
                Console.WriteLine("There are no rents in the database\n");
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
        public int DisplayAllRents(int vehicleID)
        {
            int count = 0;
            IEnumerable<Rent>? rents = DatabaseRepository.GetAllRents(vehicleID);

            if (rents == null)
            {
                Console.WriteLine("There are no rents of this vehicle in the database\n");
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

        public Vehicle GetVehicle(int ID)
        {
            return DatabaseRepository.GetVehicle(ID);
        }
        public Client GetClient(int ID)
        {
            return DatabaseRepository.GetClient(ID);
        }
        public Rent GetRent(int ID)
        {
            return DatabaseRepository.GetRent(ID);
        }

        public bool DeleteVehicle(int ID)
        {
            if (DatabaseRepository.DeleteVehicle(ID))
            {
                Console.WriteLine($"Deleted vehicle with ID:{ID}");
                return true;
            }
            return false;
        }
        public bool DeleteClient(int ID)
        {
            if (DatabaseRepository.DeleteClient(ID))
            {
                Console.WriteLine($"Deleted client with ID:{ID}");
                return true;
            }
            return false;
        }
        public bool DeleteRent(int ID)
        {
            if (DatabaseRepository.DeleteRent(ID))
            {
                Console.WriteLine($"Deleted rent with ID:{ID}");
                return true;
            }
            return false;
        }

        //public bool UpdateElectricVehicle(ElectricVehicle electricVehicle)
        //{
        //    if (DatabaseRepository.UpdateVehicle(electricVehicle, true))
        //    {
        //        Console.WriteLine($"Updated vehicle: {electricVehicle.ToString()}");
        //        return true;
        //    }
        //    return false;
        //}
        //public bool UpdateFossilFuelVehicle(FossilFuelVehicle fossilFuelVehicle)
        //{
        //    if (DatabaseRepository.UpdateVehicle(fossilFuelVehicle, false))
        //    {
        //        Console.WriteLine($"Updated vehicle: {fossilFuelVehicle.ToString()}");
        //        return true;
        //    }
        //    return false;
        //}

        public bool UpdateVehicle(object vehicle)
        {
            if (DatabaseRepository.UpdateVehicle(vehicle, out object updatedVehicle))
            {
                if (vehicle is ElectricVehicle)
                {
                    Console.WriteLine($"Updated vehicle: {((ElectricVehicle)updatedVehicle).ToString()}");
                    return true;
                }
                else if (vehicle is FossilFuelVehicle)
                {
                    Console.WriteLine($"Updated vehicle: {((FossilFuelVehicle)updatedVehicle).ToString()}");
                    return true;
                }
            }
            return false;
        }
        public bool UpdateClient(Client client)
        {
            if (DatabaseRepository.UpdateClient(client, out Client newCLient))
            {
                Console.WriteLine($"Updated client: {newCLient.ToString()}");
                return true;
            }
            return false;
        }
        public bool UpdateRent(Rent rent)
        {
            DisplayAllRents(rent.GetVehicleID());

            if (RentUpdateIsPossible(rent))
            {
                if (DatabaseRepository.UpdateRent(rent, out Rent newRent))
                {
                    Console.WriteLine($"Updated client: {newRent.ToString()}");
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool VehiclesIDExists(int id)
        {
            DatabaseRepository.GetAllVehicles(out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles);
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
                Console.WriteLine("List is empty.\n");
                return false;
            }
        }
        public bool VehiclesIDExists(int id, out bool isElectric)
        {
            DatabaseRepository.GetAllVehicles(out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles);
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
                Console.WriteLine("List is empty.\n");
                return false;
            }
        }
        public bool ClientsIDExists(int id)
        {
            List<Client> client = new(DatabaseRepository.GetAllClients());

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
                Console.WriteLine("List is empty.\n");
                return false;
            }
        }

        public bool RentsIDExists(int id)
        {
            List<Rent> rents = new(DatabaseRepository.GetAllRents());

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
                Console.WriteLine("List is empty.\n");
                return false;
            }
        }

        // Rent starts on specific date (client picks up car at any time from 00:00:00).
        // Rent ends on specific date (client returns car at any time before 23:59:59). 
        public bool RentIsPossible(Rent newRent)
        {
            IEnumerable<Rent>? allRents = DatabaseRepository.GetAllRents();
            if (allRents == null)
                return true;

            List<Rent> rents = new(allRents);

            if (rents.Count > 0)
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
                Console.WriteLine("List is empty.\n");
                return true;
            }
        }
        // Rent update is possible for from- and to-dates
        public bool RentUpdateIsPossible(Rent rentUpdate)
        {
            List<Rent> rents = new(DatabaseRepository.GetAllRents());
            if (rents.Count > 0)
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
                Console.WriteLine("List is empty.\n");
                return false;
            }
        }

        //public void PrintCollection(IEnumerable<object> collection)
        //{
        //    int count = collection.Count();
        //    if (count == 0)
        //    {
        //        Console.WriteLine("The collection is empty.\n");
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
        //        Console.WriteLine("Collection object is not ElectricVehicle, FossilFuelVehicle, Client or Rent\n");
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
        //        Console.WriteLine("\nError: vehicle ID not found.\n");
        //        return -1;
        //    }
        //    else
        //    {
        //        Console.WriteLine("Fleet is empty.\n");
        //        return -1;
        //    }
        //}
    }
}
