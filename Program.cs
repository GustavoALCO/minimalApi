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


app.MapGet("/rango/{id:int}", async (RangoDbContext rangoDbContext,IMapper mapper , int id) =>
{       //pode ser passado dois caminhos de url, apenas deve se declarar quando uma for int                

    return mapper.Map<RangoDTO> (await rangoDbContext.Rangos.FirstOrDefaultAsync( Rangos => Rangos.Id == id));
                          //esta verificando se dentro do Rango existe um com Id semelhante ao que foi passado 
});


app.MapGet("/rangos/{rangoId}/ingredientes", async (
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



app.MapGet("/rangos", async Task<Results<NoContent, 
                                        Ok<IEnumerable<RangoDTO>>
                                        >>
                                        (RangoDbContext rangoDbContext,
                                        IMapper mapper,
                                        [FromQuery(Name = "name")] 
                                        string? rangonome) =>
{                    //FromQuery serve para que não seja nessesario declarar manualmente a variavel na url, o caminho agora será rango?variavel=rango.nome
    
    
        var rangos = await rangoDbContext.Rangos
                                         .Where(Rangos => rangonome == null || Rangos.Nome.ToUpper().Contains(rangonome.ToUpper()))
                                         .ToListAsync();
            //Contains verefica se existe algum dado com algo proximo ao escrito
            
        if(rangos.Count <= 0 || rangos == null)
        {
            return TypedResults.NoContent();
        }   //retorna um 204 de notContent
        else
        {
            return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangos));
        }                       
        //fazendo uma verificação
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}//no caso de estar no ambiente de desenvolvimento ira aparecer o swagger

app.UseHttpsRedirection();

app.Run();

