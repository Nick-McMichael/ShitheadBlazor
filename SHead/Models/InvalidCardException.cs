namespace SHead.Models
{
    public class InvalidCardException : System.Exception
    {
        public InvalidCardException()
        { }

        public InvalidCardException(string message) : base(message)
        { }

        public InvalidCardException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
