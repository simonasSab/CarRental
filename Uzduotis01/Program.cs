using MongoDB.Driver;
using MongoDB.Bson;
using Serilog;

namespace Uzduotis01;

// Sukurkite C# programą, kuri valdytų automobilių nuomos procesą.
// Sistema turėtų leisti registruoti naujus automobilius, atnaujinti jų informaciją,
// peržiūrėti esamų automobilių sąrašą bei valdyti klientų nuomos procesą.
//
// LABAI SVARBU
// Laikini sąrašai, meniu ir kita, neturi būti Main dalyje - Main dalyje galimas tik servisų ir repositories inicializavimas!

    public class Program
    {
        const string connectionUri = "mongodb+srv://simonasSab2:TqrPNQWAo0TRh7NL-@carrental.uoohtxo.mongodb.net/?retryWrites=true&w=majority&appName=CarRental";
        static IDatabaseRepository _databaseRepository { get; set; }
        static IMongoDBRepository _mongoDBRepository { get; set; }
        static IRentService _rentService { get; set; }
        static RentConsoleUI _rentConsoleUI { get; set; }

    public static void Main(string[] args)
    {
        // Create Serilog configuration
        ILogger log = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        Log.Logger = log;

        // Setup MongoDB
        // Create new client and connect to server
        var client = new MongoClient(connectionUri);
        _mongoDBRepository = new MongoDBRepository(client);
        // Ping to confirm successful connection
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Log.Information("Pinged your deployment. You successfully connected to MongoDB!\n");
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

            // Initialize server DB and other services
            _databaseRepository = new DatabaseRepository("Server=DESKTOP-OD4Q280;Database=CarRental;Integrated Security=True;");
            _rentService = new RentService(_databaseRepository, _mongoDBRepository);
            _rentConsoleUI = new RentConsoleUI(_rentService);

        bool isDone = RunAsyncTasks().Result;
    }

    public static async Task<bool> RunAsyncTasks()
    {
        await _rentConsoleUI.MainMenu();
        return true;
    }
}