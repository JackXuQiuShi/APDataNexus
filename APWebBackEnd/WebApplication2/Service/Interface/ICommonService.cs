using System.Threading.Tasks;
using APWeb.Models;
using APWeb.Service.Models;

namespace APWeb.Service.Interface
{
    public interface ICommonService
    {
        Task<ServiceResult<int>> GetServerNodeIDAsync();
    }
}
