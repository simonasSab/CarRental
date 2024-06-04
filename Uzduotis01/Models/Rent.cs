namespace Uzduotis01
{
    public class Rent
    {
        public int ID { get; set; }
        public int VehicleID { get; set; }
        public int ClientID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        // Empty
        public Rent()
        {
        }
        // For writing new (without DateTo)
        public Rent(int vehicleID, int clientID, DateTime dateFrom)
        {
            ID = 0;
            VehicleID = vehicleID;
            ClientID = clientID;
            DateFrom = dateFrom;
            DateTo = null;
        }
        // For writing new
        public Rent(int vehicleID, int clientID, DateTime dateFrom, DateTime? dateTo)
        {
            ID = 0;
            VehicleID = vehicleID;
            ClientID = clientID;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
        // For updating: from RentConsoleUI to RentService (without DateTo)
        public Rent(int id, int vehicleID, int clientID, DateTime dateFrom)
        {
            ID = id;
            VehicleID = vehicleID;
            ClientID = clientID;
            DateFrom = dateFrom;
            DateTo = null;
        }
        // For updating: from RentConsoleUI to RentService
        public Rent(int id, int vehicleID, int clientID, DateTime dateFrom, DateTime? dateTo)
        {
            ID = id;
            VehicleID = vehicleID;
            ClientID = clientID;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
        // Temporary for updating only DateTo (to DatabaseRepository)
        public Rent(DateTime? dateTo)
        {
            ID = 0;
            VehicleID = 0;
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

            return $"ID {ID:000} Vehicle ID: {VehicleID}, Client ID: {ClientID}, Date From: {DateOnly.FromDateTime(DateFrom.Date)}, Date To: {dateTo}";
        }

        public int GetID()
        {
            return ID;
        }
        public int GetVehicleID()
        {
            return VehicleID;
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
}
