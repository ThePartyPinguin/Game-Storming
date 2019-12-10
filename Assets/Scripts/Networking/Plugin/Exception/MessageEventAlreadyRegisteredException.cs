namespace GameFrame.Networking.Exception
{
    public sealed class MessageEventAlreadyRegisteredException : System.Exception
    {
        public override string Message { get; }

        public MessageEventAlreadyRegisteredException(string message)
        {
            Message = message;
        }
    }
}
