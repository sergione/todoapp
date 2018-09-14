using GraphQL.Types;
using TodoApp.Business.Models;
using TodoApp.Business.Services;

namespace TodoApp.Business.Schema
{
    public class TodoCreateInputType : InputObjectGraphType
    {
        public TodoCreateInputType()
        {
            Name = "TodoInput";
            Field<NonNullGraphType<StringGraphType>>("description");
            Field<NonNullGraphType<BooleanGraphType>>("complete");
        }
    }
}