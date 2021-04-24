﻿using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace POS.DAL.Query
{
    public class OptionQuery : DbQuery
    {
        public OptionQuery(POSContext dbContext): base(dbContext) { }

        public async Task<IList<OptionValueDTO>> OptionsByTypeId(int typeId)
        {
            var query = dbContext.OptionValues.Where(x => x.IsDeleted == false).Where(x => x.TypeId == typeId);
            return await SelectOptions(query);
        }

        public async Task<IList<OptionValueDTO>> OptionsByTypeCode(string typeCode)
        {
            var query = dbContext.OptionValues.Where(x => x.IsDeleted == false).Where(x => x.Type.Code == typeCode);
            return await SelectOptions(query);
        }

        public async Task<IList<OptionValueDTO>> OptionsAll()
        {
            var query = dbContext.OptionValues.Where(x => x.IsDeleted == false);
            return await SelectOptions(query);
        }

        private async Task<IList<OptionValueDTO>> SelectOptions(IQueryable<OptionValue> query)
        {
            return await query.Select(x => new OptionValueDTO
            {
                Id = x.Id,
                TypeId = x.TypeId,
                TypeCode = x.Type.Code,
                Name = x.Name,
                Code = x.Code
            })
            .ToListAsync();
        }

        public async Task<OptionValueDTO> OptionByCode(string code)
        {
            return await dbContext.OptionValues.Where(x => x.Code == code).Select(x => new OptionValueDTO
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
