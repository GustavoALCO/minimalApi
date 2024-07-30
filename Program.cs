using CursoAsp.DdContext;
using CursoAsp.EndpointHandlers;
using CursoAsp.Extensions;
using Microsoft.EntityFrameworkCore;

//DbContext = pasta que gerencia o controle ao bando de dados 
//EndPointHandlers = É onde esta os metodos de Get,Put,Del e Create
//Entities = Pasta onde esta as Entidades("Propriedades") de cada um dos objetos
//Migrations = Pasta criada pelo EF para a criação de tabelas no sqlite
//Models = é onde esta a parte onde controlamos as estidades sem mostar a pasta raiz
//Profiles = é onde esta o mapeamento do Models
//Extensions = onde fica armazenado onde faz as chamadas dos metodos do EndPointHandlers

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

app.RegisterRangosEndpoints();
app.registerIngredientesEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}//no caso de estar no ambiente de desenvolvimento ira aparecer o swagger

app.UseHttpsRedirection();

app.Run();