using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.MessageHandling;
using System;
using System.Threading;

namespace GameFrame.Networking.NetworkConnector
{
    abstract class NetworkReceiver<TEnum> where TEnum : Enum
    {
        protected NetworkMessageDeserializer<TEnum> MessageDeserializer;

        private readonly Thread _receiverThread;

        private readonly ManualResetEvent _waitUntilTaskFinished;

        private bool _running;

        private bool _setupComplete;

        protected NetworkReceiver(NetworkMessageDeserializer<TEnum> messageDeserializer)
        {
            MessageDeserializer = messageDeserializer;
            _receiverThread = new Thread(Receive);

            _waitUntilTaskFinished = new ManualResetEvent(false);
            
            //_receiverThread.GetAwaiter().OnCompleted(ReleaseWaitUntilFinished);

            _waitUntilTaskFinished = new ManualResetEvent(true);
        }

        /// <summary>
        /// If the receiver task hasn't been started yet, start it
        /// </summary>
        public virtual void StartReceiving()
        {
            if(MessageDeserializer == null)
                throw new NoMessageHandlerRegisteredException("No message handler had been registered in: " + this.GetType() + " please use RegisterNewMessageHandler before starting");

            if (!_setupComplete)
                Setup();

            try
            {
                if (!_running)
                {
                    _waitUntilTaskFinished.Reset();
                    _running = true;
                    _receiverThread.Start();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        protected abstract void Stop();

        /// <summary>
        /// if the receiverTask is running stop it, then wait until it has finished
        /// </summary>
        public void StopReceiving()
        {
            if (_running)
            {
                _running = false;
                Stop();
                _waitUntilTaskFinished.WaitOne(500);
            }
        }

        /// <summary>
        /// This method gets called when the task has stopped receiving and released the manualResetEvent
        /// </summary>
        private void ReleaseWaitUntilFinished()
        {
            _waitUntilTaskFinished.Set();
        }

        /// <summary>
        /// The method the receiver task continuously runs when it has started
        /// </summary>
        private void Receive()
        {
            while (_running)
            {
                byte[] data = ReceiveData();

                if (data != null && data.Length > 0)
                {

                    HandleData(data);
                }
            }

            ReleaseWaitUntilFinished();
        }

        /// <summary>
        /// Overridable method that handled the data reading from the incoming dataStream, depending on Tcp or Udp
        /// </summary>
        /// <returns></returns>
        protected abstract byte[] ReceiveData();


        /// <summary>
        /// Handle the incomming data, and add it to the queue in the registered messagehandler
        /// </summary>
        /// <param name="data"></param>
        protected void HandleData(byte[] data)
        {
            if(MessageDeserializer == null)
                throw new NoMessageHandlerRegisteredException("No message handler had been registered in: " + this.GetType());


            MessageDeserializer.AddMessageToQueue(data);
        }

        protected virtual void Setup()
        {
            _setupComplete = false;
        }
    }
}
