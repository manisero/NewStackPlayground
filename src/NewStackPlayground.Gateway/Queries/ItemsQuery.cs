using System.Collections.Generic;
using Manisero.CqrsGateway.QueriesHandling;
using NewStackPlayground.Application;
using NewStackPlayground.Application.Domain;

namespace NewStackPlayground.Gateway.Queries
{
    public class ItemsQuery : IQuery<ICollection<Item>>
    {
    }

    public class ItemsQueryHandler : IQueryHandler<ItemsQuery, ICollection<Item>>
    {
        private readonly IItemRepository _itemRepository;

        public ItemsQueryHandler(
            IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public QueryOutput<ICollection<Item>> Handle(ItemsQuery query, QueryContext context)
        {
            return new QueryOutput<ICollection<Item>>
            {
                Result = _itemRepository.GetAll(),
                TotalCount = _itemRepository.CountAll()
            };
        }
    }
}
