using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Maro.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Repository.Data.Configurations.Orders
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(O => O.Status)
                .HasConversion(
                    // → It saves the enum as its name (a string) in the database, like "Pending", "PaymentReceived", etc.
                    OS => OS.ToString(),
                    // → It reads the string from DB (like "Pending") and converts it back into the enum OrderStatus.Pending.
                    OS => (OrderStatus)Enum.Parse(typeof(OrderStatus), OS) 
                    );

            builder.OwnsOne(O => O.ShippingAddress, SA => SA.WithOwner());

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(O => O.DeliveryMethodId);

        }
    }
}
