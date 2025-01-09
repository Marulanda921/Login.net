using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiLoginFull.Model
{
    public class UserCredentials
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("email")]
        [Required(ErrorMessage = "El correo electronico es Obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electronico no es valido.")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("currentToken")]
        [JsonIgnore]
        public string? CurrentToken { get; set; }
    }
}
