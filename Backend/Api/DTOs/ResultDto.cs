namespace Api.DTOs
{
    public class ResultDto<TResult>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public TResult? Result { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
