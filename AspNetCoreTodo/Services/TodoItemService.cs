using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        public readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<TodoItem> GetItemByIdAsync(IdentityUser user, Guid Id)
        {
            return await _context.Items.Where(x => x.Id == Id && x.UserId == user.Id)
                .SingleOrDefaultAsync();
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user, TodoItemList itemList)
        {
            return await _context.Items
                .Where(x => x.IsDone == false && x.UserId == user.Id  && x.ItemListId == itemList.Id)
                .OrderBy(x => x.DueAt)
                .ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(TodoItem newItem, TodoItemList itemList, ItemCategory itemTag, IdentityUser user)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.UserId = user.Id;
            newItem.ItemCategory = itemTag.ItemCategoryName;

            var itemtag = await _context.ItemCategory.Where(x => x.Id == itemTag.Id).SingleOrDefaultAsync();

            newItem.ItemCategory = itemtag.ItemCategoryName;

            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;
            item.DateRemoved = DateTime.Now;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; //one entity should have been updated
        }

        public async Task<IEnumerable<ItemCategory>> GetExistingItemCategoriesAsync(IdentityUser user, TodoItemList itemList)
        {
            return await _context.ItemCategory.
                Where(x => x.UserId == user.Id && x.ItemListId == itemList.Id).ToListAsync();
        }

        public async Task<bool> AddNewItemCategoryAsync(ItemCategory itemCategory, IdentityUser user)
        {
            itemCategory.UserId = user.Id;
            itemCategory.Id = Guid.NewGuid();

            if (itemCategory.ItemCategoryName == null)
            {
                return false;
            }
            _context.ItemCategory.Add(itemCategory);
            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<bool> RemoveItemCategoryAsync(ItemCategory itemCategory, IdentityUser user)
        {
            itemCategory.UserId = user.Id;

            if (itemCategory.Id == null)
            {
                return false;
            }
            //_context.ItemCategory.Add(itemCategory);

            var result = _context.Remove(_context.ItemCategory.Single(x => x.UserId == user.Id && x.Id == itemCategory.Id && x.ItemListId == itemCategory.ItemListId));

            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<bool> UndoLastRemovedItem(IdentityUser user, TodoItemList UndoItemList)
        {
            var item = await _context.Items
                .Where(x => x.UserId == user.Id && x.IsDone == true && x.ItemListId == UndoItemList.Id)
                .OrderByDescending(x => x.DateRemoved)
               .FirstOrDefaultAsync();

            if (item == null)
            {
                return false;
            }
            else if (item.IsDone == true)
            {
                item.IsDone = false;
                item.DateRemoved = null;
            }
            var saveResult = await _context.SaveChangesAsync();
            return true;
        }
    }
}