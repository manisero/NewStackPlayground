using Manisero.CqrsGateway.QueriesHandling;
using NewStackPlayground.Application;
using NewStackPlayground.Application.Domain;

namespace NewStackPlayground.Gateway.Queries
{
    public class ItemQuery : IQuery<Item>
    {
        public int ItemId { get; set; }
    }

    public class ItemQueryHandler : IQueryHandler<ItemQuery, Item>
    {
        private readonly IItemRepository _itemRepository;

        public ItemQueryHandler(
            IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public QueryOutput<Item> Handle(
            ItemQuery query,
            QueryContext context)
        {
            return new QueryOutput<Item>
            {
                Result = _itemRepository.Get(query.ItemId)
            };
        }
    }
}
