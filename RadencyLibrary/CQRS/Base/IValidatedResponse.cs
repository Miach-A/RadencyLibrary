namespace RadencyLibrary.CQRS.Base
{
    public interface IValidatedResponse<TError>
        where TError : class, new()
    {
        public bool Validated { get; set; }
        public ICollection<TError> Errors { get; set; }

    }
}