using System;
using System.Runtime.Serialization;

namespace CoinstantineAPI.Aidrops
{
    [Serializable]
    public class AlreadySubscribedToAirdropException : Exception
    {
        public AlreadySubscribedToAirdropException()
        {
        }

        public AlreadySubscribedToAirdropException(string message) : base(message)
        {
        }

        public AlreadySubscribedToAirdropException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadySubscribedToAirdropException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}