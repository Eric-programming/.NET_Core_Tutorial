using API_Advanced.Models;
using API_Advanced.Models.DTO;
using AutoMapper;


namespace API_Advanced.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, DisplayUserDto>();

        }
    }
}