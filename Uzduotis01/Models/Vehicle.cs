using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Uzduotis01
{
    // Sukurkite bazinę klasę Automobilis, kuri turės savybes: Id, Marke, Modelis, Metai, RegistracijosNumeris.
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(FossilFuelVehicle), typeof(ElectricVehicle))]
    public class Vehicle
    {
        public int ID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public string VIN { get; set; }
        [BsonId]
        ObjectId mongoID { get; set; }

        public Vehicle()
        {
        }
        public Vehicle(int id, string make, string model, int productionYear, string vin)
        {
            ID = id;
            Make = make;
            Model = model;
            ProductionYear = productionYear;
            VIN = vin;
        }
        public Vehicle(int id, string make, string model, int productionYear)
        {
            ID = id;
            Make = make;
            Model = model;
            ProductionYear = productionYear;
            VIN = "temp";
        }
        public Vehicle(int id)
        {
            ID = id;
            Make = "temp";
            Model = "temp";
            ProductionYear = 1337;
            VIN = "temp";
        }
        public Vehicle(string make, string model, int productionYear)
        {
            ID = 0;
            Make = make;
            Model = model;
            ProductionYear = productionYear;
            VIN = "temp";
        }
        public int GetID()
        {
            return ID;
        }
        public void SetID(int id)
        {
            ID = id;
        }
        public string GetMake()
        {
            return Make;
        }
        public void SetMake(string newValue)
        {
            Make = newValue;
        }
        public string GetModel()
        {
            return Model;
        }
        public void SetModel(string newValue)
        {
            Model = newValue;
        }
        public int GetProductionYear()
        {
            return ProductionYear;
        }
        public void SetProductionYear(int newValue)
        {
            ProductionYear = newValue;
        }
        public string GetVIN()
        {
            return VIN;
        }
        public void SetVIN(string vin)
        {
            VIN = vin;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            Vehicle vehicle = (Vehicle)obj;
            if (vehicle.ID == this.ID)
                return true;

            return false;
        }

        public override string ToString()
        {
            return $"ID {ID:000} {Make} {Model} {ProductionYear}, {VIN}";
        }
    }
}
