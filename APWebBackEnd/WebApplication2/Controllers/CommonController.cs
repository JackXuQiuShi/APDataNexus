using Microsoft.AspNetCore.Mvc;
using APWeb.Service.Interface;
using System.Threading.Tasks;

namespace APWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet("GetServerNodeID")]
        public async Task<IActionResult> GetServerNodeID()
        {
            var result = await _commonService.GetServerNodeIDAsync();

            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Error = result.ErrorMessage });
            }

            return Ok(new { NodeID = result.Data, Message = result.Message });
        }
    }
}
