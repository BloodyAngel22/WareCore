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
    public class CategoryService(ICategoryRepository categoryRepository, IValidator<CategoryDTORequest> categoryValidator, ILogger<CategoryService> logger)
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IValidator<CategoryDTORequest> _categoryValidator = categoryValidator;

        private readonly ILogger<CategoryService> _logger = logger;

        public async Task<ServiceResultHelper<IEnumerable<Category>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetCategoriesAsync();
                return ServiceResultHelper<IEnumerable<Category>>.Ok(categories);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<IEnumerable<Category>>.Fail();
            }
        }

        public async Task<ServiceResultHelper<Category>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                return ServiceResultHelper<Category>.Ok(category);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<Category>.Fail();
            }
        }

        public async Task<ServiceResultHelper<string>> CreateCategory(CategoryDTORequest category)
        {
            try
            {
                var validationResult = await _categoryValidator.ValidateAsync(category);

                if (!validationResult.IsValid)
                    throw new ValidateException(ValidatorError.GetErrorsString(ValidatorError.GetErrors(validationResult)));

                var categoryToCreate = new Category
                {
                    Name = category.Name
                };

                await _categoryRepository.AddCategoryAsync(categoryToCreate);
                return ServiceResultHelper<string>.Ok("Created category successfully");
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

        public async Task<ServiceResultHelper<string>> UpdateCategory(int id, CategoryDTORequest category)
        {
            try
            {
                var validationResult = await _categoryValidator.ValidateAsync(category);

                if (!validationResult.IsValid)
                    throw new ValidateException(ValidatorError.GetErrorsString(ValidatorError.GetErrors(validationResult)));

                var categoryToUpdate = new Category
                {
                    Id = id,
                    Name = category.Name
                };

                await _categoryRepository.UpdateCategoryAsync(categoryToUpdate);
                return ServiceResultHelper<string>.Ok("Updated category successfully");
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

        public async Task<ServiceResultHelper<string>> DeleteCategory(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
                return ServiceResultHelper<string>.Ok("Deleted category successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                return ServiceResultHelper<string>.Fail();
            }
        }

        public Task<ServiceResultHelper<CategoryDTORequest>> GenerateCategory()
        {
            var faker = new Faker();

            var categories = faker.Commerce.Categories(1);

            var fakeCategory = new CategoryDTORequest
            {
                Name = categories[0]
            };

            return Task.FromResult(ServiceResultHelper<CategoryDTORequest>.Ok(fakeCategory));
        }
    }
}