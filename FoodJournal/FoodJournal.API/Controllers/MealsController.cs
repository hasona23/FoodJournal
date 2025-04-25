using FoodJournal.API.Services;
using FoodJournal.Shared.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FoodJournal.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MealsController : ControllerBase
{
    private IMealService _mealService;
    public MealsController(IMealService mealService)
    {
        _mealService = mealService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateMeal(MealCreateDTO mealCreateDTO)
    {
        var result = await _mealService.CreateMealAsync(mealCreateDTO);
        if (result.IsSuccess())
        {
            return Created();
        }
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeal(int id)
    {
        var result = await _mealService.DeleteMealAsync(id);
        if (result.IsSuccess())
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMeal(int id, MealUpdateDTO mealDto)
    {
        if (id != mealDto.Id)
        {
            return BadRequest("ID DOESNT MATCH");
        }
        var result = await _mealService.UpdateMealAsync(id, mealDto);
        if (result.IsSuccess())
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MealGetDTOWithFoods>> GetMealById(int id)
    {
        var result = await _mealService.GetMealByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Response);
    }

    [HttpGet]
    public async Task<ActionResult<List<MealGetDTOWithFoods>>> GetAllMeal()
    {
        var result = await _mealService.GetAllMealsWithFoodAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Response);
    }
}
