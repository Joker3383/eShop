using System.Reflection;
using Autofac;
using MediatR;
using Order.API.Data;
using Shared.CrudOperations;

namespace Order.API;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(typeof(GetEntitiesQueryHandler<Models.Order,AppDbContext>).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        // підключення неймспейса кастомних запитів
        // builder.RegisterAssemblyTypes(typeof(GetButtonByIdQueryHandler).GetTypeInfo().Assembly)
        //     .AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        //HACK: Ругается на регистрацию генерик типов при старте приложения
        try
        {
            builder.RegisterAssemblyOpenGenericTypes(typeof(GetEntityByIdQueryHandler<Models.Order,AppDbContext>).Assembly)
                .AsImplementedInterfaces();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}