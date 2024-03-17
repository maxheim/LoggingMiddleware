using System.Globalization;

namespace TefApiLogger;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _filePath;
    private readonly string _fileName = "RequestLog.txt";

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, string filePath)
    {
        _next = next;
        _filePath = filePath;
    }

    private string CreateLogEntryString(string requestOrigin)
    {
        var currentTime = DateTime.Now.ToString("hh:mm:ss tt");
        var currentUser = GetUserFromRequest();

        return $"Request from: {requestOrigin} at {currentTime} by {currentUser}.\n";
    }

    private string GetUserFromRequest()
    {
        return "1000-112_Max-Heim";
    }

    public async Task Invoke(HttpContext context)
    {
        string source = context.Connection.RemoteIpAddress != null
            ? context.Connection.RemoteIpAddress.ToString()
            : "Unknown Origin";
        
        var logEntry = CreateLogEntryString(source);
        
        File.AppendAllText($"{_filePath}/{_fileName}", logEntry);

        await _next(context);
    }
}