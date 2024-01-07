using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
       private readonly IProductDal _ProductDal;
        private readonly ICategoryService _categoryService;


        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _ProductDal = productDal;
            _categoryService = categoryService;
        }
        [CacheRemoveAspect("IProductService.Get")]
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [TransactionScopeAspect]
        public IResult Add(Product product)
        {
           IResult result= BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
             CheckProductNameIsSame(product.ProductName), CheckCategoryExceed());
            if (result != null)
            {
                return result;
            } 
            _ProductDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

           
        }

        public IResult Delete(int id)
        {
            throw new NotImplementedException();

        }
        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            
            return new SuccessDataResult<List<Product>>(_ProductDal.GetAll(),Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_ProductDal.GetAll(p=>p.CategoryId == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_ProductDal.Get(p=> p.ProductId== productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>( _ProductDal.GetAll(p=>p.UnitPrice>=min && p.UnitPrice<=max));
        }

        public IDataResult<List<ProductDetailDto>>GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_ProductDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            var result = _ProductDal.Count(p=>p.CategoryId == product.CategoryId);
            if (result >= 10) 
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);

            }
            return new SuccessResult();

        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            int count = _ProductDal.Count(p => p.CategoryId == categoryId);
            if (count >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();

        }
        private IResult CheckProductNameIsSame(string productName)
        {
            if (_ProductDal.Exists(p => p.ProductName == productName))
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }return new SuccessResult();
        }
        private IResult CheckCategoryExceed()
        {
            int categoryCount = _categoryService.Count();
            if(categoryCount >= 15) 
            {
                return new ErrorResult(Messages.CategoryLimitExceted);
            }
            return new SuccessResult();

        }
    }
}
