
using CursoAsp.Models;
using MiniValidation;

namespace CursoAsp.EndpointFilters
{
    public class ValidateAnnotationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var RangoParaCriacaoDTO = context.GetArgument<RangoParaCriacaoDTO>(2);
            //Pega o RangoparaCriacaoDTO no index do post 

            if (!MiniValidator.TryValidate(RangoParaCriacaoDTO, out var validationErrors)) 
            {//Tenta validar se caso der errado ele manda a mesnsagem para o validationErrors
                return TypedResults.ValidationProblem(validationErrors);
            }

            return await next(context);
        }
    }
}
