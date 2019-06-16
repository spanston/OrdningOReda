using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem> GetItemByIdAsync(IdentityUser user, Guid Id);
        Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user, TodoList list);
        Task<bool> AddItemAsync(TodoItem newItem, TodoList lIst, ItemCategory itemTag, IdentityUser user);
        Task<bool> MarkDoneAsync(Guid id, IdentityUser user);
        Task<IEnumerable<ItemCategory>> GetExistingItemCategoriesAsync(IdentityUser user, TodoList list
        );
        Task<bool> AddNewItemCategoryAsync(ItemCategory itemCategory, IdentityUser user);
        Task<bool> RemoveItemCategoryAsync(ItemCategory itemCategory, IdentityUser user);
        Task<bool> UndoLastRemovedItem(IdentityUser user, TodoList undoList);
    }
}