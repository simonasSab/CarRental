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
        [Key] [Column("ID")]
        public int ID { get; set; }
        [Column("FullName")]
        public string FullName { get; set; }
        [Column("PersonalID")]
        public decimal PersonalID { get; set; }
        [Column("RegistrationDateTime")]
        public DateTime RegistrationDateTime { get; set; }
        [NotMapped] [BsonId]
        ObjectId mongoID { get; set; }

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
        public Client(int id, string fullName, decimal personalID)
        {
            ID = id;
            FullName = fullName;
            PersonalID = personalID;
            RegistrationDateTime = DateTime.MinValue;
        }
        public Client(int id, string fullName, decimal personalID, DateTime registrationDateTime)
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
        public decimal GetPersonalID()
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
