using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StepLang.Libraries.API.Filters;

public class NullResultToNotFoundFilter : IAlwaysRunResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult { Value: null })
        {
            context.Result = new NotFoundResult();
        }
    }
}
