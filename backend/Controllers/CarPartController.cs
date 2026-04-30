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
            return Ok(await _carPartModel.GetAllCarPartsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarPartGetDto>> GetById(string id)
        {
            try
            {
                return Ok(await _carPartModel.GetCarPartByIdAsync(id));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CarPartDto updatedPartDto)
        {
            try
            {
                await _carPartModel.UpdateCarPartAsync(id, updatedPartDto);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CarPartDto createPartDto)
        {
            try
            {
                var tempDto = await _carPartModel.CreateCarPartAsync(createPartDto);
                return CreatedAtAction(nameof(GetById), new { id = tempDto.Id }, tempDto);
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
