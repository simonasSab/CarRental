using MongoDB.Bson.Serialization.Attributes;

namespace Uzduotis01
{
    // NaftosKuroAutomobilis: papildoma savybė BakoTalpa.
    public class FossilFuelVehicle : Vehicle
    {
        public double TankCapacity { get; set; }

        public FossilFuelVehicle()
        { 
        }
        // Full constructor - for reading from DB
        public FossilFuelVehicle(int id, string make, string model, int productionYear, string vin, double tankCapacity) : base(id, make, model, productionYear, vin)
        {
            TankCapacity = tankCapacity;
        }
        // Without VIN - for editing in DB
        public FossilFuelVehicle(int id, string make, string model, int productionYear, double tankCapacity) : base(id, make, model, productionYear)
        {
            TankCapacity = tankCapacity;
        }
        // Without ID and VIN - for creating new entries in DB
        public FossilFuelVehicle(string make, string model, int productionYear, double tankCapacity) : base(make, model, productionYear)
        {
            TankCapacity = tankCapacity;
        }

        public double GetTankCapacity()
        {
            return TankCapacity;
        }

        public void SetTankCapacity(double tankCapacity)
        {
            TankCapacity = tankCapacity;
        }

        public override string ToString()
        {
            return $"ID {ID:000} {Make} {Model} {ProductionYear}, {VIN}, Tank Capacity: {TankCapacity:.0} l";
        }
    }
}
