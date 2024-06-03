namespace Uzduotis01
{
    internal class Rent
    {
        private int ID { get; }
        private int VehicleID { get; }
        private int ClientID { get; }
        private DateTime DateFrom { get; }
        private DateTime? DateTo { get; set; }

        public Rent()
        {
        }

        public Rent(DateTime? dateTo)
        {
            ID = 0;
            VehicleID = 0;
            ClientID = 0;
            DateFrom = DateTime.Now; ;
            DateTo = dateTo;
        }
        public Rent(int vehicleID, int clientID, DateTime dateFrom)
        {
            ID = 0;
            VehicleID = vehicleID;
            ClientID = clientID;
            DateFrom = dateFrom;
            DateTo = null;
        }
        public Rent(int vehicleID, int clientID, DateTime dateFrom, DateTime? dateTo)
        {
            ID = 0;
            VehicleID = vehicleID;
            ClientID = clientID;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
        public Rent(int id, int vehicleID, int clientID, DateTime dateFrom, DateTime? dateTo)
        {
            ID = id;
            VehicleID = vehicleID;
            ClientID = clientID;
            DateFrom = dateFrom;
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
