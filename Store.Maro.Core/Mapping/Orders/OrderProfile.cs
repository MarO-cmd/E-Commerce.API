using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Maro.Core.Dtos.Orders;
using Store.Maro.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Mapping.Orders
{
    public class OrderProfile :Profile
    {
        public OrderProfile(IConfiguration configuration)
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(D => D.DeliveryMethod, options => options.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(D => D.DeliveryMethodCost, options => options.MapFrom(S => S.DeliveryMethod.Cost))
                ;
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductName, options => options.MapFrom(S => S.product.ProductName))
                .ForMember(D => D.ProductId, options => options.MapFrom(S => S.product.ProductId))
                .ForMember(D => D.PictureUrl, options => options.MapFrom(S => $"{configuration["BASEURL"]}{S.product.PictureUrl}"))
                ;
        }
    }
}
