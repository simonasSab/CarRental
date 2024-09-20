using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Uzduotis01;

// Sukurti lentelę ir objektą Dviratis kuris turi Id, Modeli bei pagaminimo metus.
// Sukurti funkcionalumą pridėti dviratį, pašalinti dviratį.
// Sukurti lentelę dviračių nuoma, pagal identišką struktūrą, kaip tai yra realizuota su automobiliais.
// Implementuoti nuomos registraciją Dviračiams.Funkcijas realizuoti naudojant Entity Framework.
// Sukurti tokį patį cache'avimą kaip ir su Automobiliais naudojant MongoDb

[Table("Bicycles")]
public class Bicycle
{
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public int ProductionYear { get; set; }
    [NotMapped] [BsonId]
    ObjectId mongoID { get; set; }

    public Bicycle()
    {
    }
    // For getting from DB and displaying
    public Bicycle(int id, string name, int productionYear)
    {
        ID = id;
        Name = name;
        ProductionYear = productionYear;
    }
    // For searching in DB
    public Bicycle(int id)
    {
        ID = id;
        Name = "temp";
        ProductionYear = 1337;
    }
    // For creating new and storing into DB
    public Bicycle(string name, int productionYear)
    {
        ID = 0;
        Name = name;
        ProductionYear = productionYear;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        Bicycle bicycle = (Bicycle)obj;
        if (bicycle.ID != this.ID)
            return false;
        if (bicycle.Name != this.Name)
            return false;
        if (bicycle.ProductionYear != this.ProductionYear)
            return false;

        return true;
    }

    public override string ToString()
    {
        return $"ID {ID:000} {Name} {ProductionYear}";
    }
}
