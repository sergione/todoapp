using GraphQL;

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
}