using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validator
{
    public class MovieValidator : AbstractValidator<Movie>
    {
        public MovieValidator()
        {
            
            
            RuleFor(x=> x.MovieGenreID).NotEmpty().WithMessage("Film türü boş geçilemez.");
            RuleFor(x => x.MovieName).NotEmpty().WithMessage("Film ismi boş geçilemez.");
            RuleFor(x => x.MovieName).MinimumLength(3).WithMessage("Film ismi en az 3 karakterden oluşmalıdır.");
            RuleFor(x => x.MovieName).MaximumLength(50).WithMessage("Film ismi en fazla 50 karakterden oluşmalıdır.");
            RuleFor(x => x.MovieName).Matches("^[a-zA-Z0-9\\s]+$").WithMessage("Film ismi  sadece harf, rakam ve boşluklardan oluşmalıdır.");
            RuleFor(x => x.MovieDirector).NotEmpty().WithMessage("Film yönetmeni boş geçilemez.");
            RuleFor(x => x.MovieDirector).MinimumLength(5).WithMessage("Film yönetmeni en az 5 harften oluşmalıdır.");
            RuleFor(x => x.MovieDirector).MaximumLength(50).WithMessage("Film yönetmeni en fazla 50 harften oluşmalıdır.");
            RuleFor(x => x.MovieRelaseYear).NotEmpty().WithMessage("Filmin çıkış yılı  boş geçilemez.");
            RuleFor(x => x.MovieRelaseYear).InclusiveBetween(1895, 2024).WithMessage("Geçerli bir film yılı giriniz .(1895-2024)");
            RuleFor(x => x.MovieDuration).NotEmpty().WithMessage("Film süresi  boş geçilemez.");
            RuleFor(x => x.MovieDuration).InclusiveBetween(0,750).WithMessage("Geçerli bir film süresi  giriniz .(0-750)");
            RuleFor(x => x.MovieGenreID)
                .InclusiveBetween(1, 17)
                .WithMessage("Film türü: 1-Aksiyon 2-Belgesel, 3-Bilim Kurgu, 4-Dram, 5-Fantastik, 6-Gerilim, 7-Komedi, 8-Korku, 9-Macera, 10-Müzikal, 11-Romantik, 12-Savaş, 13-Spor, 14-Suç, 15-Tarih, 16-Western, 17-Gizem");


        }
    }
}
