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