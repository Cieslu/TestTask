namespace TestTask.ModelsDTO
{
    public class ToDoDTO
    {
        public required Guid Id { get; set; }
        public required DateTime DateAndTimeOfExpiry { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Complete { get; set; }
    }
}
