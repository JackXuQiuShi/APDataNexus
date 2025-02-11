using APWeb.Dtos;
using APWeb.Models;
using APWeb.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace APWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMovementController : ControllerBase
    {
        private readonly IProductMovementService _productMovementService;

        public ProductMovementController(IProductMovementService productMovementService)
        {
            _productMovementService = productMovementService;
        }

        [HttpPost("DraftProductMovement")]
        public async Task<IActionResult> DraftProductMovement(string orderID)
        {
            if (orderID == null)
            {
                return BadRequest("Invalid OrderID.");
            }
            
            var result = await _productMovementService.DraftProductMovementAsync(orderID);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 500, result.ErrorMessage);
            }

            return Ok(new { Message = "ProductMovement drafted successfully.", OrderID = orderID, ProductMovementID =  result.Data});
        }

        [HttpPost("UpdateProductMovementItem")]
        public async Task<IActionResult> UpdateProductMovementItem([FromBody] ProductMovementItemRequest request)
        {
            if (request == null || request.MovementID <= 0)
            {
                return BadRequest("Invalid request. ProductMovementID is required.");
            }

            var result = await _productMovementService.UpdateProductMovementItemAsync(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 500, result.ErrorMessage);
            }

            return Ok(new { Message = "ProductMovement Item updated successfully.", OrderID = request.OrderID, MovementID = request.MovementID, ProductItemID = request.ProductItemID});
        }

        [HttpPost("SubmitProductMovement")]
        public async Task<IActionResult> SubmitProductMovement([FromBody] ProductMovementRequest request)
        {
            if (request == null || request.ProductMovementID <= 0)
            {
                return BadRequest("Invalid request. ProductMovementID is required.");
            }

            var result = await _productMovementService.SubmitProductMovementAsync(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 500, result.ErrorMessage);
            }

            return Ok(new { Message = "ProductMovement submitted successfully.", OrderID = request.OrderID, ProductMovementID = request.ProductMovementID });
        }
    }
}