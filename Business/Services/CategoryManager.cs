using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.DTOs.CategoryDTOs;
using BabsKitapEvi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TS.Result;

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

        public async Task<Result<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Result<CategoryDto>.Succeed(_mapper.Map<CategoryDto>(category));
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return Result<string>.Failure(404, "Category not found.");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Result<string>.Succeed("Category deleted successfully.");
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return Result<IEnumerable<CategoryDto>>.Succeed(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        public async Task<Result<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return Result<CategoryDto>.Failure(404, "Category not found.");
            }
            return Result<CategoryDto>.Succeed(_mapper.Map<CategoryDto>(category));
        }

        public async Task<Result<string>> UpdateAsync(int id, CategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return Result<string>.Failure(404, "Category not found.");
            }
            _mapper.Map(categoryDto, category);
            await _context.SaveChangesAsync();
            return Result<string>.Succeed("Category updated successfully.");
        }
    }
}