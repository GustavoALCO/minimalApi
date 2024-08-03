
using System.Net;

namespace CursoAsp.EndpointFilters;

public class LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger) : IEndpointFilter
{                                           //Uma classe que é obrigatoria a passar um parametro
    public readonly ILogger<LogNotFoundResponseFilter> _logger = logger;
                                                       //passando um parametro para a variavel _logger com o valor de logger

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var results = await next(context);

        // Verifica se 'results' é do tipo 'INestedHttpResult'. Se for, 
        // armazena o valor de 'Result' em 'actualResults'; caso contrário, 
        // converte 'results' para 'IResult' e armazena em 'actualResults'.
        var actualResults = (results is INestedHttpResult result1) ? result1.Result : (IResult)results;

        // Verifica se 'actualResults' é do tipo 'IStatusCodeHttpResult' e se 
        // a propriedade 'StatusCode' é igual ao código de status HTTP 404 (NotFound).
        if (actualResults is IStatusCodeHttpResult { StatusCode: (int)HttpStatusCode.NotFound })
        {
            // Loga uma mensagem informativa indicando que o recurso solicitado não foi encontrado,
            // incluindo o caminho da requisição HTTP.
            _logger.LogInformation($"Resource {context.HttpContext.Request.Path} was not found.");
        }


        return results;
    }
}

