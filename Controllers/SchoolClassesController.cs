using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Interfaces;
using e_journal.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_journal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolClassesController : ControllerBase
{
    private readonly ISchoolClassService _classService;
    private readonly IUserService _userService;

    public SchoolClassesController(ISchoolClassService classService, IUserService userService)
    {
        _classService = classService;
        _userService = userService;
    }

    private async Task<User?> GetRequesterAsync()
    {
        var userIdHeader = Request.Headers["UserId"].FirstOrDefault();
        if (!int.TryParse(userIdHeader, out var userId))
            return null;

        return await _userService.GetByIdAsync(userId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClass([FromBody] SchoolClassDto dto)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _classService.CreateClassAsync(dto, requester);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClasses()
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _classService.GetAllClassesAsync(requester);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassById(int id)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _classService.GetClassByIdAsync(id, requester);
            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var success = await _classService.DeleteClassAsync(id, requester);
            if (!success) return NotFound();

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }
}
