using HomeEnergyApi.Middleware;
using HomeEnergyApi.Services;
using Microsoft.AspNetCore.Http;
using System.Data.Entity.Core.Metadata.Edm;
using System.Net;
using System.Text;

public class RateLimitingMiddlewareTest
{
    private readonly StubLogger<RateLimitingMiddleware> _stubLogger;
    private readonly DefaultHttpContext _expectedHttpContext;
    private readonly StubRateLimitingService _underRateLimitService;
    private readonly StubRateLimitingService _overRateLimitService;
    private HttpContext? _actualContext = null;
    private RequestDelegate _stubRequestDelegate;

    public RateLimitingMiddlewareTest()
    {
        _stubLogger = new StubLogger<RateLimitingMiddleware>();
        _expectedHttpContext = new DefaultHttpContext();
        _expectedHttpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        _expectedHttpContext.Response.Body = new MemoryStream();
        _underRateLimitService = new StubRateLimitingService(true);
        _overRateLimitService = new StubRateLimitingService(false);
        _stubRequestDelegate = async (HttpContext context) => _actualContext = context;
        
    }

    [Fact]
    public async Task ShouldCallNextMiddleware_WhenRateLimitNotExceeded()
    {
        var middleware = new RateLimitingMiddleware(_stubRequestDelegate, _underRateLimitService, _stubLogger);

        await middleware.InvokeAsync(_expectedHttpContext);
        _expectedHttpContext.Response.Body.Seek(0, SeekOrigin.Begin);

        var responseBody = await new StreamReader(_expectedHttpContext.Response.Body, Encoding.UTF8).ReadToEndAsync();

        Assert.Equal("", responseBody);
        Assert.Same(_actualContext, _expectedHttpContext);
        Assert.Equal(StatusCodes.Status200OK, _expectedHttpContext.Response.StatusCode);
        Assert.Equal(2, _stubLogger.LoggedDebugMessages.Count);
        Assert.Equal("Starting middleware", _stubLogger.LoggedDebugMessages[0]);
        Assert.Equal("Calling next middleware", _stubLogger.LoggedDebugMessages[1]);
    }

    [Fact]
    public async Task ShouldReturn429_WhenRateLimitExceeded()
    {
        var middleware = new RateLimitingMiddleware(_stubRequestDelegate, _overRateLimitService, _stubLogger);

        await middleware.InvokeAsync(_expectedHttpContext);
        _expectedHttpContext.Response.Body.Seek(0, SeekOrigin.Begin);

        var responseBody = await new StreamReader(_expectedHttpContext.Response.Body, Encoding.UTF8).ReadToEndAsync();

        Assert.Equal("Slow down! Too many requests.", responseBody);
        Assert.Null(_actualContext);
        Assert.Equal(StatusCodes.Status429TooManyRequests, _expectedHttpContext.Response.StatusCode);
        Assert.Single(_stubLogger.LoggedDebugMessages);
        Assert.Equal("Starting middleware", _stubLogger.LoggedDebugMessages[0]);
    }

}