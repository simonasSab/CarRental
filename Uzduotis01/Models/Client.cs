using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Uzduotis01
{
    public class Client
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public long PersonalID { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        [BsonId] ObjectId mongoID { get; set; }

        public Client()
        {
        }

        public Client(string fullName)
        {
            ID = 0;
            FullName = fullName;
            PersonalID = 0;
            RegistrationDateTime = DateTime.MinValue;
        }
        public Client(int id, string fullName, long personalID)
        {
            ID = id;
            FullName = fullName;
            PersonalID = personalID;
            RegistrationDateTime = DateTime.MinValue;
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
