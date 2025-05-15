using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Specifications.Products
{
    public class ProductSpecParames
    {
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }   
        }

        public string? Sort { get; set; }
        public int? BrandId{ get; set; }
        public int? TypeId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;

    }
}
