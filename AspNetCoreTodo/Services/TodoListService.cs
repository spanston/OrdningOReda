using System;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly ApplicationDbContext _context;

        public TodoListService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<TodoList>> GetAllTodoListForUser(IdentityUser user)
        {
            var itemLists = await _context.TodoItemList.Where(x => x.UserId == user.Id).ToListAsync();

            return itemLists;
        }

        public async Task<TodoList> GetTodoListById(IdentityUser user, Guid listId)
        {
            var itemList = await _context.TodoItemList.Where(x => x.Id == listId && x.UserId == user.Id)
                .SingleOrDefaultAsync();

            return itemList;
        }

        public async Task<bool> RemoveTodoListForUser(IdentityUser user, Guid listId)
        {
            if (listId == null)
            {
                return false;
            }

            var result = _context.Remove(_context.TodoItemList.Single(x => x.UserId == user.Id && x.Id == listId));

            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<bool> AddTodoListForUser(IdentityUser user, TodoList todoList)
        {
            todoList.Id = new Guid();
            todoList.UserId = user.Id;

            _context.TodoItemList.Add(todoList);

            var result = await _context.SaveChangesAsync();

            return result == 1;
        }
    }
}