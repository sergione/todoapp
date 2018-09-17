using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Business.Models;

namespace TodoApp.Business.Services
{
    public interface ITodosService
    {
        Task<Todo> CreateAsync(Todo todo);
        Task<Todo> GetTodoByIdAsync(string id);
        Task<IEnumerable<Todo>> GetTodosAsync();
        Task<Todo> CompleteAsync(string todoId);
        Task<Todo> UpdateAsync(Todo todo);
    }

    public class TodosService : ITodosService
    {
        private List<Todo> _todos;
        private ITodoEventService _events;

        public TodosService(ITodoEventService events)
        {
            _todos = new List<Todo>();
            _todos.Add(new Todo("1000", "Buy milk", false));
            _todos.Add(new Todo("2000", "Wash car", false));
            _todos.Add(new Todo("3000", "Buy bread", false));
            _todos.Add(new Todo("4000", "Go home", false));
            _events = events;
        }

        public Task<Todo> CreateAsync(Todo todo)
        {
            _todos.Add(todo);
            var todoEvent = new TodoEvent(todo.Id, todo.Description, todo.Complete);
            _events.AddEvent(todoEvent);
            return Task.FromResult(todo);
        }

        public Task<Todo> GetTodoByIdAsync(string id)
        {
            return Task.FromResult(_todos.Single(x => x.Id == id));
        }

        public Task<IEnumerable<Todo>> GetTodosAsync()
        {
            return Task.FromResult(_todos.AsEnumerable());
        }

        public Task<Todo> CompleteAsync(string todoId)
        {
            var todo = _todos.SingleOrDefault(x => x.Id == todoId);

            if (todo == null)
            {
                throw new ArgumentException($"Todo id {todoId} is invalid");
            }
            
            todo.ToggleComplete();
            var todoEvent = new TodoEvent(todo.Id, todo.Description, todo.Complete);
            _events.AddEvent(todoEvent);

            return Task.FromResult(todo);
        }

        public Task<Todo> UpdateAsync(Todo inputTodo)
        {
            var todo = _todos.Single(x => x.Id == inputTodo.Id);
            todo.Description = inputTodo.Description;
            
            return Task.FromResult<Todo>(inputTodo);
        }
    }
}