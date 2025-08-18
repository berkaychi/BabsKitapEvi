using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using BabsKitapEvi.Entities.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using TS.Result;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Authorize]
    public sealed class BooksController : CustomBaseController
    {
        private readonly IBookService _bookService;
        private readonly IImageUploadService _imageUploadService;
        private readonly IValidator<UpdateBookImageDto> _updateImageValidator;
        private readonly IValidator<CreateBookDto> _createBookValidator;
        private readonly IValidator<UpdateBookDto> _updateBookValidator;

        public BooksController(IBookService bookService, IImageUploadService imageUploadService, IValidator<UpdateBookImageDto> updateImageValidator, IValidator<CreateBookDto> createBookValidator, IValidator<UpdateBookDto> updateBookValidator)
        {
            _bookService = bookService;
            _imageUploadService = imageUploadService;
            _updateImageValidator = updateImageValidator;
            _createBookValidator = createBookValidator;
            _updateBookValidator = updateBookValidator;
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

        [HttpGet("publisher/{publisherId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublisherBooks(int publisherId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _bookService.GetByPublisherIdAsync(publisherId, pageNumber, pageSize);
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
            ValidationResult validationResult = await _createBookValidator.ValidateAsync(createBookDto, ct);
            if (!validationResult.IsValid)
            {
                return CreateActionResult(Result<string>.Failure(400, validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

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
            updateBookDto.Id = id;
            ValidationResult validationResult = await _updateBookValidator.ValidateAsync(updateBookDto, ct);
            if (!validationResult.IsValid)
            {
                return CreateActionResult(Result<string>.Failure(400, validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _bookService.UpdateAsync(id, updateBookDto, ct);
            return CreateActionResult(result);
        }

        [HttpPut("{id}/image")]
        [Authorize(Roles = Roles.Admin)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage(int id, UpdateBookImageDto updateBookImageDto, CancellationToken ct)
        {

            ValidationResult validationResult = await _updateImageValidator.ValidateAsync(updateBookImageDto, cancellation: ct);

            if (!validationResult.IsValid)
            {
                return CreateActionResult(Result<string>.Failure(400, validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _bookService.UpdateImageAsync(id, updateBookImageDto.ImageFile, ct);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _bookService.DeleteAsync(id, ct);
            return CreateActionResult(result);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] BooksQuery query, CancellationToken ct)
        {
            var result = await _bookService.SearchAsync(query, ct);
            return CreateActionResult(result);
        }
    }
}