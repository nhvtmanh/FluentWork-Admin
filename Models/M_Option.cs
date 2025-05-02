namespace FluentWork_Admin.Models
{
    public class M_Option
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}
