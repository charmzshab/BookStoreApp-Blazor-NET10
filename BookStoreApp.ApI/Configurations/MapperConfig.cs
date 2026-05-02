using AutoMapper;
using BookStoreApp.ApI.Data;
using BookStoreApp.ApI.DTos.Author;

namespace BookStoreApp.ApI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorCreateDto, Author>().ReverseMap();
            CreateMap<AuthorUpdateDto, Author>().ReverseMap();
            CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();

        }
    }
}
