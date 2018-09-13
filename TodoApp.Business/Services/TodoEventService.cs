using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using TodoApp.Business.Models;

namespace TodoApp.Business.Services
{
    public interface ITodoEventService
    {
        ConcurrentStack<TodoEvent> AllEvents { get; }
        void AddError(Exception ex);
        TodoEvent AddEvent(TodoEvent todoEvent);
        IObservable<TodoEvent> EventStream();
    }

    public class TodoEventService : ITodoEventService
    {
        private readonly ISubject<TodoEvent> _eventStream = new ReplaySubject<TodoEvent>(1);
        
        public ConcurrentStack<TodoEvent> AllEvents { get; }
        
        public TodoEventService()
        {
            AllEvents = new ConcurrentStack<TodoEvent>();
        }

        public void AddError(Exception ex)
        {
            _eventStream.OnError(ex);
        }

        public TodoEvent AddEvent(TodoEvent todoEvent)
        {
            AllEvents.Push(todoEvent);
            _eventStream.OnNext(todoEvent);
            return todoEvent;
        }

        public IObservable<TodoEvent> EventStream()
        {
            return _eventStream.AsObservable();
        }
    }
}