using System;
using System.Runtime.Serialization;

namespace Funday.ServiceInterface
{
    [Serializable]
    public class ThreadBusyException : Exception
    {
        public ThreadBusyException()
        {
        }

        public ThreadBusyException(string message) : base(message)
        {
        }

        public ThreadBusyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ThreadBusyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}