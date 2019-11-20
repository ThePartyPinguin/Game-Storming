namespace GameFrame.Networking.Exception
{
    public sealed class MessageTypeAlreadyRegisteredException : System.Exception
    {
        public override string Message { get; }

        public MessageTypeAlreadyRegisteredException(string message)
        {
            Message = message;
        }
    }
}
