using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using TodoApp.Business.Models;
using TodoApp.Business.Services;

namespace TodoApp.Business.Schema
{
    public class TodosSchema : GraphQL.Types.Schema
    {
        public TodosSchema(TodosQuery query, TodosMutation mutation, TodosSubscription subscription,
            IDependencyResolver resolver)
        {
            Query = query;
            Mutation = mutation;
            Subscription = subscription;
            DependencyResolver = resolver;
        }
    }

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
    
    public class TodosSubscription : ObjectGraphType<object>
    {
        private readonly ITodoEventService _events;

        public TodosSubscription(ITodoEventService events)
        {
            _events = events;
            Name = "Subscription";
            AddField(new EventStreamFieldType
            {
                Name = "todoEvent",
                Type = typeof(TodoEventType),
                Resolver = new FuncFieldResolver<TodoEvent>(ResolveEvent),
                Subscriber = new EventStreamResolver<TodoEvent>(Subscribe)
            });
        }

        private TodoEvent ResolveEvent(ResolveFieldContext context)
        {
            var todoEvent = context.Source as TodoEvent;
            return todoEvent;
        }

        private IObservable<TodoEvent> Subscribe(ResolveEventStreamContext context)
        {
            return _events.EventStream();
        }
    }
}