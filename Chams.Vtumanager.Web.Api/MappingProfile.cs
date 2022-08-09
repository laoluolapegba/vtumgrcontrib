using AutoMapper;
using Chams.Vtumanager.Provisioning.Entities.Epurse;
using Chams.Vtumanager.Web.Api.ViewModels;


namespace Chams.Vtumanager.Web.Api
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EpurseAccountModel, EpurseAccountMaster>()
                .ReverseMap();
        }
    }
}
