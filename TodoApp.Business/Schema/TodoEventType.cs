using GraphQL.Types;
using TodoApp.Business.Models;

namespace TodoApp.Business.Schema
{
    public class TodoEventType : ObjectGraphType<TodoEvent>
    {
        public TodoEventType()
        {
            Field(x => x.Id);
            Field(x => x.Description);
            Field(x => x.Complete);
        }
    }
}