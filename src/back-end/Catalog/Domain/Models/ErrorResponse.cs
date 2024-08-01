using System.Text.Json;

namespace Catalog.Domain.Models;

public class ErrorResponse
{
    public required string Message { get; set; }

    public List<ValidationError>? Details { get; set; }
}
