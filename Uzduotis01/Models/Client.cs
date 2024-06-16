using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uzduotis01
{
    [Table("Clients")]
    public class Client
    {
        [Key]
        public int ID { get; set; }
        public string FullName { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long PersonalID { get; set; } = default;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        [NotMapped] [BsonId]
        ObjectId mongoID { get; set; }

        public Client()
        {
        }

        public Client(string fullName)
        {
            ID = 0;
            FullName = fullName;
        }
        public Client(int id, string fullName, long personalID)
        {
            ID = id;
            FullName = fullName;
            PersonalID = personalID;
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
            if (client.ID != this.ID)
                return false;
            if (client.FullName != this.FullName)
                return false;
            if (client.PersonalID != this.PersonalID)
                return false;

            return true;
        }
    }
}
