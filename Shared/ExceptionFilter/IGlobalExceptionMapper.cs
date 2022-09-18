namespace Shared.ExceptionFilter
{
    public interface IGlobalExceptionMapper
    {
        Exception Map(Exception exception);
    }
}