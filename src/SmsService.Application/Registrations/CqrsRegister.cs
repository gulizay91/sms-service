using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmsService.Application.Behaviours;

namespace SmsService.Application.Registrations;

public static class CqrsRegister
{
  public static void RegisterMediatr(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
  }
}