namespace BoxOffice.BLL.Exceptions
{
    public class NotFoundException(string message) : Exception(message);
    public class ValidationException(string message) : Exception(message);

    public class BusinessException(string message) : Exception(message);
}
