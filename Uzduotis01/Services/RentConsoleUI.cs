namespace Uzduotis01
{
    // Programos valdymui su konsole turi būti sukurta klasė (kaip servisas) NuomaConsoleUI (RentConsoleUI),
    // kuriame turi būti meniu, visi pasirinkimai išbandyti visą programos funkcionalumą.
    // NuomaConsoleUI turi priimti kaip argumentą objektą pagal NuomaService interface.
    internal class RentConsoleUI
    {
        public static IRentService RentService { get; set; }

        //readonly ConsoleKey[] KEYS =
        //    { ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3,
        //    ConsoleKey.NumPad4, ConsoleKey.NumPad5, ConsoleKey.NumPad6, ConsoleKey.NumPad7,
        //    ConsoleKey.NumPad8, ConsoleKey.NumPad9, ConsoleKey.D0, ConsoleKey.D1,
        //    ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6,
        //    ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9 };

        public RentConsoleUI(string connectionString)
        {
            RentService = new RentService(connectionString);
        }

        public void MainMenu()
        {
            ConsoleKeyInfo cki = new();
            ConsoleKey[] validKeys =
                { ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3,
                  ConsoleKey.NumPad4, ConsoleKey.NumPad5, ConsoleKey.NumPad6, ConsoleKey.D0, ConsoleKey.D1,
                  ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6 };

            Console.WriteLine("~~~ CAR RENTAL \"NEW RANDOM\" ~~~" +
                  "\n\n1. Register a random electric vehicle" +
                    "\n2. Register a random fossil fuel vehicle" +
                    "\n3. Register a random client" +
                    "\n4. Display vehicles..." +
                    "\n5. Display clients..." +
                    "\n6. Display rents..." +
                    "\n\n0. Quit.\n");

            do
            {
                while (!Console.KeyAvailable)
                { }
                cki = Console.ReadKey(false);
            }
            while (!validKeys.Contains(cki.Key));

            Console.Clear();
            int selection = (int)cki.Key;

            switch (selection)
            {
                case 48:case 96:
                    Console.WriteLine($"\nGoodbye!");
                    return;
                case 49:case 97: // Create and register random electric vehicle
                    RentService.RegisterVehicle(NewRandomElectricVehicle());
                    break;
                case 50:case 98: // Create and register random fossil fuel vehicle
                    RentService.RegisterVehicle(NewRandomFossilFuelVehicle());
                    break;
                case 51:case 99: // Create and register random client
                    RentService.RegisterClient(NewRandomClient());
                    break;
                case 52:case 100: // Display Vehicles menu
                    VehiclesMenu();
                    break;
                case 53:case 101: // Display all clients (Clients menu)
                    if (RentService.DisplayAllClients() == 0)
                        break;
                    else
                        ClientsOptionsMenu();
                    break;
                case 54:case 102: // Display all rents (Rents menu)
                    if (RentService.DisplayAllRents() == 0)
                        break;
                    else
                        RentsOptionsMenu();
                    break;
                default:
                    Console.WriteLine($"Unexpected error - program is quitting.");
                    return;
            }
            MainMenu();
        }

        public void VehiclesMenu()
        {
            ConsoleKeyInfo cki = new();
            ConsoleKey[] validKeys =
                { ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3,
                  ConsoleKey.D0, ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3 };

            Console.WriteLine("~~~ VEHICLES ~~~" +
                  "\n\n1. Display all vehicles..." +
                    "\n2. Display electric vehicles..." +
                    "\n3. Display fossil fuel vehicles..." +
                    "\n\n0. Go back.\n");

            do
            {
                while (!Console.KeyAvailable) 
                { }
                cki = Console.ReadKey(false);
            }
            while (!validKeys.Contains(cki.Key));

            Console.Clear();
            int selection = (int)cki.Key;

            switch (selection)
            {
                case 48:
                case 96:
                    break;
                case 49:
                case 97: // Display all vehicles and open Vehicles Options Menu
                    if (RentService.DisplayAllVehicles() == 0)
                        break;
                    else
                        VehiclesOptionsMenu();
                    break;
                case 50:
                case 98: // Display electric vehicles and open Electric Vehicles Options Menu
                    if (RentService.DisplayAllElectricVehicles() == 0)
                        break;
                    else
                        VehiclesOptionsMenu();
                    break;
                case 51:
                case 99: // Display fossil fuel vehicles and open Fossil FUel Vehicles Options Menu
                    if (RentService.DisplayAllFossilFuelVehicles() == 0)
                        break;
                    else
                        VehiclesOptionsMenu();
                    break;
                default:
                    Console.WriteLine($"Unexpected error - program is quitting.");
                    return;
            }
        }

        public void VehiclesOptionsMenu()
        {
            ConsoleKeyInfo cki = new();
            ConsoleKey[] validKeys =
                { ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3,
                  ConsoleKey.D0, ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3 };

            Console.WriteLine("~~~ VEHICLES OPTIONS ~~~" +
                  "\n\n1. Rent vehicle" +
                    "\n2. Edit vehicle" +
                    "\n3. Delete vehicle" +
                    "\n\n0. Go back.\n");

            do
            {
                while (!Console.KeyAvailable)
                { }
                cki = Console.ReadKey(false);
            }
            while (!validKeys.Contains(cki.Key));

            Console.Clear();
            int selection = (int)cki.Key;

            switch (selection)
            {
                case 48:case 96: // Go back
                    break;
                case 49:case 97: // Rent vehicle
                    RentService.RegisterRent(NewRent());
                    break;
                case 50:case 98: // Edit vehicle
                    object? vehicle = UpdateVehicle(SelectVehicleID());
                    if (vehicle == null)
                        break;
                    else
                        RentService.UpdateVehicle(vehicle);
                    break;
                case 51:case 99: // Delete vehicle
                    int id2 = SelectVehicleID();
                    if (id2 != -1)
                        RentService.DeleteVehicle(id2);
                    break;
                default:
                    Console.WriteLine($"Unexpected error - program is quitting.");
                    return;
            }
        }

        public void ClientsOptionsMenu()
        {
            ConsoleKeyInfo cki = new();
            ConsoleKey[] validKeys =
                { ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2,
                  ConsoleKey.D0, ConsoleKey.D1, ConsoleKey.D2 };

            Console.WriteLine("~~~ CLIENTS OPTIONS ~~~" +
                  "\n\n1. Edit client" +
                    "\n2. Delete client" +
                    "\n\n0. Go back.\n");

            do
            {
                while (!Console.KeyAvailable)
                { }
                cki = Console.ReadKey(false);
            }
            while (!validKeys.Contains(cki.Key));

            Console.Clear();
            int selection = (int)cki.Key;

            switch (selection)
            {
                case 48:case 96:
                    break;
                case 49:case 97: // Edit client
                    RentService.UpdateClient(NewRandomClient());
                    break;
                case 50:case 98: // Delete client
                    int id = SelectClientID();
                    if (id != -1)
                        RentService.DeleteClient(id);
                    break;
                default:
                    Console.WriteLine($"Unexpected error - program is quitting.");
                    return;
            }
        }

        public void RentsOptionsMenu()
        {
            ConsoleKeyInfo cki = new();
            ConsoleKey[] validKeys =
                { ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2,
                  ConsoleKey.D0, ConsoleKey.D1, ConsoleKey.D2 };

            Console.WriteLine("~~~ RENTS OPTIONS ~~~" +
                  "\n\n1. Edit rent" +
                    "\n2. Delete rent" +
                    "\n\n0. Go back.\n");

            do
            {
                while (!Console.KeyAvailable)
                { }
                cki = Console.ReadKey(false);
            }
            while (!validKeys.Contains(cki.Key));

            Console.Clear();
            int selection = (int)cki.Key;

            switch (selection)
            {
                case 48:case 96:
                    break;
                case 49:case 97: // Edit rent
                    int id = SelectRentID();
                    if (id != -1)
                        RentService.UpdateRent(NewRent());
                    break;
                case 50:case 98: // Delete rent
                    int id2 = SelectRentID();
                    if (id2 != -1)
                        RentService.DeleteRent(id2);
                    break;
                default:
                    Console.WriteLine($"Unexpected error - program is quitting.");
                    return;
            }
        }

        public ElectricVehicle NewRandomElectricVehicle()
        {
            Random random = new();
            // Get make
            string make = Enum.GetName(typeof(Make), random.Next(42));
            // Get model
            string model = Enum.GetName(typeof(Model), random.Next(191));
            // Get productionYear
            int productionYear = random.Next(1985, 2025);
            // Get battery capacity
            double batteryCapacity = Math.Round(40 + random.NextDouble() * 40, 2);

            // Create product and return it
            ElectricVehicle vehicle = new(make, model, productionYear, batteryCapacity);

            return vehicle;
        }

        public FossilFuelVehicle NewRandomFossilFuelVehicle()
        {
            Random random = new();
            // Get make
            string make = Enum.GetName(typeof(Make), random.Next(42));
            // Get model
            string model = Enum.GetName(typeof(Model), random.Next(191));
            // Get productionYear
            int productionYear = random.Next(1985, 2025);
            // Get fuel tank capacity
            double tankCapacity = Math.Round(40 + random.NextDouble() * 40, 2);

            // Create product and return it
            FossilFuelVehicle vehicle = new(make, model, productionYear, tankCapacity);

            return vehicle;
        }

        public Client NewRandomClient()
        {
            Random random = new();
            // Get full name
            string fullName = Enum.GetName(typeof(FirstName), random.Next(20)) + " " + Enum.GetName(typeof(LastName), random.Next(20));

            // Create client and return it
            Client client = new(fullName);

            return client;
        }

        public object? UpdateVehicle(int id)
        {
            if (!RentService.VehiclesIDExists(id, out bool isElectric))
                return null;

            Console.Clear();
            Console.WriteLine("Please provide vehicle make: \n");
            string make = SelectString();
            if (make == "-1")
                return null;

            Console.Clear();
            Console.WriteLine("Please provide vehicle model: \n");
            string model = SelectString();
            if (model == "-1")
                return null;

            Console.Clear();
            Console.WriteLine("Please provide vehicle production year: \n");
            int productionYear = SelectInteger(1900, 2025);
            if (productionYear == -1)
                return null;

            if (isElectric)
            {
                Console.Clear();
                Console.WriteLine("Please provide vehicle battery capacity: \n");
                double batteryCapacity = SelectDouble(1, 3000);
                return new ElectricVehicle(id, make, model, productionYear, batteryCapacity);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Please provide vehicle fuel tank capacity: \n");
                double fuelTankCapacity = SelectDouble(1, 3000);
                return new FossilFuelVehicle(id, make, model, productionYear, fuelTankCapacity);
            }
        }

        public Rent? NewRent()
        {
            int vehicleID = SelectVehicleID();
            if (vehicleID == -1)
                return null;
            int clientID = SelectClientID();
            if (clientID == -1)
                return null;
            DateTime dateFrom = SelectDate();

            Console.WriteLine("\nWould you like to set an end-date to the rent (yes/no)?\n");
            if (YesOrNo())
                return new(vehicleID, clientID, dateFrom, SelectDate());
            else
                return new(vehicleID, clientID, dateFrom);
        }

        public int SelectVehicleID()
        {
            int id;
            bool isInt;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter vehicle ID number... (Cancel: -1)\n");
                isInt = int.TryParse(Console.ReadLine(), out id);
                if (id == -1)
                {
                    Console.WriteLine("\nCancelled\n");
                    return id;
                }
            }
            while (!isInt || !RentService.VehiclesIDExists(id));

            Console.WriteLine();
            return id;
        }

        public int SelectVehicleID(out bool isElectric)
        {
            int id;
            bool isInt;
            isElectric = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter vehicle ID number... (Cancel: -1)\n");
                isInt = int.TryParse(Console.ReadLine(), out id);
                if (id == -1)
                {
                    Console.WriteLine("\nCancelled\n");
                    return id;
                }
            }
            while (!isInt || !RentService.VehiclesIDExists(id, out isElectric));

            Console.WriteLine();
            return id;
        }

        public int SelectRentID()
        {
            int id;
            bool isInt;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter rent ID number... (Cancel: -1)\n");
                isInt = int.TryParse(Console.ReadLine(), out id);
                if (id == -1)
                {
                    Console.WriteLine("\nCancelled\n");
                    return id;
                }
            }
            while (!isInt || !RentService.RentsIDExists(id));

            Console.WriteLine();
            return id;
        }

        public int SelectClientID()
        {
            int id;
            bool isInt;
            do
            {
                Console.WriteLine("Enter client ID number... (Cancel: -1)\n");
                isInt = int.TryParse(Console.ReadLine(), out id);
                if (id == -1)
                {
                    Console.WriteLine("\nCancelled\n");
                    return id;
                }
            }
            while (!isInt || !RentService.ClientsIDExists(id));

            Console.WriteLine();
            return id;
        }

        public DateTime SelectDate()
        {
            DateTime date;
            bool isDateTime;
            do
            {
                Console.Clear();
                Console.WriteLine("Please provide a date: \n");
                isDateTime = DateTime.TryParse(Console.ReadLine(), out date);
            }
            while (!isDateTime || date.Date < DateTime.Now.Date);

            Console.WriteLine();

            return date;
        }

        public string SelectString()
        {
            string newString;
            do
            {
                Console.WriteLine("(Cancel: -1)\n");
                newString = Console.ReadLine();
                if (newString == "-1")
                {
                    Console.WriteLine("\nCancelled\n");
                    return newString;
                }
            }
            while (newString == null || newString.Trim().Length < 1);

            Console.WriteLine();
            return newString;
        }

        public int SelectInteger(int minValueIncl, int maxValueExcl)
        {
            int integer;
            bool isInt;
            do
            {
                Console.WriteLine($"Enter a whole number >={minValueIncl} and <{maxValueExcl}... (Cancel: -1)\n");
                isInt = int.TryParse(Console.ReadLine(), out integer);
                if (integer == -1)
                {
                    Console.WriteLine("\nCancelled\n");
                    return integer;
                }
            }
            while (!isInt || integer < minValueIncl || integer >= maxValueExcl);

            Console.WriteLine();
            return integer;
        }

        public double SelectDouble(double minValueIncl, double maxValueExcl)
        {
            double doubleNumber;
            bool isDouble;
            do
            {
                Console.WriteLine($"Enter a fractional number between {minValueIncl} and {maxValueExcl}... (Cancel: -1)\n");
                isDouble = double.TryParse(Console.ReadLine(), out doubleNumber);
                if (doubleNumber == -1)
                {
                    Console.WriteLine("\nCancelled\n");
                    return doubleNumber;
                }
            }
            while (!isDouble || doubleNumber < minValueIncl || doubleNumber > maxValueExcl);

            Console.WriteLine();
            return doubleNumber;
        }

        public bool YesOrNo()
        {
            string? response = "";
            do
            {
                response = Console.ReadLine();
                if (response != null)
                    response = response.ToLower();
            }
            while (response == null || (response != "yes" && response != "no"));

            if (response == "yes")
                return true;
            else
                return false;
        }

        //public int SelectDaysToRent()
        //{
        //    int days;
        //    bool isInt;
        //    do
        //    {
        //        Console.WriteLine("How many days to rent? (Cancel: -1)\n");
        //        isInt = int.TryParse(Console.ReadLine(), out days);
        //        if (days == -1)
        //        {
        //            Console.WriteLine("\nCancelled\n");
        //            return days;
        //        }
        //    }
        //    while (!isInt || days < 0);

        //    Console.WriteLine();

        //    return days;
        //}
    }
}