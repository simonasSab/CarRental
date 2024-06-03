namespace Uzduotis01
{
    internal class Client
    {
        private int ID { get; set; }
        private string FullName { get; set; }
        private long PersonalID { get; set; }
        private DateTime RegistrationDateTime { get; set;  }

        public Client()
        {
        }

        public Client(string fullName)
        {
            FullName = fullName;
        }
        public Client(int id, string fullName, long personalID, DateTime registrationDateTime)
        {
            ID = id;
            FullName = fullName;
            PersonalID = personalID;
            RegistrationDateTime = registrationDateTime;
        }

        public int GetID()
        {
            return ID;
        }
        public string GetFullName()
        {
            return FullName;
        }
        public void SetFullName(string fullName)
        {
            FullName = fullName;
        }
        public long GetPersonalID()
        {
            return PersonalID;
        }
        public DateTime GetRegistrationDateTime()
        {
            return RegistrationDateTime;
        }

        public override string ToString()
        {
            return $"ID {ID:000} {FullName}, P/N {PersonalID}, Joined {RegistrationDateTime}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            Client client = (Client)obj;
            if (client.ID == this.ID)
                return true;

            return false;
        }
    }
}
