using FluentValidation;

namespace Manisero.CqrsGateway.Validation
{
    public interface IValidatorResolver
    {
        IValidator<TItem> Resolve<TItem>();
    }
}
