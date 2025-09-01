using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
  public  class SlideRepository :RepositoryBase<long,Slide>,ISlideRepository
  {
      private readonly ShopContext _context;
        public SlideRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public EditSlide GetDetails(long id)
        {
            return _context.Slides.Select(X => new EditSlide
            {
                PictureTitle = X.PictureTitle,
                BtnText = X.BtnText,
                Text = X.Text,
                Heading = X.Heading,
                Id = X.Id,
                Picture = X.Picture,
                PictureAlt = X.PictureAlt,
                Title = X.Title
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<SlideViewModel> GetList()
        {
           return _context.Slides.Select(x => new SlideViewModel
            {
                Heading = x.Heading,
                Picture = x.Picture,
                Title = x.Title,
                Id = x.Id,
                IsRemoved = x.IsRemoved,
                CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture)
                
            }).OrderByDescending(x => x.Id).ToList();
        }
    }
}
