using AutoMapper;
using ChessApp.API.Resources;
using ChessApp.Domain.Models;

namespace ChessApp.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameResource>().ReverseMap();


            CreateMap<Player, PlayerResource>().ReverseMap();

            CreateMap<SaveGameResource, Game>();



            CreateMap<SavePlayerResource, Player>();
        }
    }
}