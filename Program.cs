using CursoAsp.DdContext;
using CursoAsp.EndpointHandlers;
using CursoAsp.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net;

//DbContext = pasta que gerencia o controle ao bando de dados 

//EndPointHandlers = � onde esta os metodos de Get,Put,Del e Create

//Entities = Cont�m as classes de entidade que representam as tabelas do banco de dados

//Migrations = Pasta criada pelo EF para a cria��o de tabelas no sqlite

// Models = Cont�m as classes de DTOs (Data Transfer Objects) que s�o usados para definir como os dados s�o expostos ou recebidos pela API, sem expor
// diretamente as entidades do banco de dados.

//Profiles = Cont�m as configura��es de mapeamento do AutoMapper

//Extensions = onde fica armazenado onde faz as chamadas dos metodos do EndPointHandlers

var builder = WebApplication.CreateBuilder(args);
//cria��o de uma variavel para armazenar as configura�oes da webAplication

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//configurando o swagger(vem padr�o )
builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbStr"])
    );
//criando uma conex�o para o banco de dados

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//PEGA OS ASSEMBLY DO DOMINIO ATUAL, PESQUISA QUEM HERDA DE PROFILES E ATRIBUI PARA O PROGRAM 

builder.Services.AddProblemDetails();
// configura para poder usar o UseExceptionHandler
var app = builder.Build();

app.RegisterRangosEndpoints();
app.registerIngredientesEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    // novo modo de mostrar um erro sem aparecer as tecnologias que esta sendo usada 

    //MODO ALTERNATIVO
    /*
    app.UseExceptionHandler(configureApplicationBuilder =>
    {
        configureApplicationBuilder.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("N�o foi possivel achar");
            });//essa fun��o vai pegar o status code, 2� vai retornar um text/html para o header, 3� vai enviar uma mensagem no body 
    });*/
    //criar uma mensagem personalizada para o ambiente de produ��o quando houver um erro
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}//no caso de estar no ambiente de desenvolvimento ira aparecer o swagger

app.UseHttpsRedirection();

app.Run();