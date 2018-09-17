namespace TodoApp.Business.Models
{
    public class TodoUpdateInput
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Complete { get; set; }
    }
}