namespace RadencyLibrary.CQRS.Base
{
    public interface IResponseResult<TResult>
    {
        public TResult? Result { get; set; }
    }
}