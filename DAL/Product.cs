using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Typ")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [Range(100, 1500, ErrorMessage = "Podaj wartość pomiędzy 100 a 1500")]
        [DisplayName("Waga")]
        public int Weight { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Kod kreskowy")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Data")]
        [BsonDateTimeOptions(DateOnly = true)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
