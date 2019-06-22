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
        private readonly ITodoListService _todoListService;
        private readonly UserManager<IdentityUser> _userManager;


        public TodoController(ITodoItemService todoItemService, ITodoListService todoListService, UserManager<IdentityUser> userManager)
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
        //Returns a specific list with to-do items
        public async Task<IActionResult> ItemList(TodoList list)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            
            
            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser, list);
            var categories = await _todoItemService.GetExistingItemCategoriesAsync(currentUser, list);
            var itemList = await _todoListService.GetTodoListById(currentUser, list.Id);

            
            ViewBag.listofTags = categories; //Used for select list
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
        public async Task<IActionResult> AddItem(TodoItem newItem,  ItemTag itemTag)
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

            var successful = await _todoItemService.AddItemAsync(newItem, itemTag, currentUser);
            if (!successful)
            {
                return BadRequest("Could not add item.");
            }
            //TODO: find a better solution
            return RedirectToAction("ItemList", "Todo", new TodoList{Id = newItem.ItemListId});


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
            var itemList = await _todoListService.GetTodoListById(currentUser, item.ItemListId);
            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);


            if (!successful)
            {
                return BadRequest("Could not mot mark item as done");
            }

            return RedirectToAction("ItemList", "Todo", itemList);
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItemCategory(ItemTag newTag)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ItemList");
            }
            

            var currentUser = await _userManager.GetUserAsync(User);
            var itemList = await _todoListService.GetTodoListById(currentUser, newTag.ItemListId);
            if (currentUser == null)
            {
                return Challenge();
            }
            var successful = await _todoItemService.AddNewItemCategoryAsync(newTag, currentUser);

            return RedirectToAction("ItemList", "Todo", itemList);
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItemCategory(ItemTag newTag)
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
            var itemList = await _todoListService.GetTodoListById(currentUser, newTag.ItemListId);

            var successful = await _todoItemService.RemoveItemCategoryAsync(newTag, currentUser);


            return RedirectToAction("ItemList", "Todo", itemList);
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
            //var list = await _todoListService.GetTodoListById(currentUser, list.ItemListId);
            var successful = await _todoItemService.UndoLastRemovedItem(currentUser, list);
            return RedirectToAction("ItemList", "Todo", list);
        }
    }
}

