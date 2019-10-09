using System;
using System.Transactions;
using Manisero.CqrsGateway.CommandsHandling;
using Manisero.CqrsGateway.LogsAndErrors;
using Manisero.CqrsGateway.Validation;
using Manisero.Logger;

namespace Manisero.CqrsGateway
{
    public interface ICommandsGateway
    {
        CommandResult<TResult> Handle<TCommand, TResult>(
            TCommand command,
            CommandContext context)
            where TCommand : ICommand<TResult>;
    }

    public class CommandsGateway : ICommandsGateway
    {
        private static readonly ILogger Logger = LoggerFacade.GetLogger();

        private readonly ICommandHandlerResolver _commandHandlerResolver;
        private readonly Func<IDisposable> _scopeFactory;
        private readonly IValidationFacade _validationFacade;

        public CommandsGateway(
            ICommandHandlerResolver commandHandlerResolver,
            Func<IDisposable> scopeFactory,
            IValidatorResolver validatorResolver)
        {
            _commandHandlerResolver = commandHandlerResolver;
            _scopeFactory = scopeFactory;
            _validationFacade = new ValidationFacade(validatorResolver);
        }

        public CommandResult<TResult> Handle<TCommand, TResult>(
            TCommand command,
            CommandContext context)
            where TCommand : ICommand<TResult>
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Logger.Log(new CommandReceivedLog(command, context));

            using (_scopeFactory())
            {
                try
                {
                    using (var transaction = new TransactionScope())
                    {
                        var validationErrors = _validationFacade.Validate(command);

                        if (validationErrors != null)
                        {
                            return new CommandResult<TResult> { ValidationErrors = validationErrors };
                        }

                        var commandHandler = _commandHandlerResolver.Resolve<TCommand, TResult>();
                        var result = commandHandler.Handle(command, context);
                        
                        transaction.Complete();

                        return new CommandResult<TResult> { Result = result };
                    }
                }
                catch (KnownException e)
                {
                    return new CommandResult<TResult> { KnownError = e };
                }
                catch (Exception e)
                {
                    return new CommandResult<TResult> { UnknownError = e };
                }
            }
        }
    }
}
