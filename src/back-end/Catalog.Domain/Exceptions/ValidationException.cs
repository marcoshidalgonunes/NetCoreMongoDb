using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace Catalog.Domain.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public List<string> ValdationErrors { get; set; }

        public ValidationException(ValidationResult validationResult)
            : base()
        {
            ValdationErrors = new List<string>();

            foreach (var validationError in validationResult.Errors)
            {
                ValdationErrors.Add(validationError.ErrorMessage);
            }
        }
    }
}
