using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Validations
{
    public interface IThirdPartyService<T>
    {
        bool HasValidatedProfile(ApiUser user);
        bool IsTheUser(ApiUser user, T id);
        Task StartProcess(T id);
        Task<IProfileItem> GetProfileItem(T id);
        Task<(IProfileItem Profile, bool Success)> SetProfileItem(ApiUser user, T id, object encapsulatedData = null);
        Task<(IProfileItem Profile, bool Success)> Cancel(ApiUser user);
        Task<IProfileItem> Update(ApiUser user, T id);
    }
}
