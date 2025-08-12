using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using BabsKitapEvi.Entities.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Authorize]
    public sealed class BooksController : CustomBaseController
    {
        private readonly IBookService _bookService;
        private readonly IImageUploadService _imageUploadService;
        private readonly IValidator<UpdateBookImageDto> _updateImageValidator;

        public BooksController(IBookService bookService, IImageUploadService imageUploadService, IValidator<UpdateBookImageDto> updateImageValidator)
        {
            _bookService = bookService;
            _imageUploadService = imageUploadService;
            _updateImageValidator = updateImageValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _bookService.GetAllAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }

        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryBooks(int categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _bookService.GetByCategoryIdAsync(categoryId, pageNumber, pageSize);
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateBookDto createBookDto, CancellationToken ct)
        {
            string? imageUrl = null;
            string? imagePublicId = null;
            if (createBookDto.ImageFile is { Length: > 0 })
            {
                using var stream = createBookDto.ImageFile.OpenReadStream();
                var upload = await _imageUploadService.UploadImageAsync(stream, createBookDto.ImageFile.FileName, createBookDto.ImageFile.ContentType, "babs-kitap-evi/books", ct);
                if (upload != null)
                {
                    imageUrl = upload.Url;
                    imagePublicId = upload.PublicId;
                }
            }

            var result = await _bookService.CreateAsync(createBookDto, imageUrl, imagePublicId, ct);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto updateBookDto, CancellationToken ct)
        {
            var result = await _bookService.UpdateAsync(id, updateBookDto, null, null, ct);
            return CreateActionResult(result);
        }

        [HttpPut("{id}/image")]
        [Authorize(Roles = Roles.Admin)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage(int id, IFormFile imageFile, CancellationToken ct)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(imageFile.ContentType.ToLower()))
            {
                return BadRequest("Invalid image type. Allowed types are: jpeg, jpg, png, gif, webp.");
            }

            if (imageFile.Length > 5 * 1024 * 1024)
            {
                return BadRequest("File size cannot exceed 5MB.");
            }

            var result = await _bookService.UpdateImageAsync(id, imageFile, ct);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _bookService.DeleteAsync(id, ct);
            return CreateActionResult(result);
        }
    }
}