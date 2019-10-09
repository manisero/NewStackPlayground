using Manisero.CqrsGateway.CommandsHandling;
using NewStackPlayground.Application;
using NewStackPlayground.Application.Domain;

namespace NewStackPlayground.Gateway.Commands
{
    public class UpdateItemCommand : ICommand<Item>
    {
        public int ItemId { get; set; }

        public string Name { get; set; }
    }

    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, Item>
    {
        private readonly IItemRepository _itemRepository;

        public UpdateItemCommandHandler(
            IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public Item Handle(
            UpdateItemCommand command,
            CommandContext context)
            => _itemRepository.Update(new Item
            {
                ItemId = command.ItemId,
                Name = command.Name
            });
    }
}
