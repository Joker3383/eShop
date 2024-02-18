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