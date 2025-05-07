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
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly IUserService _userService;

    public TeachersController(ITeacherService teacherService, IUserService userService)
    {
        _teacherService = teacherService;
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
    public async Task<IActionResult> CreateTeacher([FromBody] TeacherDto dto)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _teacherService.CreateTeacherAsync(dto, requester);
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
    public async Task<IActionResult> GetAllTeachers()
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _teacherService.GetAllTeachersAsync(requester);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacherById(int id)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _teacherService.GetTeacherByIdAsync(id, requester);
            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var success = await _teacherService.DeleteTeacherAsync(id, requester);
            if (!success) return NotFound();

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }
}
