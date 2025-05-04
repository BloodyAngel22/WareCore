using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Application.DTOs.Request;
using backend.Application.Services;
using backend.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController(WarehouseService warehouseService) : ControllerBase
    {
        private readonly WarehouseService _warehouseService = warehouseService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _warehouseService.GetAllWarehouses();

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _warehouseService.GetWarehouseById(id);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WarehouseDTORequest warehouse)
        {
            var result = await _warehouseService.CreateWarehouse(warehouse);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid id, [FromBody] WarehouseDTORequest warehouse)
        {
            var result = await _warehouseService.UpdateWarehouse(id, warehouse);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _warehouseService.DeleteWarehouse(id);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate()
        {
            var result = await _warehouseService.GenerateWarehouse();

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }
    }
}