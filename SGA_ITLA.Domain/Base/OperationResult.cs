namespace SGA_ITLA.Domain.Base
{
    public class OperationResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string? ErrorType { get; set; }   
        public object? Data { get; set; }
    }
}