using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        //Bu sınıfın bu metodu verile validator ve validate edilecek entitinin validate operasyonu sonrasında
        //hata varsa fırlatacğı kısımdır 
        //IValidator benim kodlarını ezeceğim yeri belirttiğim alan örneğin Product(doğrulama kurallarının olduğu class)
        //Entity ise doğrulayacağım varlık verdiğim kısım örneğim Product 
        public static void Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
