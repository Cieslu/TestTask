namespace TestTask.Models
{
    public class ToDo
    {
        public Guid Id { get; set; }
        public DateTime DateAndTimeOfExpiry { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int Complete { get; set; }
    }
}
