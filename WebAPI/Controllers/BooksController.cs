using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Entities.DTOs.BookDTOs;
using BabsKitapEvi.Entities.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Authorize]
    public sealed class BooksController : CustomBaseController
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _bookService.GetAllAsync(pageNumber, pageSize);
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
        public async Task<IActionResult> Create([FromBody] CreateBookDto createBookDto)
        {
            var result = await _bookService.CreateAsync(createBookDto);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto updateBookDto)
        {
            var result = await _bookService.UpdateAsync(id, updateBookDto);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteAsync(id);
            return CreateActionResult(result);
        }
    }
}