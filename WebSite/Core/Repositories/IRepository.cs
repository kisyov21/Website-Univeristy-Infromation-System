using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebSite.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //Group for finding objects
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        //Group for add objects
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities); //add list of objects 

        //Group for remove objects
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities); //remove list of objects
    }
}