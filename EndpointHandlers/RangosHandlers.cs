using AutoMapper;
using CursoAsp.DdContext;
using CursoAsp.Entities;
using CursoAsp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CursoAsp.EndpointHandlers;

public static class RangosHandlers
{
    public static async Task<Results<NoContent,
                                       Ok<IEnumerable<RangoDTO>>
                                       >> GetRangosNameAsync
                                        (RangoDbContext rangoDbContext,
                                        IMapper mapper,
                                        [FromQuery(Name = "name")]
                                        string? rangonome)
    {
        //FromQuery serve para que não seja nessesario declarar manualmente a variavel na url, o caminho agora será rango?variavel=rango.nome



        var rangos = mapper.Map<IEnumerable<RangoDTO>>(await rangoDbContext.Rangos
                                         .Where(Rangos => rangonome == null || Rangos.Nome.ToUpper().Contains(rangonome.ToUpper()))
                                         .ToListAsync());
        //Contains verefica se existe algum dado com algo proximo ao escrito

        if (rangos == null || rangos.Count() <= 0)
        {
            return TypedResults.NoContent();
        }   //retorna um 204 de notContent
        else
        {
            return TypedResults.Ok(rangos);
        }
        //fazendo uma verificação
    }
    //ENDPOINT DO METODO GETRANGOSASYNC


    public static async Task<Results<NoContent, Ok<RangoDTO>>> GetRangosIdAsync
        (RangoDbContext rangoDbContext, IMapper mapper, int? rangoId)
    {       //pode ser passado dois caminhos de url, apenas deve se declarar quando uma for int                

        var rango = mapper.Map<RangoDTO>(await rangoDbContext.Rangos
                                             .FirstOrDefaultAsync(Rangos => Rangos.Id == rangoId));
        //esta verificando se dentro do Rango existe um com Id semelhante ao que foi passado
        if (rango == null)
            return TypedResults.NoContent();


        return TypedResults.Ok(rango);
    }
    //ENDPOINT DO METODO GETRANGOIDASYNC


    public static async Task<CreatedAtRoute<RangoDTO>> PostRangosAsync
                             //Essa task informa que essa classe vai retornar um RangoDTO
                             (RangoDbContext rangoDbContext,
                             IMapper mapper,
                             LinkGenerator linkGenerator,
                             HttpContext httpContext,
                             [FromBody] RangoParaCriacaoDTO rangoParaCriacaoDto)
    {
        var rangoEntity = mapper.Map<Rango>(rangoParaCriacaoDto);
        //coleta tudo que foi passado no body
        rangoDbContext.Add(rangoEntity);
        //adiciona ao banco de dados
        await rangoDbContext.SaveChangesAsync();
        //salva alteraçoes

        var rangoToReturn = mapper.Map<RangoDTO>(rangoEntity);

        return TypedResults.CreatedAtRoute(rangoToReturn,
                                           // retorna o objeto no corpo da resposta
                                           "GetRangos",
                                           //Chama a função de get
                                           new
                                           {
                                               rangoId = rangoToReturn.Id
                                           });
        //passa o parametro de Id do rango que acabou de ser criado 
        //MODO ALTERNATIVO
        // var linkToReturn = linkGenerator.GetUriByName(
        //    httpContext,
        //Pega o contexto que sua api esta rodando
        //   "GetRango",
        //chama a função de Get
        //  new {id = rangoToReturn.Id});
        //passa o parametro de Id do rango que acabou de ser criado 

        //  return TypedResults.Created(linkToReturn , rangoToReturn);
        //retorna o objeto criado 
    }
    //ENPOINT DO POSTRANGOSASYNC


    public static async Task<Results<NotFound, Ok>> PutRangoAsync(
    //declarando que só existe, duas exeçoes no código. O NotFound e o Ok
    RangoDbContext rangoDbContext,
    IMapper mapper,
    //ADICIONA O MAPPER NA CLASSE
    RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO,
    int rangoId)
    {
        var rangos = await rangoDbContext.Rangos
                                             .FirstOrDefaultAsync(Rangos => Rangos.Id == rangoId);
        //Contains verefica se existe algum dado com algo proximo ao escrito

        if (rangos == null)
        {
            return TypedResults.NotFound();
        }   //retorna um NotFound
            //fazendo uma verificação

        mapper.Map(rangoParaAtualizacaoDTO, rangos);

        await rangoDbContext.SaveChangesAsync();

        return TypedResults.Ok();
    }
    //ENPOINT DO PUTRANGOSASYNC


    public static async Task<Results<NotFound, NoContent>> DeleteRangoAsync(
                                    RangoDbContext rangodbContext,
                                    int rangoId
                                    )
    {
        var rangos = await rangodbContext.Rangos.FirstOrDefaultAsync(rangos => rangos.Id == rangoId);

        if (rangos == null)
        {
            return TypedResults.NotFound();
        }

        rangodbContext.Rangos.Remove(rangos);
        rangodbContext.SaveChanges();

        return TypedResults.NoContent();
    }
    //ENDPOINT DO DELETERANGOASYNC
}
