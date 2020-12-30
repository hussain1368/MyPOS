using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.DAL.Query
{
    public class OptionQuery : DbQuery
    {
        public OptionQuery(POSContext dbContext): base(dbContext)
        {

        }

        public async Task<IList<OptionValueDT>> OptionsByTypeId(int typeId)
        {
            return await dbContext.OptionValues
                .Where(x => x.IsDeleted == false)
                .Where(x => x.TypeId == typeId)
                .Select(x => new OptionValueDT
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToListAsync();
        }
    }
}
