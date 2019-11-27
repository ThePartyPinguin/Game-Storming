namespace GameFrame.Networking.Exception
{
    public sealed class MessageEventNotRegisteredException : System.Exception
    {
        public override string Message { get; }

        public MessageEventNotRegisteredException(string message)
        {
            Message = message;
        }
    }
}
