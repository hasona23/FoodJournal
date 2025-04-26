using FoodJournal.API.Services;
using FoodJournal.Shared.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FoodJournal.API.Controllers;
[Route("api/[controller]")]
[ApiController]

public class FoodController : ControllerBase
{
    private IFoodService _foodService;
    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateFood(FoodCreateDTO foodCreateDto)
    {
        var result = await _foodService.CreateFoodAsync(foodCreateDto);
        if (result.IsSuccess())
        {
            return Created();
        }
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFood(int id)
    {
        var result = await _foodService.DeleteFoodAsync(id);
        if (result.IsSuccess())
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateFood(int id, FoodUpdateDTO foodDto)
    {
        if (id != foodDto.Id)
        {
            return BadRequest("ID DOESNT MATCH");
        }
        var result = await _foodService.UpdateFoodAsync(id, foodDto);
        if (result.IsSuccess())
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FoodGetDTOWithMeals>> GetFoodById(int id)
    {
        var result = await _foodService.GetFoodByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Response);
    }

    [HttpGet]
    public async Task<ActionResult<List<FoodGetDTOWithMeals>>> GetAllFood()
    {
        var result = await _foodService.GetAllFoodWithMealsAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Response);
    }
}
