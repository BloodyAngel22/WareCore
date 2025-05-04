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
    public class CategoryController(CategoryService categoryService) : ControllerBase
    {
        private readonly CategoryService _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategories();

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryById(id);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTORequest category)
        {
            var result = await _categoryService.CreateCategory(category);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTORequest category)
        {
            var result = await _categoryService.UpdateCategory(id, category);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate()
        {
            var result = await _categoryService.GenerateCategory();

            return result.Success
                ? ResponseHelper.Ok(result.Data)
                : ResponseHelper.Error(result.Message);
        }
    }
}