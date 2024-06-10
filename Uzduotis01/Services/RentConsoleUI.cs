namespace Uzduotis01
{
    // Programos valdymui su konsole turi būti sukurta klasė (kaip servisas) NuomaConsoleUI (RentConsoleUI),
    // kuriame turi būti meniu, visi pasirinkimai išbandyti visą programos funkcionalumą.
    // NuomaConsoleUI turi priimti kaip argumentą objektą pagal NuomaService interface.
    public class RentConsoleUI
    {
        public static IRentService RentService { get; set; }

        //readonly ConsoleKey[] KEYS =
        //    { ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3,
        //    ConsoleKey.NumPad4, ConsoleKey.NumPad5, ConsoleKey.NumPad6, ConsoleKey.NumPad7,
        //    ConsoleKey.NumPad8, ConsoleKey.NumPad9, ConsoleKey.D0, ConsoleKey.D1,
        //    ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6,
        //    ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9 };

        public RentConsoleUI(IRentService rentService)
        {
            RentService = rentService;
        }

        public async Task MainMenu()
        {
            ConsoleKeyInfo cki = new();
            ConsoleKey[] validKeys =
                { ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3, ConsoleKey.NumPad4,
                ConsoleKey.NumPad5, ConsoleKey.NumPad6, ConsoleKey.NumPad7, ConsoleKey.D0, ConsoleKey.D1, ConsoleKey.D2,
                ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7 };

            Console.WriteLine("~~~ CAR RENTAL \"NEW RANDOM\" ~~~" +
                  "\n\n1. Register a random electric vehicle" +
                    "\n2. Register a random fossil fuel vehicle" +
                    "\n3. Register a random client" +
                    "\n4. Display vehicles..." +
                    "\n5. Display clients..." +
                    "\n6. Display rents..." +
                    "\n7. Toggle Cache Cleaning on/off" +
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
                    await RentService.RegisterVehicle(NewRandomElectricVehicle());
                    break;
                case 50:case 98: // Create and register random fossil fuel vehicle
                    await RentService.RegisterVehicle(NewRandomFossilFuelVehicle());
                    break;
                case 51:case 99: // Create and register random client
                    await RentService.RegisterClient(NewRandomClient());
                    break;
                case 52:case 100: // Display Vehicles menu
                    await VehiclesMenu();
                    break;
                case 53:case 101: // Display all clients (Clients menu)
                    if ((await RentService.DisplayAllClientsAsync()) == 0)
                        break;
                    else
                        ClientsOptionsMenu();
                    break;
                case 54:case 102: // Display all rents (Rents menu)
                    if ((await RentService.DisplayAllRentsAsync()) == 0)
                        break;
                    else
                        await RentsOptionsMenu();
                    break;
                case 55:case 103: // Toggle Cache Cleaning on (with specific time period in ms) / off
                    if (RentService.GetCacheCleaningON())
                        RentService.ToggleCacheCleaning(0);
                    else
                        RentService.ToggleCacheCleaning(SelectInteger(1000, 3600001));
                    break;
                default:
                    Console.WriteLine($"Unexpected error - program is quitting.");
                    return;
            }
            await MainMenu();
        }

        public async Task VehiclesMenu()
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
                        await VehiclesOptionsMenu();
                    break;
                case 50:
                case 98: // Display electric vehicles and open Electric Vehicles Options Menu
                    if ((await RentService.DisplayAllElectricVehiclesAsync()) == 0)
                        break;
                    else
                        await VehiclesOptionsMenu();
                    break;
                case 51:
                case 99: // Display fossil fuel vehicles and open Fossil FUel Vehicles Options Menu
                    if ((await RentService.DisplayAllFossilFuelVehiclesAsync()) == 0)
                        break;
                    else
                        await VehiclesOptionsMenu();
                    break;
                default:
                    Console.WriteLine($"Unexpected error - program is quitting.");
                    return;
            }
        }

        public async Task VehiclesOptionsMenu()
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

            int selection = (int)cki.Key;

            switch (selection)
            {
                case 48:case 96: // Go back
                    Console.Clear();
                    break;
                case 49:case 97: // Rent vehicle
                    await RentService.RegisterRent(NewRent());
                    Console.Clear();
                    break;
                case 50:case 98: // Edit vehicle
                    object? vehicle = UpdateVehicle(SelectVehicleID());
                    Console.Clear();
                    if (vehicle == null)
                        break;
                    else
                        RentService.UpdateVehicle(vehicle);
                    break;
                case 51:case 99: // Delete vehicle
                    int id2 = SelectVehicleID();
                    Console.Clear();
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

            int selection = (int)cki.Key;

            switch (selection)
            {
                case 48:case 96:
                    Console.Clear();
                    break;
                case 49:case 97: // Edit client
                    int id = SelectClientID();
                    Console.Clear();
                    if (id != -1)
                        RentService.UpdateClient(UpdateClient(id));
                    Console.Clear();
                    break;
                case 50:case 98: // Delete client
                    int id2 = SelectClientID();
                    Console.Clear();
                    if (id2 != -1)
                        RentService.DeleteClient(id2);
                    break;
                default:
                    Console.WriteLine($"Unexpected error - program is quitting.");
                    return;
            }
        }

        public async Task RentsOptionsMenu()
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

            int selection = (int)cki.Key;

            switch (selection)
            {
                case 48:case 96:
                    Console.Clear();
                    break;
                case 49:case 97: // Edit rent
                    int id = SelectRentID();
                    Console.Clear();
                    if (id != -1)
                        RentService.UpdateRent(NewRent(id));
                    break;
                case 50:case 98: // Delete rent
                    int id2 = SelectRentID();
                    Console.Clear();
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

        public Client? UpdateClient(int id)
        {
            if (!RentService.ClientsIDExists(id))
                return null;

            Console.Clear();
            Console.WriteLine("Please provide full name: \n");
            string fullName = SelectString();
            if (fullName == "-1")
                return null;

            Console.Clear();
            Console.WriteLine($"Enter an 11-digit personal ID number (Cancel: -1)\n");
            long personalID = SelectPersonalID();
            if (personalID == -1)
                return null;

            return new Client(id, fullName, personalID);
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

        public Rent? NewRent(int id)
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
                return new(id, vehicleID, clientID, dateFrom, SelectDate());
            else
                return new(id, vehicleID, clientID, dateFrom);
        }

        public int SelectVehicleID()
        {
            int id;
            bool isInt;
            do
            {
                Console.WriteLine("\nEnter vehicle ID number... (Cancel: -1)\n");
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
                Console.WriteLine("\nEnter vehicle ID number... (Cancel: -1)\n");
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
                Console.WriteLine("\nEnter rent ID number... (Cancel: -1)\n");
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
                Console.WriteLine("\nEnter client ID number... (Cancel: -1)\n");
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
                Console.WriteLine("\nPlease provide a date: \n");
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

        public long SelectPersonalID()
        {
            long minValueIncl = 10000000000;
            long maxValueExcl = 99999999999;
            long number;
            bool isLong;
            do
            {
                Console.WriteLine("(Cancel: -1)\n");
                isLong = long.TryParse(Console.ReadLine(), out number);
                if (number == -1)
                {
                    Console.WriteLine("\nCancelled\n");
                    return number;
                }
            }
            while (!isLong || number < minValueIncl || number >= maxValueExcl);

            Console.WriteLine();
            return number;
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