using System.Collections.Generic;
using System.Linq;
using Dapper;
using Manisero.DatabaseAccess;
using NewStackPlayground.Application.DataAccess;
using NewStackPlayground.Application.Domain;

namespace NewStackPlayground.Application
{
    public interface IItemRepository
    {
        ICollection<Item> GetAll();
        int CountAll();
        Item Get(int id);
        Item Create(Item item);
        Item Update(Item item);
        bool Delete(int id);
    }

    public class ItemRepository : IItemRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public ItemRepository(
            IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public ICollection<Item> GetAll()
        {
            return _databaseAccessor.Access(conn =>
            {
                using (var context = new EfContext(conn))
                {
                    return context.GetSet<Item>().ToArray();
                }
            });
        }

        public int CountAll()
        {
            return _databaseAccessor.Access(conn =>
            {
                using (var context = new EfContext(conn))
                {
                    return context.GetSet<Item>().Count();
                }
            });
        }

        public Item Get(int id)
        {
            return _databaseAccessor.Access(conn =>
            {
                using (var context = new EfContext(conn))
                {
                    return context.GetSet<Item>().Find(id);
                }
            });
        }

        public Item Create(Item item)
        {
            return _databaseAccessor.Access(conn =>
            {
                using (var context = new EfContext(conn))
                {
                    context.GetSet<Item>().Add(item);
                    context.SaveChanges();

                    return item;
                }
            });
        }

        public Item Update(Item item)
        {
            return _databaseAccessor.Access(conn =>
            {
                using (var context = new EfContext(conn))
                {
                    context.GetSet<Item>().Update(item);
                    context.SaveChanges();

                    return item;
                }
            });
        }

        public bool Delete(int id)
        {
            return _databaseAccessor.Access(conn =>
            {
                var rowsAffected = conn.Execute(
                    @"DELETE FROM ""Item"" WHERE ""ItemId"" = @id",
                    new { id });

                return rowsAffected != 0;
            });
        }
    }
}
