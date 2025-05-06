namespace Copilot.Admin.Data.Exceptions
{
    public class NotFoundHttpException : Exception
    {
        public NotFoundHttpException(string message) : base(message)
        {
        }
    }
}