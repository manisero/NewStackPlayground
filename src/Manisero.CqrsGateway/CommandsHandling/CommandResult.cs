using System;
using System.Collections.Generic;
using Manisero.CqrsGateway.Validation;
using Manisero.Logger;

namespace Manisero.CqrsGateway.CommandsHandling
{
    public class CommandResult<TResult>
    {
        public TResult Result { get; set; }

        public ICollection<ValidationError> ValidationErrors { get; set; }

        public KnownException KnownError { get; set; }

        public Exception UnknownError { get; set; }
    }
}
