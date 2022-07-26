using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Characteristic
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Typ")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Nazwa")]
        public string Name { get; set; }
    }
}
