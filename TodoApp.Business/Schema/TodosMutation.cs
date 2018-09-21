using System;
using GraphQL.Types;
using TodoApp.Business.Models;
using TodoApp.Business.Services;

namespace TodoApp.Business.Schema
{
    public class TodosMutation : ObjectGraphType<object>
    {
        public TodosMutation(ITodosService todos)
        {
            Name = "Mutation";
            Field<TodoType>(
                "createTodo",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<TodoCreateInputType>> {Name = "todo"}),
                resolve: context =>
                {
                    var todoInput = context.GetArgument<TodoCreateInput>("todo");
                    var id = Guid.NewGuid().ToString();
                    var todo = new Todo(id, todoInput.Description, todoInput.Complete);

                    return todos.CreateAsync(todo);
                }
            );
            Field<TodoType>(
                "updateTodo",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<TodoUpdateInputType>> {Name = "todo"}),
                resolve: context =>
                {
                    var todoInput = context.GetArgument<TodoUpdateInput>("todo");
                    var todo = new Todo(todoInput.Id, todoInput.Description, todoInput.Complete);

                    return todos.UpdateAsync(todo);
                }
            );
            FieldAsync<TodoType>(
                "completeTodo",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "todoId" }),
                resolve: async context =>
                {
                    var todoId = context.GetArgument<string>("todoId");
                    return await context.TryAsyncResolve(async c => await todos.CompleteAsync(todoId));
                }
            );
        }
    }
}