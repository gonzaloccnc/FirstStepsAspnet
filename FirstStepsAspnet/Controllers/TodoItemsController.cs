﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstStepsAspnet.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FirstStepsAspnet.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class TodoItemsController : ControllerBase
  {
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
      var id = User.FindFirst("id")!;

      return await _context.TodoItems
          .Where(x => x.UserId == long.Parse(id.Value))
          .Select(x => ItemToDTO(x))
          .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
      var todoItem = await _context.TodoItems.FindAsync(id);

      if (todoItem == null)
      {
        return NotFound();
      }

      return ItemToDTO(todoItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
      if (id != todoDTO.Id)
      {
        return BadRequest();
      }

      var todoItem = await _context.TodoItems.FindAsync(id);

      if (todoItem == null)
      {
        return NotFound();
      }

      todoItem.Name = todoDTO.Name;
      todoItem.IsComplete = todoDTO.IsComplete;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        return NotFound();
      }

      return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
      // this pass the userId from TodoItemDTO but can get from User.FindFirst("id")
      // because this endpoint is from user authenticated
      var userId = User.FindFirst("id")!.Value;
      var todoItem = new TodoItem
      {
        IsComplete = todoDTO.IsComplete,
        Name = todoDTO.Name,
        UserId = long.Parse(userId)
      };

      var todoSaved = _context.TodoItems.Add(todoItem);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTodoItem", new { id = todoSaved.Entity.Id }, ItemToDTO(todoSaved.Entity));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
      var todoItem = await _context.TodoItems.FindAsync(id);
      if (todoItem == null)
      {
        return NotFound();
      }

      _context.TodoItems.Remove(todoItem);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new()
        {
          Id = todoItem.Id,
          Name = todoItem.Name,
          IsComplete = todoItem.IsComplete,
          UserId = todoItem.UserId
        };
  }
}
