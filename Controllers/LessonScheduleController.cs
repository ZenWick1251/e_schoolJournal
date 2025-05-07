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
public class LessonScheduleController : ControllerBase
{
    private readonly ILessonScheduleService _lessonService;
    private readonly IUserService _userService;

    public LessonScheduleController(ILessonScheduleService lessonService, IUserService userService)
    {
        _lessonService = lessonService;
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
    public async Task<IActionResult> CreateLesson([FromBody] LessonScheduleDto dto)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _lessonService.CreateLessonAsync(dto, requester);
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

    [HttpGet("class/{classId}")]
    public async Task<IActionResult> GetScheduleByClass(int classId)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _lessonService.GetScheduleByClassIdAsync(classId, requester);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLessons()
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _lessonService.GetAllLessonsAsync(requester);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpDelete("{lessonId}")]
    public async Task<IActionResult> DeleteLesson(int lessonId)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var success = await _lessonService.DeleteLessonAsync(lessonId, requester);
            if (!success) return NotFound();

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }
}