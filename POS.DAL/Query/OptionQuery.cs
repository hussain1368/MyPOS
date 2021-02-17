using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace POS.DAL.Query
{
    public class OptionQuery : DbQuery
    {
        public OptionQuery(POSContext dbContext): base(dbContext) { }

        public async Task<IList<OptionValueDTM>> OptionsByTypeId(int typeId)
        {
            var query = dbContext.OptionValues.Where(x => x.IsDeleted == false).Where(x => x.TypeId == typeId);
            return await SelectOptions(query);
        }

        public async Task<IList<OptionValueDTM>> OptionsByTypeCode(string typeCode)
        {
            var query = dbContext.OptionValues.Where(x => x.IsDeleted == false).Where(x => x.Type.Code == typeCode);
            return await SelectOptions(query);
        }

        public async Task<IList<OptionValueDTM>> OptionsAll()
        {
            var query = dbContext.OptionValues.Where(x => x.IsDeleted == false);
            return await SelectOptions(query);
        }

        private async Task<IList<OptionValueDTM>> SelectOptions(IQueryable<OptionValue> query)
        {
            return await query.Select(x => new OptionValueDTM
            {
                Id = x.Id,
                TypeId = x.TypeId,
                TypeCode = x.Type.Code,
                Name = x.Name,
                Code = x.Code
            })
            .ToListAsync();
        }

        public async Task<OptionValueDTM> OptionByCode(string code)
        {
            return await dbContext.OptionValues.Where(x => x.Code == code).Select(x => new OptionValueDTM
            {
                Id = x.Id,
                TypeId = x.TypeId,
                TypeCode = x.Type.Code,
                Name = x.Name,
                Code = x.Code
            })
            .SingleOrDefaultAsync();
        }
    }
}
