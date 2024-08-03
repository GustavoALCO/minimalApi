using CursoAsp.EndpointFilters;
using CursoAsp.EndpointHandlers;

namespace CursoAsp.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder) 
    {
        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos");
        //Mapeando todos que possuem /rangos
        var rangosComIDEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}");

        rangosComIDEndpoints.MapGet("", RangosHandlers.GetRangosIdAsync);

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosNameAsync).WithName("GetRangos");
        //Adiciona um apelido para ser chamado 
        rangosEndpoints.MapPost("", RangosHandlers.PostRangosAsync);

        rangosComIDEndpoints.MapPut("", RangosHandlers.PutRangoAsync)
            .AddEndpointFilter<TorresmoRangoIsLockedFilter>();
        //CHAMANDO UMA CLASSE ONDE NÃO SERA POSSIVEL ALTERAR O TORRESMO COM O ID 13
            
        rangosComIDEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
        .AddEndpointFilter<TorresmoRangoIsLockedFilter>();
    }

    public static void registerIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndPoint = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes");

        ingredientesEndPoint.MapGet("", IngredientesHandles.GetIngredientesAsync);
        ingredientesEndPoint.MapPost("", () => 
        {
            throw new NotImplementedException();
        });
    }
}

