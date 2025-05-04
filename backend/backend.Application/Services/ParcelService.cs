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
    public class ParcelService(IParcelRepository parcelRepository, IShelfRepository shelfRepository, IWarehouseRepository warehouseRepository, ICategoryRepository categoryRepository, IValidator<ParcelDTORequest> parcelValidator, ILogger<ParcelService> logger)
    {
        private readonly IParcelRepository _parcelRepository = parcelRepository;
        private readonly IShelfRepository _shelfRepository = shelfRepository;
        private readonly IWarehouseRepository _warehouseRepository = warehouseRepository;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IValidator<ParcelDTORequest> _parcelValidator = parcelValidator;

        private readonly ILogger<ParcelService> _logger = logger;

        public async Task<ServiceResultHelper<IEnumerable<Parcel>>> GetAllParcels()
        {
            try
            {
                var parcels = await _parcelRepository.GetParcelsAsync();
                return ServiceResultHelper<IEnumerable<Parcel>>.Ok(parcels);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<IEnumerable<Parcel>>.Fail();
            }
        }

        public async Task<ServiceResultHelper<IEnumerable<Parcel>>> GetParcelsByWarehouseId(Guid shelfId)
        {
            try
            {
                var parcels = await _parcelRepository.GetParcelsByWarehouseIdAsync(shelfId);
                return ServiceResultHelper<IEnumerable<Parcel>>.Ok(parcels);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<IEnumerable<Parcel>>.Fail();
            }
        }

        public async Task<ServiceResultHelper<Parcel>> GetParcelById(Guid id)
        {
            try
            {
                var parcel = await _parcelRepository.GetParcelByIdAsync(id);
                return ServiceResultHelper<Parcel>.Ok(parcel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<Parcel>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> DeleteParcel(Guid id)
        {
            try
            {
                await _parcelRepository.DeleteParcelAsync(id);
                return ServiceResultHelper<string>.Ok("Deleted parcel successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<string>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> AddParcel(ParcelDTORequest parcel)
        {
            try
            {
                var validationResult = await _parcelValidator.ValidateAsync(parcel);

                if (!validationResult.IsValid)
                    throw new ValidateException(ValidatorError.GetErrorsString(ValidatorError.GetErrors(validationResult)));

                var shelf = await _shelfRepository.GetAvailableShelfWithCategoryAndForWeight(parcel.WarehouseId, parcel.CategoryId, parcel.Weight);

                var newParcel = new Parcel
                {
                    Id = Guid.NewGuid(),
                    Name = parcel.Name,
                    Weight = parcel.Weight,
                    ShelfId = shelf.Id,
                    CategoryId = parcel.CategoryId,
                    Category = null!
                };

                await _parcelRepository.AddParcelAsync(newParcel);

                return ServiceResultHelper<string>.Ok("Added parcel successfully");
            }
            catch (ValidateException e)
            {
                _logger.LogError($"Error/s: {e.Message}");
                return ServiceResultHelper<string>.Fail(e.Message);
            }
            catch (CustomException e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<string>.Fail(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<string>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> UpdateParcel(Guid id, ParcelDTORequest parcel)
        {
            try
            {
                var validationResult = await _parcelValidator.ValidateAsync(parcel);

                if (!validationResult.IsValid)
                    throw new ValidateException(ValidatorError.GetErrorsString(ValidatorError.GetErrors(validationResult)));

                var shelf = await _shelfRepository.GetAvailableShelfWithCategoryAndForWeight(parcel.WarehouseId, parcel.CategoryId, parcel.Weight);

                var parcelToUpdate = new Parcel
                {
                    Id = id,
                    Name = parcel.Name,
                    Weight = parcel.Weight,
                    ShelfId = shelf.Id,
                    CategoryId = parcel.CategoryId,
                    Category = null!
                };

                await _parcelRepository.UpdateParcelAsync(parcelToUpdate);
                return ServiceResultHelper<string>.Ok("Updated parcel successfully");
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

        public async Task<ServiceResultHelper<ParcelDTORequest>> GenerateParcel()
        {
            try
            {
                var faker = new Faker();

                var availableWarehouseIds = await _warehouseRepository.GetAvailableWarehouseIdsAsync();
                var availableCategoryIds = await _categoryRepository.GetAvailableCategoryIdsAsync();

                if (!availableCategoryIds.Any() || !availableWarehouseIds.Any())
                    throw new CustomException("No warehouses or categories available");

                var fakeParcel = new ParcelDTORequest
                {
                    Name = faker.Commerce.Product(),
                    Weight = faker.Random.Float(1, 10),
                    CategoryId = faker.PickRandom(availableCategoryIds),
                    WarehouseId = faker.PickRandom(availableWarehouseIds)
                };

                return ServiceResultHelper<ParcelDTORequest>.Ok(fakeParcel);
            }
            catch (CustomException e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<ParcelDTORequest>.Fail(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<ParcelDTORequest>.Fail();
            }
        }
    }
}