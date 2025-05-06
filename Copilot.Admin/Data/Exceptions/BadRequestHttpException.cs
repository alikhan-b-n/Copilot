namespace Copilot.Admin.Data.Exceptions
{
    public class BadRequestHttpException : Exception
    {
        public BadRequestHttpException(string message) : base(message)
        {
        }
    }
}