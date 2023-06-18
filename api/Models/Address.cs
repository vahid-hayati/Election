using System.ComponentModel.DataAnnotations;

namespace api.Models;

public record CompleteAddress(
    [MinLength(2), MaxLength(40)] string Street,
    [MinLength(2), MaxLength(30)] string City,
    [MinLength(2), MaxLength(25)] string State,
    [MinLength(2), MaxLength(30)] string Country
);