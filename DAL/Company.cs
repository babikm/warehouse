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
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        public string Nip { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Kod pocztowy")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Lokalizacja")]
        public string Locality { get; set; }

        [Required(ErrorMessage = "Uzupełnij to pole")]
        [DisplayName("Ulica")]
        public string Street { get; set; }
    }
}
