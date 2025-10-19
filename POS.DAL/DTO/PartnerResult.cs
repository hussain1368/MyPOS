using System.Collections.Generic;

namespace POS.DAL.DTO
{
    public class PartnerResult
    {
        public IEnumerable<PartnerDTO> Partners { get; set; }
        public int PageCount { get; set; }
    }
}
