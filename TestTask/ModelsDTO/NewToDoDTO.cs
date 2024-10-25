namespace TestTask.ModelsDTO
{
    public class NewToDoDTO
    {
        /// <summary>
        /// Change the date to your own. Below example.
        /// </summary>
        /// <example>2024-10-10T16:11:00</example>
        public required DateTime DateAndTimeOfExpiry { get; set; }
        /// <summary>
        /// Maximum of 20 characters.
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// Maximum of 100 characters.
        /// </summary>
        public required string Description { get; set; }
    }
}
