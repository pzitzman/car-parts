using backend.Data.Entity;
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
        public async Task<ActionResult<List<CarPart>>> Get()
        {
            var parts = await _carPartModel.GetAllCarPartsAsync();
            return Ok(parts);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CarPart updatedPart)
        {
            if (id != updatedPart.Id)
            {
                return BadRequest("Missmatch between body ID and URL ID");
            }
            await _carPartModel.UpdateCarPartAsync(id, updatedPart);
            return NoContent();
        }
    }
}
