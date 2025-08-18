using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Authorize]
    public sealed class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateAndUpdateCategoryDto createCategoryDto)
        {
            var result = await _categoryService.CreateAsync(createCategoryDto);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAndUpdateCategoryDto updateCategoryDto)
        {
            var result = await _categoryService.UpdateAsync(id, updateCategoryDto);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return CreateActionResult(result);
        }
    }
}