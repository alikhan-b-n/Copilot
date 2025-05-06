namespace Copilot.Admin.Data.Exceptions
{
    public class UnauthorizedHttpException : Exception
    {
        public UnauthorizedHttpException(string message) : base(message)
        {
        }
    }
}