namespace TestTask.ModelsDTO
{
    public class UpdateToDoDTO
    {
        /// <summary>
        /// The example: 2024-10-10T16:11:00
        /// </summary>
        public DateTime? DateAndTimeOfExpiry { get; set; }
        /// <summary>
        /// Maximum of 20 characters.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Maximum of 100 characters.
        /// </summary>
        public string? Description { get; set; }
    }
}
