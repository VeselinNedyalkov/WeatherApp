namespace WheatherApp.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public object Result { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
