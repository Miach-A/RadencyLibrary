using AutoMapper;
using RadencyLibrary.CQRS.BookCq.Commands.RateBook;
using RadencyLibrary.CQRS.BookCq.Commands.ReviewBook;
using RadencyLibrary.CQRS.BookCq.Commands.Save;
using RadencyLibrary.CQRS.BookCq.Dto;
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
                .ForMember(x => x.ReviewsNumber,
                    y => y.MapFrom(x => x.Ratings.Count()));

            CreateMap<Book, BookDetailsDto>()
                .ForMember(x => x.Rating,
                    y => y.MapFrom(
                        x => x.Ratings.Count() > 0
                        ? Convert.ToDecimal(x.Ratings.Average(y => y.Score))
                        : 0));

            CreateMap<Review, ReviewDto>();

            CreateMap<SaveBookCommand, Book>()
                .ForMember(x => x.Reviews, y => y.Ignore())
                .ForMember(x => x.Ratings, y => y.Ignore());

            CreateMap<ReviewBookCommand, Review>()
                .ForMember(x => x.Book, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.BookId, y => y.MapFrom(x => x.Id));

            CreateMap<RateBookCommand, Rating>()
                .ForMember(x => x.Book, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.BookId, y => y.MapFrom(x => x.Id));

        }
    }
}
