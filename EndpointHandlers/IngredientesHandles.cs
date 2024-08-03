using AutoMapper;
using CursoAsp.DdContext;

using CursoAsp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CursoAsp.EndpointHandlers;

public static class IngredientesHandles
{
    public static async Task<Results<NotFound, Ok<IEnumerable<IngredientesDTO>>>> GetIngredientesAsync(
    RangoDbContext rangoDbContext,
    IMapper mapper,
    //ADICIONA O MAPPER NA CLASSE
    int rangoId)
    {
        var rangos = mapper.Map<RangoDTO>(await rangoDbContext.Rangos
                                             .FirstOrDefaultAsync(Rangos => Rangos.Id == rangoId));

        if (rangos == null)
            return TypedResults.NotFound();
        //IENUMERABLE SERVE PARA QUANDO VOCÊ PRECISA RETORNAR UMA LISTA 
        return TypedResults.Ok(mapper.Map<IEnumerable<IngredientesDTO>>((await rangoDbContext.Rangos.Include(rango => rango.Ingredientes)
        //VAI RETORNAR OS INGREDIENTES DO RANGO  //METODO INCLUDE PARA CARREGAR A TABELA INGREDIENTES 
                                      .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredientes));
    }
}

