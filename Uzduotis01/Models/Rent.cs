using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Uzduotis01;

public class Rent
{
    public int ID { get; set; }
    public int? VehicleID { get; set; } = null;
    public int? BicycleID { get; set; } = null;
    public int ClientID { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    [BsonId] ObjectId mongoID { get; set; }

    // Empty
    public Rent()
    {
    }
    // For writing new vehicle rent (without DateTo)
    public Rent(int vehicleID, int clientID, DateTime dateFrom)
    {
        ID = 0;
        VehicleID = vehicleID;
        ClientID = clientID;
        DateFrom = dateFrom;
        DateTo = null;
    }
    // For writing new vehicle rent
    public Rent(int vehicleID, int clientID, DateTime dateFrom, DateTime? dateTo)
    {
        ID = 0;
        VehicleID = vehicleID;
        ClientID = clientID;
        DateFrom = dateFrom;
        DateTo = dateTo;
    }
    // For updating vehicle rent: from RentConsoleUI to RentService (without DateTo)
    public Rent(int id, int vehicleID, int clientID, DateTime dateFrom)
    {
        ID = id;
        VehicleID = vehicleID;
        ClientID = clientID;
        DateFrom = dateFrom;
        DateTo = null;
    }
    // For updating vehicle rent: from RentConsoleUI to RentService
    public Rent(int id, int vehicleID, int clientID, DateTime dateFrom, DateTime? dateTo)
    {
        ID = id;
        VehicleID = vehicleID;
        ClientID = clientID;
        DateFrom = dateFrom;
        DateTo = dateTo;
    }
    // For writing new bicycle rent (without DateTo)
    public Rent(int clientID, DateTime dateFrom, int bicycleID)
    {
        ID = 0;
        ClientID = clientID;
        DateFrom = dateFrom;
        BicycleID = bicycleID;
        DateTo = null;
    }
    // For writing new bicycle rent
    public Rent(int clientID, DateTime dateFrom, DateTime? dateTo, int bicycleID)
    {
        ID = 0;
        ClientID = clientID;
        DateFrom = dateFrom;
        DateTo = dateTo;
        BicycleID = bicycleID;
    }
    // For updating bicycle rent: from RentConsoleUI to RentService (without DateTo)
    public Rent(int id, int clientID, DateTime dateFrom, int bicycleID)
    {
        ID = id;
        ClientID = clientID;
        DateFrom = dateFrom;
        BicycleID = bicycleID;
        DateTo = null;
    }
    // For updating bicycle rent: from RentConsoleUI to RentService
    public Rent(int id, int clientID, DateTime dateFrom, DateTime? dateTo, int bicycleID)
    {
        ID = id;
        ClientID = clientID;
        DateFrom = dateFrom;
        DateTo = dateTo;
        BicycleID = bicycleID;
    }
    // Temporary for updating only DateTo (to DatabaseRepository)
    public Rent(DateTime? dateTo)
    {
        ID = 0;
        ClientID = 0;
        DateFrom = DateTime.Now;
        DateTo = dateTo;
    }

    public override string ToString()
    {
        string dateTo = "";
        if (DateTo == null)
            dateTo = "Open-ended contract";
        else if (DateTo != null)
            dateTo = ((DateTime)DateTo).ToString(("yyy-MM-dd"));

        string transportID = "";
        if (VehicleID == null)
            transportID = $"Bicycle ID: {BicycleID}";
        else if (BicycleID == null)
            transportID = $"Vehicle ID: {VehicleID}";

        return $"ID {ID:000} {transportID}, Client ID: {ClientID}, Date From: {DateOnly.FromDateTime(DateFrom.Date)}, Date To: {dateTo}";
    }

    public int GetID()
    {
        return ID;
    }
    public int? GetVehicleID()
    {
        return VehicleID;
    }
    public int? GetBicycleID()
    {
        return BicycleID;
    }
    public int GetClientID()
    {
        return ClientID;
    }
    public DateTime GetDateFrom()
    {
        return DateFrom;
    }
    public DateTime? GetDateTo()
    {
        return DateTo;
    }
    public void SetDateTo(DateTime? dateTo)
    {
        DateTo = dateTo;
    }
}
