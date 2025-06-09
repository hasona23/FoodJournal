using FoodJournal.API.Models;
using FoodJournal.API.Services;
using FoodJournal.Shared.Models;
using FoodJournal.Shared.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodJournal.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MealsController : ControllerBase
{
    private IMealService _mealService;
    private UserManager<AppUser> _userManager;

    public MealsController(IMealService mealService, UserManager<AppUser> userManager)
    {
        _mealService = mealService;
        _userManager = userManager;
    }

    private async Task<AppUser> GetUser()
    {
        return await _userManager.FindByEmailAsync(User.Claims.First(c => c.Type == ClaimTypes.Email).Value);
    }

    [HttpPost]
    public async Task<ActionResult> CreateMeal(MealCreateDTO mealCreateDTO)

    {
        if (string.IsNullOrEmpty(mealCreateDTO.Name.Trim()))
            return BadRequest(new Result("Name cant be empty/null"));

        var result = await _mealService.CreateMealAsync(mealCreateDTO, (await GetUser()).Id);
        if (result.IsSuccess())
        {
            return Created();
        }
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeal(int id)
    {
        var result = await _mealService.DeleteMealAsync(id, (await GetUser()).Id);
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
        if (string.IsNullOrEmpty(mealDto.Name.Trim()))
            return BadRequest(new Result("Name cant be empty/null"));

        var result = await _mealService.UpdateMealAsync(mealDto, (await GetUser()).Id);
        if (result.IsSuccess())
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MealGetDTOWithFoods>> GetMealById(int id)
    {
        var result = await _mealService.GetMealByIdAsync(id, (await GetUser()).Id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Response);
    }

    [HttpGet]
    public async Task<ActionResult<List<MealGetDTOWithFoods>>> GetAllMeal()
    {
        var result = await _mealService.GetAllMealsWithFoodAsync((await GetUser()).Id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Response);
    }
}
