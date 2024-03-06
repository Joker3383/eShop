namespace Order.API.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Models.Order>
{
    public void Configure(EntityTypeBuilder<Models.Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.SubId);
        builder.Property(o => o.TotalSum);
        builder.Property(o => o.DateOfOrder)
            .HasColumnType("timestamp with time zone")
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.Property(o => o.BasketId);
    }
}