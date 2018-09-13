using GraphQL.Types;
using TodoApp.Business.Models;
using TodoApp.Business.Services;

namespace TodoApp.Business.Schema
{
    public class TodoType : ObjectGraphType<Todo>
    {
        public TodoType(ITodosService todos)
        {
            Field(x => x.Id);
            Field(x => x.Description);
            Field(x => x.Complete);
        }
    }
}