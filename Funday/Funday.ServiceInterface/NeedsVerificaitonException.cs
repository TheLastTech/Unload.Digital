using Funday.ServiceModel.StockXAccount;
using System;
using System.Runtime.Serialization;

namespace Funday.ServiceInterface
{
    [Serializable]
    internal class NeedsVerificaitonException : Exception
    {
        public NeedsVerificaitonException( StockXAccount login)
        {
            Login = login;
        }

        public NeedsVerificaitonException(string message) : base(message)
        {
        }

        public NeedsVerificaitonException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NeedsVerificaitonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public StockXAccount Login { get; }
    }
}