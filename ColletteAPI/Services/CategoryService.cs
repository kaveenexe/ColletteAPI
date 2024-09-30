﻿using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using ColletteAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = "Default Description" // If needed
            };

            var newCategory = await _categoryRepository.AddAsync(category);

            return new CategoryDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name
            };
        }

        public async Task<CategoryDto> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
            if (category == null) return null;

            category.Name = categoryDto.Name;

            var updatedCategory = await _categoryRepository.UpdateAsync(category);

            return new CategoryDto
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name
            };
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}

