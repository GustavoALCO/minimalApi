using CursoAsp.EndpointFilters;
using CursoAsp.EndpointHandlers;
using Microsoft.AspNetCore.Identity;

namespace CursoAsp.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder) 
    {
        endpointRouteBuilder.MapGroup("/Identity/").MapIdentityApi<IdentityUser>();
                                            //trazendo o grupo de IdentityUser
        endpointRouteBuilder.MapGet("pratos/{pratoId:int}", (int pratoId) => $"O prato {pratoId} é muito bom") //Para deixar uma rota como predicate é necessario baixar o nuget Microsoft.aspnetcore.openapi
            .WithOpenApi(operation =>
            {
                operation.Deprecated = true;
                return operation;
            })
            .WithSummary("Este Endpoint esta deprecaded e será excluido futuralmente")
        //deixa uma mensagem no swagger no cabeçalho 
            .WithDescription("Use a rota ");
        //deixa uma mensagem no swagger como uma descrição dentro da chamada  
        //QUANDO VOCÊ FAZ ISSO A ROTA NO SWAGGER FICA UM POUCO TRANSPARENTE MAS FUNCIONA NORMALMENTE, APENAS ALERTANDO QUE FUTURALMENTE VAI SER EXCLUIDA

        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos").RequireAuthorization();
        //Mapeando todos que possuem /rangos                            
        var rangosComIDEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}").WithSummary("Esta rota retornara todos as comidas"); ;

        var rangocomIdEndpointLocked = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
                                                           //Teve que criar um novo MapGroup para não haver nenhuma interferencia nos passados
            .AddEndpointFilter(new RangoIsLockedFilter(13))
            .AddEndpointFilter(new RangoIsLockedFilter(6)).RequireAuthorization().RequireAuthorization("RequiredAdminFromBrazil");
                                                                                                        //esse REQUIREDADMINFROMBRAZIL VEM DO PROGRAM onte é o nome da politica
        //CHAMANDO UMA CLASSE ONDE NÃO SERA POSSIVEL ALTERAR OU EXCLUIR A PROPRIEDADE COM OS ID 13 E 8 
        //esta criando uma chain of responsibility
        

        rangosComIDEndpoints.MapGet("", RangosHandlers.GetRangosIdAsync).AllowAnonymous();
                                                                        //ALLOWANOUNYMOUS FAZ COM QUE NÃO SEJA NECESSARIO FAZER ATENTICAÇÃO
        rangosEndpoints.MapGet("", RangosHandlers.GetRangosNameAsync).WithName("GetRangos");
                                            //Adiciona um apelido para ser chamado 

        rangosEndpoints.MapPost("", RangosHandlers.PostRangosAsync)
            .AddEndpointFilter<ValidateAnnotationFilter>();

        rangocomIdEndpointLocked.MapPut("", RangosHandlers.PutRangoAsync);

        

        rangocomIdEndpointLocked.MapDelete("", RangosHandlers.DeleteRangoAsync).AddEndpointFilter<LogNotFoundResponseFilter>();
        
    }

    public static void registerIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndPoint = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes").RequireAuthorization();

        ingredientesEndPoint.MapGet("", IngredientesHandles.GetIngredientesAsync);
        ingredientesEndPoint.MapPost("", () => 
        {
            throw new NotImplementedException();
        });
    }
}

