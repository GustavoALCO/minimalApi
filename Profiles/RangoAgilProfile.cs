using AutoMapper;
using CursoAsp.Entities;
using CursoAsp.Models;

// PROFILES É UMA CLASSE ONDE ELA VAI MAPEAR TODOS OS MODELS
namespace CursoAsp.Profiles;
//É NECESSARIO ADICIONAR O PROFILE PARA CONSEGUIR TRABALHAR COM AUTO MAPPER
public class RangoAgilProfile : Profile
{
    public RangoAgilProfile()
    {
        CreateMap<Rango, RangoDTO>().ReverseMap();//REVERSE SERVE QUE NO CASO DO PROGRAM ESTA TUDO RETORNANDO O RANGO ELE VAI FAZER RETORNAR O RANGODTO
        //CRIANDO UM MAPA DO RANGO PARA O RANGO DTO

        CreateMap<Rango, RangoParaCriacaoDTO>().ReverseMap();

        CreateMap<Rango, RangoParaAtualizacaoDTO>().ReverseMap();
        CreateMap<Ingredientes, IngredientesDTO>()
            .ForMember(d => d.RangoID,
            o => o.MapFrom(s => s.Rangos.First().Id));// Mapeia a partir da propriedade Id do primeiro Rango na coleção Rangos
    }
}

