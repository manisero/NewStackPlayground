using System.Collections.Generic;
using System.Linq;
using Manisero.CqrsGateway.LogsAndErrors;
using Manisero.Utils;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace Manisero.CqrsGateway.Validation
{
    public interface IValidationFacade
    {
        ICollection<ValidationError> Validate<TItem>(
            TItem item);

        void ValidateAndThrow<TItem>(
            TItem item);
    }

    internal class ValidationFacade : IValidationFacade
    {
        private static readonly HashSet<string> DataKeysToExclude =
            new HashSet<string>(new[] { MessageFormatter.PropertyName, MessageFormatter.PropertyValue });

        private readonly IValidatorResolver _validatorResolver;

        public ValidationFacade(
            IValidatorResolver validatorResolver)
        {
            _validatorResolver = validatorResolver;
        }

        public ICollection<ValidationError> Validate<TItem>(
            TItem item)
        {
            var validator = _validatorResolver.Resolve<TItem>();

            if (validator == null)
            {
                return null;
            }

            var validationResult = validator.Validate(item);

            if (validationResult.IsValid)
            {
                return null;
            }

            return MapValidationResult(validationResult);
        }

        public void ValidateAndThrow<TItem>(
            TItem item)
        {
            var validationErrors = Validate(item);

            if (validationErrors != null)
            {
                throw new ValidationErrorException(item, validationErrors);
            }
        }

        private ICollection<ValidationError> MapValidationResult(
            ValidationResult validationResult)
        {
            return validationResult
                   .Errors
                   .Select(x => new ValidationError
                   {
                       ErrorCode = x.ErrorCode,
                       Property = x.PropertyName.NullIfEmpty(),
                       Data = x.FormattedMessagePlaceholderValues
                               .Where(dataItem => !DataKeysToExclude.Contains(dataItem.Key))
                               .ToDictionary(dataItem => dataItem.Key, dataItem => dataItem.Value)
                   })
                   .ToArray();
        }
    }
}
