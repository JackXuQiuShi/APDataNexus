using System.Threading.Tasks;
using APWeb.Dtos;
using APWeb.Service.Models;


namespace APWeb.Service.Interface
{
    public interface IProductMovementService
    {
        Task<ServiceResult<int>> DraftProductMovementAsync(string orderID);
        Task<ServiceResult<bool>> UpdateProductMovementItemAsync(ProductMovementItemRequest pmiRequest);
        Task<ServiceResult<bool>> SubmitProductMovementAsync(ProductMovementRequest pmRequest);
    }

}
