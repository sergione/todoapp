namespace TodoApp.Business.Models
{
    public class TodoEvent
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Complete { get; set; }

        public TodoEvent(string id, string description, bool complete)
        {
            Id = id;
            Description = description;
            Complete = complete;
        }
    }
}