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
        public static void Main(string[] args)
        {
            DatabaseRepository databaseRepository = new("Server=DESKTOP-OD4Q280;Database=CarRental;Integrated Security=True;");
            RentService rentService = new(databaseRepository);
            RentConsoleUI rentConsoleUI = new(rentService);

            rentConsoleUI.MainMenu();
        }
    }
}
