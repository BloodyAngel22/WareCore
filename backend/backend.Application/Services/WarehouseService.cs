using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Application.DTOs.Request;
using backend.Application.Helpers;
using backend.Core.Exceptions;
using backend.Core.IRepositories;
using backend.Core.Models;
using Bogus;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class WarehouseService(IWarehouseRepository warehouseRepository, IValidator<WarehouseDTORequest> warehouseValidator, ILogger<WarehouseService> logger)
    {
        private readonly IWarehouseRepository _warehouseRepository = warehouseRepository;
        private readonly IValidator<WarehouseDTORequest> _warehouseValidator = warehouseValidator;

        private readonly ILogger<WarehouseService> _logger = logger;

        public async Task<ServiceResultHelper<IEnumerable<Warehouse>>> GetAllWarehouses()
        {
            try
            {
                var warehouses = await _warehouseRepository.GetWarehousesAsync();
                return ServiceResultHelper<IEnumerable<Warehouse>>.Ok(warehouses);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<IEnumerable<Warehouse>>.Fail();
            }
        }

        public async Task<ServiceResultHelper<Warehouse>> GetWarehouseById(Guid id)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(id);
                return ServiceResultHelper<Warehouse>.Ok(warehouse);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<Warehouse>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> CreateWarehouse(WarehouseDTORequest warehouseDTO)
        {
            try
            {
                var validationResult = await _warehouseValidator.ValidateAsync(warehouseDTO);

                if (!validationResult.IsValid)
                    throw new ValidateException(ValidatorError.GetErrorsString(ValidatorError.GetErrors(validationResult)));

                var newWarehouse = new Warehouse
                {
                    Id = Guid.NewGuid(),
                    Name = warehouseDTO.Name
                };

                await _warehouseRepository.CreateWarehouseAsync(newWarehouse);
                return ServiceResultHelper<string>.Ok("Created warehouse successfully");
            }
            catch (ValidateException e)
            {
                _logger.LogError($"Error/s: {e.Message}");
                return ServiceResultHelper<string>.Fail(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<string>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> UpdateWarehouse(Guid id, WarehouseDTORequest warehouseDTO)
        {
            try
            {
                var validationResult = await _warehouseValidator.ValidateAsync(warehouseDTO);

                if (!validationResult.IsValid)
                    throw new ValidateException(ValidatorError.GetErrorsString(ValidatorError.GetErrors(validationResult)));

                var warehouseToUpdate = new Warehouse
                {
                    Id = id,
                    Name = warehouseDTO.Name
                };

                await _warehouseRepository.UpdateWarehouseAsync(warehouseToUpdate);
                return ServiceResultHelper<string>.Ok("Updated warehouse successfully");
            }
            catch (ValidateException e)
            {
                _logger.LogError($"Error/s: {e.Message}");
                return ServiceResultHelper<string>.Fail(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<string>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> DeleteWarehouse(Guid id)
        {
            try
            {
                await _warehouseRepository.DeleteWarehouseAsync(id);
                return ServiceResultHelper<string>.Ok("Deleted warehouse successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<string>.Fail();
            }
        }

        public Task<ServiceResultHelper<WarehouseDTORequest>> GenerateWarehouse()
        {
            var faker = new Faker();

            var fakeWarehouse = new WarehouseDTORequest
            {
                Name = faker.Address.StreetName() + " " + faker.Random.Int(1, 256)
            };

            return Task.FromResult(ServiceResultHelper<WarehouseDTORequest>.Ok(fakeWarehouse));
        }
    }
}