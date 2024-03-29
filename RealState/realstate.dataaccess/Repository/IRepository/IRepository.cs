﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace realstate.dataaccess.Repository.IRepository
{
   public interface IRepository<T> where T:class
    {
        T Get(int id);
        Tuple<List<T>, decimal> GetAllWithPagination(Expression<Func<T, bool>> filter = null ,int page=1,  decimal pageResult=3);
        IEnumerable<T> GetAll(
            Expression<Func<T,bool>> filter=null,
            Func<IQueryable<T>,IOrderedQueryable<T>> orderby=null,
            string includeProperties=null
            );
       T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );
        void Add(T entity);
        public T SetAndGet(T entity);
        void Remove(T entity);
        void Remove(int id);
        void RemoveRange(IEnumerable<T> entities);

    }
}
