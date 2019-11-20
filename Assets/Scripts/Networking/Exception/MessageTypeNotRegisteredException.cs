namespace GameFrame.Networking.Exception
{
    public sealed class MessageTypeNotRegisteredException : System.Exception
    {
        public override string Message { get; }

        public MessageTypeNotRegisteredException(string message)
        {
            Message = message;
        }
    }
}
