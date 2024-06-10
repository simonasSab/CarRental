using MongoDB.Bson.Serialization.Attributes;

namespace Uzduotis01
{
    // Elektromobilis: papildoma savybė BaterijosTalpa.
    public class ElectricVehicle : Vehicle
    {
        public double BatteryCapacity { get; set; }

        public ElectricVehicle()
        {
        }
        // Full constructor - for reading from DB
        public ElectricVehicle(int id, string make, string model, int productionYear, string vin, double batteryCapacity) : base(id, make, model, productionYear, vin)
        {
            BatteryCapacity = batteryCapacity;
        }
        // Without VIN - for editing in DB
        public ElectricVehicle(int id, string make, string model, int productionYear, double batteryCapacity) : base(id, make, model, productionYear)
        {
            BatteryCapacity = batteryCapacity;
        }
        // Without ID and VIN - for editing or creating new entries in DB
        public ElectricVehicle(string make, string model, int productionYear, double batteryCapacity) : base(make, model, productionYear)
        {
            BatteryCapacity = batteryCapacity;
        }

        public double GetBatteryCapacity()
        {
            return BatteryCapacity;
        }

        public void SetBatteryCapacity(double batteryCapacity)
        {
            BatteryCapacity = batteryCapacity;
        }

        public override string ToString()
        {
            return $"ID {ID:000} {Make} {Model} {ProductionYear}, {VIN}, Battery Capacity: {BatteryCapacity:.0} kWh";
        }
    }
}
