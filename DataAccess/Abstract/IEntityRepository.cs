using Entities.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    //Generic constraint (generic kısıt)
    // class burada referans tip demektir burada 
    // IEntity demek IEntity olabilir veya IEntity implment eden bir nesne olabilir 
    //New(): newlenebilir olmalı demek yani burada referans tip olarak IEntity 
    //vermemesi için kısıtladık çünkü interfaceler newlenemezler
    public interface IEntityRepository<T> where T : class,IEntity ,new()
    {
        List<T> GetAll(Expression<Func<T,bool>> filter=null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
       
    }
}
