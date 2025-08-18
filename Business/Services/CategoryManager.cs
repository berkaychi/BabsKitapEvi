using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Common.DTOs.CategoryDTOs;
using BabsKitapEvi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Services
{
    public sealed class CategoryManager : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IServiceResult> CreateAsync(CreateAndUpdateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new SuccessDataResult<CategoryDto>(categoryDto, 201, "Category created successfully.");
        }

        public async Task<IServiceResult> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return new ErrorResult(404, "Category not found.");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return new SuccessResult(204, "Category deleted successfully.");
        }

        public async Task<IServiceResult> GetAllAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return new SuccessDataResult<IEnumerable<CategoryDto>>(categoryDtos, 200);
        }

        public async Task<IServiceResult> GetByIdAsync(int id)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return new ErrorResult(404, "Category not found.");
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new SuccessDataResult<CategoryDto>(categoryDto, 200);
        }

        public async Task<IServiceResult> UpdateAsync(int id, CreateAndUpdateCategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return new ErrorResult(404, "Category not found.");
            }
            _mapper.Map(categoryDto, category);
            await _context.SaveChangesAsync();
            return new SuccessResult(204, "Category updated successfully.");
        }
    }
}