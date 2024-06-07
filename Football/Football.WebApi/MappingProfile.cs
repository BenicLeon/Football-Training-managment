using AutoMapper;
using Football.Model;
namespace Football.WebApi
{
    

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<CreatePlayerDto, Player>();
            CreateMap<UpdatePlayerDto, Player>();
            
        }
    }
}
