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
public class FoodController : ControllerBase
{
    private IFoodService _foodService;
    private UserManager<AppUser> _userManager;

    public FoodController(IFoodService foodService, UserManager<AppUser> userManager)
    {
        _foodService = foodService;
        _userManager = userManager;

    }

    private async Task<AppUser> GetUser()
    {
        return await _userManager.FindByEmailAsync(User.Claims.First(c => c.Type == ClaimTypes.Email).Value);
    }
    [HttpPost]
    public async Task<ActionResult> CreateFood(FoodCreateDTO foodCreateDto)
    {

        if (string.IsNullOrEmpty(foodCreateDto.Name.Trim()))
            return BadRequest(new Result("Name cant be empty/null"));
        var result = await _foodService.CreateFoodAsync(foodCreateDto, (await GetUser()).Id);
        if (result.IsSuccess())
        {
            return Created();
        }
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFood(int id)
    {
        var result = await _foodService.DeleteFoodAsync(id, (await GetUser()).Id);
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
        if (string.IsNullOrEmpty(foodDto.Name.Trim()))
            return BadRequest(new Result("Name cant be empty/null"));
        var result = await _foodService.UpdateFoodAsync(foodDto, (await GetUser()).Id);
        if (result.IsSuccess())
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FoodGetDTOWithMeals>> GetFoodById(int id)
    {
        var result = await _foodService.GetFoodByIdAsync(id, (await GetUser()).Id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Response);
    }

    [HttpGet]

    public async Task<ActionResult<List<FoodGetDTOWithMeals>>> GetAllFood()
    {
        var result = await _foodService.GetAllFoodWithMealsAsync((await GetUser()).Id);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Response);
    }
}
