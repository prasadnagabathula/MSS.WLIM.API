using MSS.WLIM.DataServices.Models;

namespace MSS.WLIM.LostItemRequest.API.Services
{
    public interface ILostItemRequestService
    {
        public Task<IEnumerable<LostItemRequests>> GetAll();
        public Task<LostItemRequests> Get(string id);
        public Task<LostItemRequests> Add(LostItemRequests _object);
        Task<string> UploadPhotoAsync(LostItemRequestPhoto lostItemRequestPhoto);
        public Task<LostItemRequests> Update(LostItemRequests _object);
        public Task<bool> Delete(string id);

        public Task<LostItemRequests> Claim(LostItemRequestsViewModel _object);
        public Task<DashboardData> ClaimCount(string location);
        public Task<IEnumerable<WHLocation>> GetLocations();
        public Task<UserDashboardData> UserCountsData(string user);
        public Task<LostItemRequests> UpdateReceiptStatus(LostItemRequests _object);
    }
}
