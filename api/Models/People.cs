using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models;

public record PresidentialElectionForm(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? Id,
    [MinLength(3), MaxLength(20)]string Name,
    [MinLength(3), MaxLength(30)]string LastName,
    [MinLength(10), MaxLength(10)]string NationalCode,
    [MinLength(24), MaxLength(24)]string PresidentId,
    [MinLength(10), MaxLength(10)]string NationalCodePresident,
    CompleteAddress CompleteAddress
);