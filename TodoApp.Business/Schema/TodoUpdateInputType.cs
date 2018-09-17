using GraphQL.Types;
using TodoApp.Business.Models;
using TodoApp.Business.Services;

namespace TodoApp.Business.Schema
{
    public class TodoUpdateInputType : InputObjectGraphType
    {
        public TodoUpdateInputType()
        {
            Name = "TodoUpdateInput";
            Field<NonNullGraphType<StringGraphType>>("id");
            Field<NonNullGraphType<StringGraphType>>("description");
            Field<NonNullGraphType<BooleanGraphType>>("complete");
        }
    }
}