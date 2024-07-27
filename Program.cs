using AutoMapper;
using CursoAsp.DdContext;
using CursoAsp.Entities;
using CursoAsp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//criação de uma variavel para armazenar as configuraçoes da webAplication

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//configurando o swagger(vem padrão )
builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbStr"])
    );
//criando uma conexão para o banco de dados

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                              //PEGA OS ASSEMBLY DO DOMINIO ATUAL, PESQUISA QUEM HERDA DE PROFILES E ATRIBUI PARA O PROGRAM 
var app = builder.Build();

var rangosEndpoints = app.MapGroup("/rangos");
//Mapeando todos que possuem /rangos
var rangosComIDEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}");
var rangosComIngredientes = rangosComIDEndpoints.MapGroup("/ingredientes");
rangosComIDEndpoints.MapGet("", async Task<Results<NoContent, Ok<RangoDTO>>> (RangoDbContext rangoDbContext, IMapper mapper, int rangoId) =>
{       //pode ser passado dois caminhos de url, apenas deve se declarar quando uma for int                

   var rango = await rangoDbContext.Rangos.FirstOrDefaultAsync( Rangos => Rangos.Id == rangoId);
                          //esta verificando se dentro do Rango existe um com Id semelhante ao que foi passado
    if( rango == null )
        return TypedResults.NoContent();

    var rangoDto = mapper.Map<RangoDTO>(rango);
    return TypedResults.Ok(rangoDto);
});


rangosComIngredientes.MapGet("", async (
    RangoDbContext rangoDbContext,
    IMapper mapper,
    //ADICIONA O MAPPER NA CLASSE
    int rangoId) =>
{                    //IENUMERABLE SERVE PARA QUANDO VOCÊ PRECISA RETORNAR UMA LISTA 
    return mapper.Map<IEnumerable<IngredientesDTO>> 
    //ADICIONANDO UM CONTEXTO PARA O MAPPER FUNCIONAR
    ((await rangoDbContext.Rangos.Include(rango => rango.Ingredientes)
//VAI RETORNAR OS INGREDIENTES DO RANGO  //METODO INCLUDE PARA CARREGAR A TABELA INGREDIENTES 
                                      .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredientes);
});                                    //BUSCANDO O RANGO DESEJADO PELO ID              //SEGUNDO MAP, SE CASO NÃO FOR NULO RETORNE OS INGREDIENTES
//vai retornar todos os ingredientes



rangosEndpoints.MapGet("", async Task<Results<NoContent, 
                                        Ok<IEnumerable<RangoDTO>>
                                        >>
                                        (RangoDbContext rangoDbContext,
                                        IMapper mapper,
                                        [FromQuery(Name = "name")] 
                                        string? rangonome) =>
{                    //FromQuery serve para que não seja nessesario declarar manualmente a variavel na url, o caminho agora será rango?variavel=rango.nome
    
    
        var rangos = mapper.Map<IEnumerable<RangoDTO>> (await rangoDbContext.Rangos
                                         .Where(Rangos => rangonome == null || Rangos.Nome.ToUpper().Contains(rangonome.ToUpper()))
                                         .ToListAsync());
            //Contains verefica se existe algum dado com algo proximo ao escrito
            
        if(rangos == null || rangos.Count() <= 0)
        {
            return TypedResults.NoContent();
        }   //retorna um 204 de notContent
        else
        {
            return TypedResults.Ok(rangos);
        }                       
        //fazendo uma verificação
}).WithName("GetRangos");
//Adiciona um apelido para ser chamado 

rangosEndpoints.MapPost("", async Task<CreatedAtRoute<RangoDTO>> 
                             //Essa task informa que essa classe vai retornar um RangoDTO
                             (RangoDbContext rangoDbContext,
                             IMapper mapper,
                             LinkGenerator linkGenerator,
                             HttpContext httpContext,
                             [FromBody] RangoParaCriacaoDTO rangoParaCriacaoDto) => 
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
                                       new { rangoId = rangoToReturn.Id});
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
});

rangosComIDEndpoints.MapPut("", async Task<Results<NotFound, Ok>>(
                                                    //declarando que só existe, duas exeçoes no código. O NotFound e o Ok
    RangoDbContext rangoDbContext,
    IMapper mapper,
    //ADICIONA O MAPPER NA CLASSE
    RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO,
    int rangoId) =>
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
});

rangosComIDEndpoints.MapDelete("", async Task<Results<NotFound, NoContent>> (
    RangoDbContext rangodbContext,
    int rangoId
    ) =>
{
    var rangos = await rangodbContext.Rangos.FirstOrDefaultAsync(rangos => rangos.Id == rangoId);

    if (rangos == null) {
        return TypedResults.NotFound(); 
    }
    
    rangodbContext.Rangos.Remove(rangos);
    rangodbContext.SaveChanges();

    return TypedResults.NoContent();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}//no caso de estar no ambiente de desenvolvimento ira aparecer o swagger

app.UseHttpsRedirection();

app.Run();

