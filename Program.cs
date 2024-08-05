using CursoAsp.DdContext;
using CursoAsp.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


//DbContext = pasta que gerencia o controle ao bando de dados 

//EndPointFilters = Esta os filtros onde por exemplo no caso de excluir ou alterar algum item do banco de dados ele proteger e n�o deixar 

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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TokenAuthRango",
        //defini��o do nome das configura�oes
        new()
        {
            Name = "Authorization",
            Description = "Token baseado em Autentica��o e Autoriza��o",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = ParameterLocation.Header
        }
        );
    options.AddSecurityRequirement(new()
        {
        //criando uma chave 
            {
            new()
            //adicionando um elemento
            {
                Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "TokenAuthRango"
                }
            },
            new List<string>()
            //adicionando um elemento
            }
        });
});
//configurando o swagger(vem padr�o )
builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbStr"])
    );
//criando uma conex�o para o banco de dados

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                .AddEntityFrameworkStores<RangoDbContext>(); 

builder.Services.AddAuthorizationBuilder()
                .AddPolicy("RequiredAdminFromBrazil", policy =>
                {
                    policy
                          .RequireRole("admin")
                          //ROLE � PARA ATRIBUIR O PAPEL 
                          .RequireClaim("country", "Brazil");
                          //CLAIM SERVE PARA ATRIBUIR A LOCALIZA��O 
                });//adicionando politica 


builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
//adicionando configura�oes de seguran�a dentro da api, o codigo se encontra no appsettings.json

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


app.UseAuthentication();
app.UseAuthorization();
//Faz funcionar os filtros de seguran�a configurado l� em cima
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}//no caso de estar no ambiente de desenvolvimento ira aparecer o swagger

app.UseHttpsRedirection();

app.Run();