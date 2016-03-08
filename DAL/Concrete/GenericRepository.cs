using DAL.Interface.DTO;
using DAL.Interface.Repository;
using DAL.Mappers;
using ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class GenericRepository<TEntity, TOrm> : IRepository<TEntity>
        where TEntity : class, IDalEntity
        where TOrm : class, IORMEntity
    {
        private readonly DbContext context;

        public GenericRepository(DbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TOrm>().ToList().Select(answ => (TEntity)answ.ToDal());
        }

        public TEntity GetById(int key)
        {
            TOrm entity = context.Set<TOrm>().FirstOrDefault(ent => ent.Id == key);
            return (TEntity)entity.ToDal();
        }

        public TEntity GetByPredicate(System.Linq.Expressions.Expression<Func<TEntity, bool>> f)
        {
            Func<TEntity, bool> func = f.Compile();
            IEnumerable<TEntity> answers = GetAll();
            return answers.FirstOrDefault(answ => func(answ));
        }

        public void Create(TEntity e)
        {
            context.Set<TOrm>().Add((TOrm)e.ToOrm());
        }

        public void Delete(TEntity e)
        {
            context.Set<TOrm>().Remove((TOrm)e.ToOrm());
        }

        public void Update(TEntity e)
        {
            TOrm entity = context.Set<TOrm>().FirstOrDefault(answ => answ.Id == e.Id);
            e.CopyToOrm(entity, context);
            context.Set<TOrm>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
