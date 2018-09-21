using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using TodoApp.Business.Models;
using TodoApp.Business.Services;

namespace TodoApp.Business.Schema
{
    public class TodosQuery : ObjectGraphType<object>
    {
        public TodosQuery(ITodosService todos)
        {
            Name = "Query";
            Field<ListGraphType<TodoType>>(
                "todos",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "todoId"}, 
                    new QueryArgument<IntGraphType> {Name = "offset"},
                    new QueryArgument<IntGraphType> {Name = "limit"}),
                resolve: context =>
                {
                    var todoId = context.GetArgument<string>("todoId");
                    var offset = context.GetArgument<int>("offset");
                    var limit = context.GetArgument<int>("limit");

                    if (todoId == null)
                    {
                        return todos.GetTodosAsync(offset, limit);
                    }
                    
                    //TODO: clean this up
                    return Task.FromResult(new List<Todo>{todos.GetTodoByIdAsync(todoId).Result}.AsEnumerable());
                });
        }
    }
}