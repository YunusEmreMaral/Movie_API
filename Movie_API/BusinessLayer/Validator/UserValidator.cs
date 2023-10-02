using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserFullName).NotEmpty().WithMessage("Kullanıcı isim ve soyisim boş geçilemez.");
            RuleFor(x => x.UserFullName).MinimumLength(4).WithMessage("Kullanıcı isim ve soyisim en az 4 karakterden oluşmalıdır.");
            RuleFor(x => x.UserFullName).MaximumLength(50).WithMessage("Kullanıcı isim ve soyisim en fazla 50 karakterden oluşmalıdır.");
            RuleFor(x => x.UserFullName).Matches("^[a-zA-Z0-9\\s]+$").WithMessage("Kullanıcı isim ve soyisim sadece harf, rakam ve boşluklardan oluşmalıdır.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş geçilemez.");
            RuleFor(x => x.UserName).MinimumLength(3).WithMessage("Kullanıcı ismi en az 3 karakterden oluşmalıdır.");
            RuleFor(x => x.UserName).MaximumLength(15).WithMessage("Kullanıcı ismi en fazla 15 karakterden oluşmalıdır.");
            RuleFor(x => x.UserPassword).NotEmpty().WithMessage("Şifre  boş geçilemez.");
            RuleFor(x => x.UserPassword).MinimumLength(4).WithMessage("Şifre en az 4 karakterden oluşmalıdır.");

        }
    }
}
