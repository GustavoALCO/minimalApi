using static System.Net.Mime.MediaTypeNames;

namespace CursoAsp.EndpointFilters;

public class RangoIsLockedFilter : IEndpointFilter
{
    public readonly int _lockedRangoId;

    public RangoIsLockedFilter(int _LockedRangoId)
    {
        _lockedRangoId = _LockedRangoId;
        //Criando um construtor para que quando a classe ser chamada ser obrigatoriamente ser passado um ID 
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        int rangoId;

        if (context.HttpContext.Request.Method == "PUT")
        {
            rangoId = context.GetArgument<int>(3);
            //Para pegar o Id dentro do array de metodos do PUTRANGOSASYNC, se deve colocar o GetArgument colocar o tipo da variavel e a posição.
            //no caso ela é a 4 variavel declarado então o valor vai ser 3
        }
        else if (context.HttpContext.Request.Method == "DELETE")
        {
            rangoId = context.GetArgument<int>(1);
        }
        else
        {
            throw new NotSupportedException("Este filtro não suporta este cenario.");
        }


        if (rangoId == _lockedRangoId)
        {
            return TypedResults.Problem(new()
            {
                Status = 400,
                Title = "Não é Possivel alterar/deletar o torresmo ",
                Detail = "você não pode modificar/deletar essa receita"
            });
        }

        var result = await next.Invoke(context);

        return result;

        throw new NotImplementedException();
    }
    //ADICIONANDO UM FILTRO ONDE NÃO SERA POSSIVEL ALTERAR O TORRESMO COM O ID 13
}
