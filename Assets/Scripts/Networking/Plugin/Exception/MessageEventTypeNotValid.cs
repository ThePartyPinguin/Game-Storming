namespace GameFrame.Networking.Exception
{
    public sealed class MessageEventTypeNotValid : System.Exception
    {
        public override string Message { get; }

        public MessageEventTypeNotValid(string message)
        {
            Message = message;
        }
    }
}
