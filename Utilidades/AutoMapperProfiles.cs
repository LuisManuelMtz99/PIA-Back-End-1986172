using AutoMapper;
using PIA___Back___End___1986172.DTOs;
using PIA___Back___End___1986172.Entidades;

namespace PIA___Back___End___1986172.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RifaCreacionDTO, Rifa>();
            CreateMap<ClienteCreacionDTO, Cliente>();
            CreateMap<BoletoCreacionDTO, Boleto>();
            CreateMap<PremioCreacionDTO, Premio>();

            CreateMap<Cliente, GetClienteDTO>();
            CreateMap<Premio, GetPremioDTO>();
            CreateMap<Rifa, GetRifaDTO>();
            CreateMap<Boleto, GetBoletoDTO>();

            CreateMap<ClientePatchDTO, Cliente>().ReverseMap();
            CreateMap<BoletoPatchDTO, Boleto>().ReverseMap();
            CreateMap<RifaPatchDTO, Rifa>().ReverseMap();
            CreateMap<PremioPatchDTO, Premio>().ReverseMap();
        }
    }
}

