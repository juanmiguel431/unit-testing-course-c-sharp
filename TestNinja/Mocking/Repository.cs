using System.Collections.Generic;
using System.Data.Entity;

namespace TestNinja.Mocking
{
    public interface IRepository<TE> where TE : class
    {
        void Remove(TE item);
        void RemoveRange(IEnumerable<TE> items);
        void Add(TE item);
        void AddRange(IEnumerable<TE> items);
        TE Find(int id);
    }

    public class Repository<TC, TE> : IRepository<TE> where TC: DbContext where TE : class
    {
        protected TC Context { get; }
        protected DbSet<TE> DbSet { get; }
        
        public Repository(TC context)
        {
            Context = context;
            DbSet = context.Set<TE>();
        }

        public virtual void Remove(TE item)
        {
            DbSet.Remove(item);
        }
        
        public virtual void RemoveRange(IEnumerable<TE> items)
        {
            DbSet.RemoveRange(items);
        }
        
        public virtual void Add(TE item)
        {
            DbSet.Add(item);
        }
        
        public virtual void AddRange(IEnumerable<TE> items)
        {
            DbSet.AddRange(items);
        }
        
        public virtual TE Find(int id)
        {
            return DbSet.Find(id);
        }
    }
}