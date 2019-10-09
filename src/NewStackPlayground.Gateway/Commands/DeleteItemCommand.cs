using Manisero.CqrsGateway.CommandsHandling;
using NewStackPlayground.Application;

namespace NewStackPlayground.Gateway.Commands
{
    public class DeleteItemCommand : ICommand<bool>
    {
        public int ItemId { get; set; }
    }

    public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, bool>
    {
        private readonly IItemRepository _itemRepository;

        public DeleteItemCommandHandler(
            IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public bool Handle(
            DeleteItemCommand command,
            CommandContext context)
            => _itemRepository.Delete(command.ItemId);
    }
}
