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
    public class ParcelController(ParcelService parcelService) : ControllerBase
    {
        private readonly ParcelService _parcelService = parcelService;

        [HttpGet]
        public async Task<IActionResult> GetAllParcels([FromQuery] Guid? warehouseId)
        {
            ServiceResultHelper<IEnumerable<Parcel>> result;

            if (warehouseId.HasValue)
            {
                result = await _parcelService.GetParcelsByWarehouseId(warehouseId.Value);
            }
            else
            {
                result = await _parcelService.GetAllParcels();
            }

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParcelById(Guid id)
        {
            var result = await _parcelService.GetParcelById(id);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParcel(Guid id)
        {
            var result = await _parcelService.DeleteParcel(id);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParcel(Guid id, [FromBody] ParcelDTORequest parcel)
        {
            var result = await _parcelService.UpdateParcel(id, parcel);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateParcel([FromBody] ParcelDTORequest parcel)
        {
            var result = await _parcelService.AddParcel(parcel);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate()
        {
            var result = await _parcelService.GenerateParcel();

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }
    }
}