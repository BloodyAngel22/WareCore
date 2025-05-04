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
    public class ShelfService(IShelfRepository shelfRepository, ICategoryRepository categoryRepository, IWarehouseRepository warehouseRepository, IValidator<ShelfDTORequest> shelfValidator, ILogger<ShelfService> logger)
    {
        private readonly IShelfRepository _shelfRepository = shelfRepository;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IWarehouseRepository _warehouseRepository = warehouseRepository;
        private readonly IValidator<ShelfDTORequest> _shelfValidator = shelfValidator;

        private readonly ILogger<ShelfService> _logger = logger;

        public async Task<ServiceResultHelper<IEnumerable<Shelf>>> GetShelvesByWarehouseId(Guid warehouseId)
        {
            try
            {
                var shelves = await _shelfRepository.GetShelvesByWarehouseId(warehouseId);
                return ServiceResultHelper<IEnumerable<Shelf>>.Ok(shelves);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<IEnumerable<Shelf>>.Fail();
            }
        }

        public async Task<ServiceResultHelper<IEnumerable<Shelf>>> GetAllShelves()
        {
            try
            {
                var shelves = await _shelfRepository.GetShelvesAsync();
                return ServiceResultHelper<IEnumerable<Shelf>>.Ok(shelves);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<IEnumerable<Shelf>>.Fail();
            }
        }

        public async Task<ServiceResultHelper<Shelf>> GetShelfById(Guid id)
        {
            try
            {
                var shelf = await _shelfRepository.GetShelfByIdAsync(id);
                return ServiceResultHelper<Shelf>.Ok(shelf);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<Shelf>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> CreateShelf(ShelfDTORequest shelf)
        {
            try
            {
                var validationResult = await _shelfValidator.ValidateAsync(shelf);

                if (!validationResult.IsValid)
                    throw new ValidateException(ValidatorError.GetErrorsString(ValidatorError.GetErrors(validationResult)));

                var shelfToCreate = new Shelf
                {
                    Id = Guid.NewGuid(),
                    ShelfWeight = shelf.ShelfWeight,
                    CategoryId = shelf.CategoryId,
                    Category = null!,
                    WarehouseId = shelf.WarehouseId
                };

                await _shelfRepository.CreateShelfAsync(shelfToCreate);
                return ServiceResultHelper<string>.Ok("Created shelf successfully");
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

        public async Task<ServiceResultHelper<string>> DeleteShelf(Guid id)
        {
            try
            {
                await _shelfRepository.DeleteShelfAsync(id);
                return ServiceResultHelper<string>.Ok("Deleted shelf successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<string>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> UpdateShelf(Guid id, ShelfDTORequest shelfDTO)
        {
            try
            {
                var validationResult = await _shelfValidator.ValidateAsync(shelfDTO);

                if (!validationResult.IsValid)
                    throw new ValidateException(ValidatorError.GetErrorsString(ValidatorError.GetErrors(validationResult)));

                var canUpdateShelfWeight = await _shelfRepository.CanUpdateShelfWeightAsync(id, shelfDTO.ShelfWeight);

                if (!canUpdateShelfWeight)
                    throw new CustomException("Shelf cannot be sized smaller than current weight of parcels");

                var canUpdateShelfCategory = await _shelfRepository.CanUpdateShelfCategoryAsync(id, shelfDTO.CategoryId);

                if (!canUpdateShelfCategory)
                    throw new CustomException("You can't change the shelf category as long as there are parcels there");

                var shelfToUpdate = new Shelf
                {
                    Id = id,
                    ShelfWeight = shelfDTO.ShelfWeight,
                    CategoryId = shelfDTO.CategoryId,
                    Category = await _categoryRepository.GetCategoryByIdAsync(shelfDTO.CategoryId)
                };

                await _shelfRepository.UpdateShelfAsync(shelfToUpdate);
                return ServiceResultHelper<string>.Ok("Updated shelf successfully");
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

        public async Task<ServiceResultHelper<ShelfDTORequest>> GenerateShelf()
        {
            try
            {
                var faker = new Faker();

                var availableCategoryIds = await _categoryRepository.GetAvailableCategoryIdsAsync();

                var availableWarehouseIds = await _warehouseRepository.GetAvailableWarehouseIdsAsync();

                if (!availableCategoryIds.Any() || !availableWarehouseIds.Any())
                    throw new CustomException("No warehouses or categories available");

                var fakeShelf = new ShelfDTORequest
                {
                    ShelfWeight = faker.Random.Float(1, 2048),
                    CategoryId = faker.PickRandom(availableCategoryIds),
                    WarehouseId = faker.PickRandom(availableWarehouseIds)
                };

                return ServiceResultHelper<ShelfDTORequest>.Ok(fakeShelf);
            }
            catch (CustomException e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<ShelfDTORequest>.Fail(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<ShelfDTORequest>.Fail();
            }
        }
    }
}