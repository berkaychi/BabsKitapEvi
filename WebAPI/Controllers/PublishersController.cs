using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.PublisherDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : CustomBaseController
    {
        private readonly IPublisherService _publisherService;

        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _publisherService.GetAllAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _publisherService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAndUpdatePublisherDto newPublisherDto)
        {
            var result = await _publisherService.CreateAsync(newPublisherDto);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAndUpdatePublisherDto updatePublisherDto)
        {
            var result = await _publisherService.UpdateAsync(id, updatePublisherDto);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _publisherService.DeleteAsync(id);
            return CreateActionResult(result);
        }
    }
}