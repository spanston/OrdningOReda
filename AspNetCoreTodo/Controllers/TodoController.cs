using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


//TODO: Change itemtag to itemcategory

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        //Dependency Injection
        private readonly ITodoItemService _todoItemService;
        private readonly ITodoListService _todoListService;
        private readonly UserManager<IdentityUser> _userManager;


        public TodoController(ITodoItemService todoItemService, ITodoListService todoListService,
            UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
            _todoListService = todoListService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var todoLists = await _todoListService.GetAllTodoListForUser(currentUser);

            var model = new TodoListViewModel()
            {
                TodoItemLists = todoLists
            };
            return View(model);
        }

        //Returns a specific todoList with to-do items
        public async Task<IActionResult> ItemList(TodoList todoList)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();


            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser, todoList);
            var categories = await _todoItemService.GetExistingItemCategoriesAsync(currentUser, todoList);
            var itemList = await _todoListService.GetTodoListById(currentUser, todoList.Id);


            ViewBag.listofTags = categories; //Used for select todoList
            ViewBag.ItemList = itemList;
            // Put items into a model
            var model = new TodoViewModel()
            {
                Items = items,
                PriorityTagsList = categories,
                List = itemList
            };
            //Render view using the model            
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToDoItemList(TodoList todoList)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            //TODO: Value is not used, returns how many saved...
            //var successful = await _todoListService.AddTodoListForUser(currentUser, todoList);

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveToDoItemList(TodoList todoList)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var successful = await _todoListService.RemoveTodoListForUser(currentUser, todoList.Id);

            return RedirectToAction("Index");
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem, ItemCategory itemCategory)
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

            var successful = await _todoItemService.AddItemAsync(newItem, itemCategory, currentUser);

            if (!successful)
            {
                return BadRequest("Could not add item.");
            }

            //TODO: Quickfix so far
            return RedirectToAction("ItemList", "Todo", new TodoList {Id = newItem.ItemListId});
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("ItemList");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var itemMarkedDone = await _todoItemService.GetItemByIdAsync(currentUser, id);
            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);


            if (!successful)
            {
                return BadRequest("Could not mot mark item as done");
            }

            return RedirectToAction("ItemList", "Todo", new TodoList {Id = itemMarkedDone.ItemListId});
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItemCategory(ItemCategory newCategory)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ItemList");
            }


            var currentUser = await _userManager.GetUserAsync(User);
            var itemList = await _todoListService.GetTodoListById(currentUser, newCategory.ItemListId);
            if (currentUser == null)
            {
                return Challenge();
            }

            var successful = await _todoItemService.AddNewItemCategoryAsync(newCategory, currentUser);

            return RedirectToAction("ItemList", "Todo", itemList);
        }

        //TODO: change name of itemtag
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItemCategory(ItemCategory categoryToBeRemoved)
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


            var successful = await _todoItemService.RemoveItemCategoryAsync(categoryToBeRemoved, currentUser);

            return RedirectToAction("ItemList", "Todo", new TodoList {Id = categoryToBeRemoved.ItemListId});
        }


        public async Task<IActionResult> UndoLastRemovedItem(TodoList list)
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

            var successful = await _todoItemService.UndoLastRemovedItem(currentUser, list);
            return RedirectToAction("ItemList", "Todo", list);
        }
    }
}