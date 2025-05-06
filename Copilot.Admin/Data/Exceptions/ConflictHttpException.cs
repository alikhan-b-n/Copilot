namespace Copilot.Admin.Data.Exceptions
{
    public class ConflictHttpException : Exception
    {
        public ConflictHttpException(string message) : base(message)
        {
        }
    }
}