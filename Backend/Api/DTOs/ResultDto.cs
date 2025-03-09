namespace Api.DTOs
{
    public class ResultDto<TEntity>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TEntity Entity { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
