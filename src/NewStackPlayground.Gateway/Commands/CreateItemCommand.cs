using Manisero.CqrsGateway.CommandsHandling;
using FluentValidation;
using NewStackPlayground.Application;
using NewStackPlayground.Application.Domain;

namespace NewStackPlayground.Gateway.Commands
{
    public class CreateItemCommand : ICommand<Item>
    {
        public string Name { get; set; }
    }

    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, Item>
    {
        private readonly IItemRepository _itemRepository;

        public CreateItemCommandHandler(
            IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public Item Handle(
            CreateItemCommand command,
            CommandContext context)
            => _itemRepository.Create(new Item
            {
                Name = command.Name
            });
    }
}
