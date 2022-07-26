using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SentProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Faktura")]
        public string Invoice { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Firma")]
        public Company Company { get; set; }

        [DisplayName("Suma")]
        public int Sum { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Data")]
        [Required(ErrorMessage = "Uzupełnij to pole")]
        public DateTime Date { get; set; }

        [DisplayName("Tablica rejestracyjna")]
        [Required(ErrorMessage = "Uzupełnij to pole")]
        public string LicensePate { get; set; }

        public int WZ { get; set; }

        [DisplayName("Lista")]
        public IEnumerable<Product> List { get; set; }
    }
}
