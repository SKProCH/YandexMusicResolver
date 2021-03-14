using System;
using System.Runtime.Serialization;

namespace YandexMusicResolver
{
    /// <summary>
    /// Represents errors that occurs in YandexMusicResolver
    /// For more info see InnerException
    /// </summary>
    [Serializable]
    public class YandexMusicException : Exception
    {
        /// <inheritdoc />
        public YandexMusicException()
        {
        }

        /// <inheritdoc />
        public YandexMusicException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public YandexMusicException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <inheritdoc />
        protected YandexMusicException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}