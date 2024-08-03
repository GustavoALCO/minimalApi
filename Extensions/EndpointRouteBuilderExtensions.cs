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

        var rangocomIdEndpointLocked = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
                                                           //Teve que criar um novo MapGroup para não haver nenhuma interferencia nos passados
            .AddEndpointFilter(new RangoIsLockedFilter(13))
            .AddEndpointFilter(new RangoIsLockedFilter(6));
        //CHAMANDO UMA CLASSE ONDE NÃO SERA POSSIVEL ALTERAR OU EXCLUIR A PROPRIEDADE COM OS ID 13 E 8 
        //esta criando uma chain of responsibility
        

        rangosComIDEndpoints.MapGet("", RangosHandlers.GetRangosIdAsync);

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosNameAsync).WithName("GetRangos");
        //Adiciona um apelido para ser chamado 
        rangosEndpoints.MapPost("", RangosHandlers.PostRangosAsync)
            .AddEndpointFilter<ValidateAnnotationFilter>();

        rangocomIdEndpointLocked.MapPut("", RangosHandlers.PutRangoAsync);

        

        rangocomIdEndpointLocked.MapDelete("", RangosHandlers.DeleteRangoAsync).AddEndpointFilter<LogNotFoundResponseFilter>();
        
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

