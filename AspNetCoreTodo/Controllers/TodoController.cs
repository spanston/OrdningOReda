using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        //Dependency Injection
        private readonly ITodoItemService _todoItemService;
        private readonly ITodoItemListService _todoItemListService;
        private readonly UserManager<IdentityUser> _userManager;


        public TodoController(ITodoItemService todoItemService, ITodoItemListService todoItemListSericve, UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
            _todoItemListService = todoItemListSericve;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var todoItemLists = await _todoItemListService.GetAllItemListForUser(currentUser);

            var model = new TodoListViewModel()
            {
                TodoItemLists = todoItemLists

            };
            return View(model);
        }
        
        public async Task<IActionResult> ItemList(TodoItemList itemList)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            
            
            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser, itemList);
            var categories = await _todoItemService.GetExistingItemCategoriesAsync(currentUser, itemList);

            ViewBag.listofTags = categories;
            ViewBag.ItemList = itemList;
            // Put items into a model
            var model = new TodoViewModel()
            {
                Items = items,
                PriorityTagsList = categories,
                ItemList = itemList
            };
            //Render view using the model            
            return View(model);
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToDoItemList(TodoItemList todoList)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            //TODO: Value is not used, returns how many saved...
            var successful = await _todoItemListService.AddItemListForUser(currentUser, todoList);

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveToDoItemList(TodoItemList todoItemList)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            
            var successful = await _todoItemListService.RemoveItemListForUser(currentUser, todoItemList.Id);

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem, TodoItemList itemList,  ItemCategory itemCategory)
        {
            
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ItemList");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var successful = await _todoItemService.AddItemAsync(newItem, itemList, itemCategory, currentUser);
            if (!successful)
            {
                return BadRequest("Could not add item.");
            }
            //TODO: find a better solution
            itemList.Id = newItem.ItemListId;
            return RedirectToAction("ItemList", "Todo", itemList);


        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("ItemList");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var item = await _todoItemService.GetItemByIdAsync(currentUser, id);
            var itemList = await _todoItemListService.GetItemListById(currentUser, item.ItemListId);
            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);


            if (!successful)
            {
                return BadRequest("Could not mot mark item as done");
            }

            return RedirectToAction("ItemList", "Todo", itemList);
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItemCategory(ItemCategory newCategory)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ItemList");
            }
            

            var currentUser = await _userManager.GetUserAsync(User);
            var itemList = await _todoItemListService.GetItemListById(currentUser, newCategory.ItemListId);
            if (currentUser == null)
            {
                return Challenge();
            }
            var successful = await _todoItemService.AddNewItemCategoryAsync(newCategory, currentUser);

            return RedirectToAction("ItemList", "Todo", itemList);
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItemCategory(ItemCategory newCategory)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ItemList");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }
            var itemList = await _todoItemListService.GetItemListById(currentUser, newCategory.ItemListId);

            var successful = await _todoItemService.RemoveItemCategoryAsync(newCategory, currentUser);


            return RedirectToAction("ItemList", "Todo", itemList);
        }



        public async Task<IActionResult> UndoLastRemovedItem(TodoItemList itemList)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ItemList");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }
            //var itemList = await _todoItemListService.GetItemListById(currentUser, itemList.ItemListId);
            var successful = await _todoItemService.UndoLastRemovedItem(currentUser, itemList);
            return RedirectToAction("ItemList", "Todo", itemList);
        }
    }
}

