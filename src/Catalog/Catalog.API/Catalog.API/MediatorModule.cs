namespace Catalog.API;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(typeof(GetEntitiesQueryHandler<Product,AppDbContext>).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        try
        {
            builder.RegisterAssemblyOpenGenericTypes(typeof(GetEntityByIdQueryHandler<Product,AppDbContext>).Assembly)
                .AsImplementedInterfaces();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}