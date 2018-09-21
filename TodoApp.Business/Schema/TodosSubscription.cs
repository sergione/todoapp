using System;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using TodoApp.Business.Models;
using TodoApp.Business.Services;

namespace TodoApp.Business.Schema
{
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