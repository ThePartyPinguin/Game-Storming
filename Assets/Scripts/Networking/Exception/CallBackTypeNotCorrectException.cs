namespace GameFrame.Networking.Exception
{
    public sealed class CallBackTypeNotCorrectException : System.Exception
    {
        public override string Message { get; }

        public CallBackTypeNotCorrectException(string message)
        {
            Message = message;
        }
    }
}
