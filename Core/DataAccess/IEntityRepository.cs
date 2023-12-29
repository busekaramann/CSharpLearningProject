
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess
{
    //Generic constraint (generic kısıt)
    // class burada referans tip demektir burada 
    // IEntity demek IEntity olabilir veya IEntity implment eden bir nesne olabilir 
    //New(): newlenebilir olmalı demek yani burada referans tip olarak IEntity 
    //vermemesi için kısıtladık çünkü interfaceler newlenemezler
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T,bool>> filter=null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        int Count(Expression<Func<T, bool>> filter = null);

        bool Exists(Expression<Func<T, bool>> filter = null);
       
    }
}
