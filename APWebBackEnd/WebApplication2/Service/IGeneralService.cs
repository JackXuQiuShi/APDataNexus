using System.Threading.Tasks;

namespace APWeb.Service
{
    public interface IGeneralService
    {
        Task<string> GetNormalizedID(string ProductID);
    }


}
