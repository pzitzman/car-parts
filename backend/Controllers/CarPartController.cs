using backend.Data.Entity;
using backend.DTOs;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarPartController : ControllerBase
    {
        private readonly CarPartModel _carPartModel;

        public CarPartController(CarPartModel carPartModel)
        {
            _carPartModel = carPartModel;
        }

        [HttpGet]
        public async Task<ActionResult<List<CarPartGetDto>>> Get()
        {
            var parts = await _carPartModel.GetAllCarPartsAsync();

            var dtos = parts
                .Select(part => new CarPartGetDto
                {
                    Id = part.Id,
                    Name = part.Name,
                    PartNumber = part.PartNumber,
                    Description = part.Description,
                    CreatedAt = part.CreatedAt,
                    UpdatedAt = part.UpdatedAt,
                })
                .ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarPartGetDto>> GetById(string id)
        {
            var tempPart = await _carPartModel.GetCarPartByIdAsync(id);

            if (tempPart == null)
            {
                return NotFound($"Car Part not found with{id}");
            }

            var dto = new CarPartGetDto
            {
                Id = tempPart.Id,
                Name = tempPart.Name,
                PartNumber = tempPart.PartNumber,
                Description = tempPart.Description,
                CreatedAt = tempPart.CreatedAt,
                UpdatedAt = tempPart.UpdatedAt,
            };

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CarPartUpdateDto updatedPartDto)
        {
            var carPartToUpdate = new CarPart
            {
                Id = id,
                Name = updatedPartDto.Name,
                PartNumber = updatedPartDto.PartNumber,
                Description = updatedPartDto.Description,
            };
            try
            {
                await _carPartModel.UpdateCarPartAsync(id, carPartToUpdate);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Car Part not found with{id}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CarPartCreateDto createPartDto)
        {
            var tempPart = new CarPart
            {
                Name = createPartDto.Name,
                PartNumber = createPartDto.PartNumber,
                Description = createPartDto.Description,
            };
            try
            {
                await _carPartModel.CreateCarPartAsync(tempPart);
                return CreatedAtAction(nameof(Get), new { id = tempPart.Id }, tempPart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _carPartModel.DeleteCarPartAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Cannot delete. Car Part not found with{id}");
            }
        }
    }
}
