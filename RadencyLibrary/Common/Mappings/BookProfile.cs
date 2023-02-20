using AutoMapper;
using RadencyLibrary.CQRS.Book.Dto;
using RadencyLibraryDomain.Entities;

namespace RadencyLibrary.Common.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(x => x.Rating,
                    y => y.MapFrom(
                        x => x.Ratings.Count() > 0
                        ? Convert.ToDecimal(x.Ratings.Average(y => y.Score))
                        : 0))
                .ForMember(x => x.ReviwsNumber,
                    y => y.MapFrom(x => x.Ratings.Count()));

            CreateMap<Book, BookDetailsDto>()

        }
    }
}
