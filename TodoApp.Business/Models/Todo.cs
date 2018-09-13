namespace TodoApp.Business.Models
{
    public class Todo
    {
        public string Id { get; }
        public string Description { get; }
        public bool Complete { get; set; }

        public Todo(string id, string description, bool complete)
        {
            Id = id;
            Description = description;
            Complete = complete;
        }

        public void ToggleComplete()
        {
            this.Complete = !this.Complete;
        }
    }
}