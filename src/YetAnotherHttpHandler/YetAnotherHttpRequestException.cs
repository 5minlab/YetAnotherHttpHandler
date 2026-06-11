using System;
using System.Net.Http;

namespace Cysharp.Net.Http
{
    /// <summary>
    /// Represents an HTTP request failure reported by YetAnotherHttpHandler.
    /// </summary>
    public sealed class YetAnotherHttpRequestException : HttpRequestException
    {
        internal YetAnotherHttpRequestException(string message, Exception innerException, uint http2ErrorCode, bool isHttp2GoAway, bool isRemoteHttp2Error, bool isHttp2StreamReset)
            : base(message, innerException)
        {
            Http2ErrorCode = http2ErrorCode;
            IsHttp2GoAway = isHttp2GoAway;
            IsRemoteHttp2Error = isRemoteHttp2Error;
            IsHttp2StreamReset = isHttp2StreamReset;
        }

        /// <summary>
        /// Gets the HTTP/2 error code reported by the native layer. A value of 0 means that no specific HTTP/2 reason code was available.
        /// </summary>
        public uint Http2ErrorCode { get; }

        /// <summary>
        /// Gets a value that indicates whether the failure was caused by an HTTP/2 GOAWAY frame.
        /// </summary>
        public bool IsHttp2GoAway { get; }

        /// <summary>
        /// Gets a value that indicates whether the HTTP/2 failure was reported by the remote peer.
        /// </summary>
        public bool IsRemoteHttp2Error { get; }

        /// <summary>
        /// Gets a value that indicates whether the failure was caused by an HTTP/2 stream reset.
        /// </summary>
        public bool IsHttp2StreamReset { get; }

        /// <summary>
        /// Gets a value that indicates whether the caller should reconnect and retry with a fresh request message.
        /// </summary>
        public bool ShouldReconnect => IsHttp2GoAway;
    }
}
