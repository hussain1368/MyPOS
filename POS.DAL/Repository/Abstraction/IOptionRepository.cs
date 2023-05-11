using POS.DAL.Domain;
using POS.DAL.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IOptionRepository
    {
        Task CreateOption(OptionValueDTO data);
        Task DeleteOptions(int[] ids);
        SettingDTO GetActiveSetting();
        Task<IEnumerable<TreasuryDTO>> GetTreasuriesList();
        Task<IEnumerable<WarehouseDTO>> GetWarehousesList();
        Task<OptionValueDTO> OptionByCode(string code);
        Task<IList<OptionValueDTO>> OptionsAll(bool includeDeleted = false);
        Task<IList<OptionValueDTO>> OptionsByTypeCode(string typeCode);
        Task<IList<OptionValueDTO>> OptionsByTypeId(int typeId);
        Task<IList<OptionTypeDTO>> OptionTypes();
        Task<IList<OptionValueDTO>> SelectOptions(IQueryable<OptionValue> query);
        Task UpdateOption(OptionValueDTO data);
        Task UpdateSetting(SettingDTO data);
    }
}