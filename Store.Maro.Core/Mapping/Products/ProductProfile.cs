using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Maro.Core.Dtos.Product;
using Store.Maro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Mapping.Products
{
    public class ProductProfile:Profile
    {
        

        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductDto>()
                .ForMember(D => D.BrandName, option => option.MapFrom(S => S.Brand.Name))
                .ForMember(D => D.TypeName, option => option.MapFrom(S => S.Type.Name))
                .ForMember(D=> D.PictureUrl , option => option.MapFrom(s=> $"{configuration["BASEURL"]}{s.PictureUrl}"))
                ;
                

            CreateMap<ProductBrand, TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
