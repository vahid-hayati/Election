using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models;

public record President(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? Id,
    [MinLength(3), MaxLength(20)] string Name,
    [MinLength(3), MaxLength(30)] string LastName,
    [MinLength(10), MaxLength(10)] string NationalCode,
    [MinLength(5), MaxLength(20)] string Education,
    [MinLength(5), MaxLength(50)] string DegreeOfEEducation,
    [Range(30, 80)]int Age,
    CompleteAddress CompleteAddress
);