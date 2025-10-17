using System.Collections.Generic;

namespace POS.DAL.DTO
{
    public class ProductResult
    {
        public IEnumerable<ProductDTO> Products { get; set; }
        public int PageCount { get; set; }
    }
}
