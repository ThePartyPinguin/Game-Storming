namespace GameFrame.Networking.Exception
{
    public sealed class InvalidIpAddressException : System.Exception
    {
        public override string Message { get; }

        public InvalidIpAddressException(string message)
        {
            Message = message;
        }
    }
}
