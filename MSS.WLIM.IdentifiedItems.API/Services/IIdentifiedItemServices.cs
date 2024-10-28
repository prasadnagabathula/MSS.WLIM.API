using MSS.WLIM.DataServices.Models;

namespace MSS.WLIM.IdentifiedItem.API.Services
{
    public interface IIdentifiedItemServices
    {
        public  Task<IEnumerable<IdentifiedItems>> GetAll();
       public Task<IdentifiedItems> Get(string id);
       public Task<IdentifiedItems> Add(IdentifiedItems item);
       public Task<IdentifiedItems> Update(IdentifiedItems item);
       public Task<bool> Delete(string id);
    }
}
