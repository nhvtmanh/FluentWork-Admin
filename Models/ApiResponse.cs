namespace FluentWork_Admin.Models
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public List<string> Message { get; set; } = new List<string>();
        public T? Data { get; set; }
        public string Error { get; set; } = string.Empty;
    }
    public static class ApiSuccessMessage
    {
        public const string CREATE = "Create successfully!";
        public const string UPDATE = "Update successfully!";
        public const string DELETE = "Delete successfully!";
    }
}
