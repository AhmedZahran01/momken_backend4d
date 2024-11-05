namespace momken_backend.Dtos.DataRespons
{
    public class GlobalResponseDebugDto<T,J> 
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "Message";
        public T data { get; set; }
        public J debug { get; set; }

    }
    public class GlobalResponseNoBodyDebugDto<T>
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "Message";
        public T debug { get; set; }
    }
}
