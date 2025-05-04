using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Application.DTOs.Request;
using backend.Application.Helpers;
using backend.Application.Services;
using backend.Core.Models;
using backend.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelfController(ShelfService shelfService) : ControllerBase
    {
        private readonly ShelfService _shelfService = shelfService;

        [HttpGet]
        public async Task<IActionResult> GetShelves([FromQuery] Guid? warehouseId)
        {
            ServiceResultHelper<IEnumerable<Shelf>> result;

            if (warehouseId.HasValue)
            {
                result = await _shelfService.GetShelvesByWarehouseId(warehouseId.Value);
            }
            else
            {
                result = await _shelfService.GetAllShelves();
            }

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShelfById(Guid id)
        {
            var result = await _shelfService.GetShelfById(id);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShelf([FromBody] ShelfDTORequest shelf)
        {
            var result = await _shelfService.CreateShelf(shelf);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShelf(Guid id, [FromBody] ShelfDTORequest shelf)
        {
            var result = await _shelfService.UpdateShelf(id, shelf);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShelf(Guid id)
        {
            var result = await _shelfService.DeleteShelf(id);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate()
        {
            var result = await _shelfService.GenerateShelf();

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }
    }
}