using AutoMapper;
using BookStore.Contract.DTOs;
using BookStore.Domain;

namespace BookStore.Contract;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<BookDto, Book>().ReverseMap();
        CreateMap<BookMutationDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}