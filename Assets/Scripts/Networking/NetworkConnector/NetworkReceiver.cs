using System;
using System.Threading;
using System.Threading.Tasks;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.MessageHandling;

namespace GameFrame.Networking.NetworkConnector
{
    abstract class NetworkReceiver<TEnum> where TEnum : Enum
    {
        protected NetworkMessageDeserializer<TEnum> RegisteredMessageDeserializer;

        private Task _receiverTask;

        private ManualResetEvent _waitUntilTaskFinished;

        private bool _running;
        protected NetworkReceiver()
        {
            _receiverTask = new Task(Receive);

            _receiverTask.GetAwaiter().OnCompleted(ReleaseWaitUntilFinished);

            _waitUntilTaskFinished = new ManualResetEvent(true);
        }

        /// <summary>
        /// If the receiver task hasn't been started yet, start it
        /// </summary>
        public virtual void StartReceiving()
        {
            if(RegisteredMessageDeserializer == null)
                throw new NoMessageHandlerRegisteredException("No message handler had been registered in: " + this.GetType() + " please use RegisterNewMessageHandler before starting");

            if (!_running)
                _receiverTask.Start();
        }

        /// <summary>
        /// if the receiverTask is running stop it, then wait until it has finished
        /// </summary>
        public void StopReceiving()
        {
            if (_running)
            {
                _running = false;
                _waitUntilTaskFinished.WaitOne();
            }
        }

        /// <summary>
        /// This method gets called when the task has stopped receiving and released the manualResetEvent
        /// </summary>
        private void ReleaseWaitUntilFinished()
        {
            _waitUntilTaskFinished.Reset();
        }

        /// <summary>
        /// The method the receiver task continuously runs when it has started
        /// </summary>
        private async void Receive()
        {
            _waitUntilTaskFinished.Set();
            while (_running)
            {
                byte[] data = ReceiveData();

                if (data != null && data.Length > 0)
                {
                    HandleData(data);
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }

        /// <summary>
        /// Overridable method that handled the data reading from the incoming dataStream, depending on Tcp or Udp
        /// </summary>
        /// <returns></returns>
        protected abstract byte[] ReceiveData();

        /// <summary>
        /// Register a new Messagehandler that handles the incoming messages
        /// </summary>
        /// <param name="messageDeserializer"></param>
        public void RegisterNewMessageHandler(NetworkMessageDeserializer<TEnum> messageDeserializer)
        {
            RegisteredMessageDeserializer = messageDeserializer;
        }

        /// <summary>
        /// Handle the incomming data, and add it to the queue in the registered messagehandler
        /// </summary>
        /// <param name="data"></param>
        protected void HandleData(byte[] data)
        {
            if(RegisteredMessageDeserializer == null)
                throw new NoMessageHandlerRegisteredException("No message handler had been registered in: " + this.GetType());

            RegisteredMessageDeserializer.AddMessageToQueue(data);
        }
    }
}
