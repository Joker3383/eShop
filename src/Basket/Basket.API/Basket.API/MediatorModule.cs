using Basket.API.Data;
using Basket.API.Models;

namespace Basket.API;

using System.Reflection;
using Autofac;
using Basket.API;
using MediatR;
using Shared.CrudOperations;


public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(typeof(GetEntitiesQueryHandler<Models.Basket,AppDbContext>).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        // підключення неймспейса кастомних запитів
        // builder.RegisterAssemblyTypes(typeof(GetButtonByIdQueryHandler).GetTypeInfo().Assembly)
        //     .AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        //HACK: Ругается на регистрацию генерик типов при старте приложения
        try
        {
            builder.RegisterAssemblyOpenGenericTypes(typeof(GetEntityByIdQueryHandler<Models.Basket,AppDbContext>).Assembly)
                .AsImplementedInterfaces();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}