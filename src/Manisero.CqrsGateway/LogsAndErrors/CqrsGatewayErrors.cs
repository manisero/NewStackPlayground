using System.Collections.Generic;
using Manisero.CqrsGateway.Validation;
using Manisero.Logger;

namespace Manisero.CqrsGateway.LogsAndErrors
{
    [KnownException("09942676-F423-4830-B302-7A50C29A5E16")]
    public class ValidationErrorException : KnownException
    {
        public object InvalidItem { get; }
        public ICollection<ValidationError> Errors { get; }

        public ValidationErrorException(
            object invalidItem, 
            ICollection<ValidationError> errors)
        {
            InvalidItem = invalidItem;
            Errors = errors;
        }

        public override object GetData() => new { ItemType = InvalidItem.GetType(), Item = InvalidItem, Errors };
    }
}
