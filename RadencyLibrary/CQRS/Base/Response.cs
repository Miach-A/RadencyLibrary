namespace RadencyLibrary.CQRS.Base
{
    public class Response<TResult, TError> : IValidatedResponse<TError>
        where TError : class, new()
    {
        public Response() { }
        public bool Validated { get; set; } = true;
        public TResult? Result { get; set; }
        public ICollection<TError> Errors { get; set; } = new List<TError>();
    }
}
