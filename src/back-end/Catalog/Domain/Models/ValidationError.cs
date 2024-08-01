namespace Catalog.Domain.Models;

public class ValidationError
{
    /// <summary>
    /// The name of the property.
    /// </summary>
    public required string Field { get; set; }

    /// <summary>
    /// The error message
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// The property value that caused the failure.
    /// </summary>
    public object? AttemptedValue { get; set; }
}
