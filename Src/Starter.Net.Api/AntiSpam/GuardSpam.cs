using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using reCAPTCHA.AspNetCore;

namespace Starter.Net.Api.AntiSpam
{
    public class GuardSpam : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var recaptchaService = context.HttpContext.RequestServices.GetRequiredService<IRecaptchaService>();
            if ((!context.HttpContext.Request.Headers.TryGetValue("X-Recaptcha-Response", out var recaptchaResponse)) || recaptchaResponse == "null")
            {
                context.Result = new ForbidResult();
                return;
            }

            var response = await recaptchaService.Validate(recaptchaResponse);
            if (!response.success)
            {
                context.Result = new ForbidResult();
                return;
            }
            await next();
        }
    }
}
