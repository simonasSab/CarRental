using MongoDB.Driver;
using MongoDB.Bson;

namespace Uzduotis01
{
    // Sukurkite C# programą, kuri valdytų automobilių nuomos procesą.
    // Sistema turėtų leisti registruoti naujus automobilius, atnaujinti jų informaciją,
    // peržiūrėti esamų automobilių sąrašą bei valdyti klientų nuomos procesą.
    //
    // LABAI SVARBU
    // Laikini sąrašai, meniu ir kita, neturi būti Main dalyje - Main dalyje galimas tik servisų ir repositories inicializavimas!

    public class Program
    {
        const string connectionUri = "mongodb+srv://simonasSab2:TqrPNQWAo0TRh7NL-@carrental.uoohtxo.mongodb.net/?retryWrites=true&w=majority&appName=CarRental";
        static IDatabaseRepository databaseRepository { get; set; }
        static IMongoDBRepository mongoDBRepository { get; set; }
        static IRentService rentService { get; set; }
        static RentConsoleUI rentConsoleUI { get; set; }

        public static void Main(string[] args)
        {
            // Create a new client and connect to the server
            var client = new MongoClient(connectionUri);
            mongoDBRepository = new MongoDBRepository(client);
            // Send a ping to confirm a successful connection
            try
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            databaseRepository = new DatabaseRepository("Server=DESKTOP-OD4Q280;Database=CarRental;Integrated Security=True;");
            rentService = new RentService(databaseRepository, mongoDBRepository);
            rentConsoleUI = new RentConsoleUI(rentService);

            bool isDone = RunAsyncTasks().Result;
        }

        public static async Task<bool> RunAsyncTasks()
        {
            await rentConsoleUI.MainMenu();
            return true;
        }
    }
}
