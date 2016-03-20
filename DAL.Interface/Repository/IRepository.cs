using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using System.Linq.Expressions;

namespace DAL.Interface.Repository
{
    public interface IRepository<TEntity> where TEntity : IDalEntity
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetBetween(int start, int count);
        TEntity GetById(int key);
        IEnumerable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> f);
        void Create(TEntity e);
        void Delete(TEntity e);
        void Update(TEntity e);
    }
}
