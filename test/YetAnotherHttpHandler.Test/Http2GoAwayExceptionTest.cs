using System.IO;
using System.Net.Http;
using Cysharp.Net.Http;

namespace _YetAnotherHttpHandler.Test;

public class Http2GoAwayExceptionTest
{
    [Fact]
    public void CreateRequestException_GoAway_ReturnsReconnectableException()
    {
        var exception = ResponseContext.CreateRequestException("connection error received: NO_ERROR", 0, Http2ErrorFlags.GoAway | Http2ErrorFlags.Remote);

        var requestException = Assert.IsType<YetAnotherHttpRequestException>(exception);
        Assert.True(requestException.IsHttp2GoAway);
        Assert.True(requestException.IsRemoteHttp2Error);
        Assert.False(requestException.IsHttp2StreamReset);
        Assert.True(requestException.ShouldReconnect);
        Assert.Equal<uint>(0, requestException.Http2ErrorCode);
        var innerException = Assert.IsType<HttpRequestException>(requestException.InnerException);
        Assert.IsType<IOException>(innerException.InnerException);
    }

    [Fact]
    public void CreateRequestException_ResetStream_PreservesHttp2Code()
    {
        var exception = ResponseContext.CreateRequestException("stream error received: REFUSED_STREAM", 0x7, Http2ErrorFlags.ResetStream | Http2ErrorFlags.Remote);

        var requestException = Assert.IsType<YetAnotherHttpRequestException>(exception);
        Assert.False(requestException.IsHttp2GoAway);
        Assert.True(requestException.IsRemoteHttp2Error);
        Assert.True(requestException.IsHttp2StreamReset);
        Assert.False(requestException.ShouldReconnect);
        Assert.Equal<uint>(0x7, requestException.Http2ErrorCode);
    }

    [Fact]
    public void CreateRequestException_WithoutHttp2Metadata_ReturnsIoException()
    {
        var exception = ResponseContext.CreateRequestException("transport failed", 0, Http2ErrorFlags.None);

        var ioException = Assert.IsType<IOException>(exception);
        Assert.Equal("transport failed", ioException.Message);
    }
}
