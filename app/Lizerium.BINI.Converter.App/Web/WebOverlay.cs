/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 24 апреля 2026 15:29:41
 * Version: 1.0.
 */

using System.Text;
using Lizerium.BINI.Converter.App.Conversion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Lizerium.BINI.Converter.App.Web;

internal static class WebOverlay
{
    /// <summary>
    /// Starts the minimal local ASP.NET host that serves the drag-and-drop converter UI.
    /// </summary>
    public static async Task<int> RunAsync(WebOptions options)
    {
        var webRoot = Path.Combine(AppContext.BaseDirectory, "wwwroot");
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            WebRootPath = webRoot
        });

        builder.WebHost.UseUrls(options.Urls);

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapGet("/", static () => Results.Redirect("/index.html"));
        app.MapPost("/api/convert", ConvertFileAsync);

        Console.WriteLine($"Web converter: {options.Urls}");
        Console.WriteLine($"Web root: {webRoot}");
        await app.RunAsync();
        return 0;
    }

    private static async Task<IResult> ConvertFileAsync(HttpRequest request, string mode = "auto", string encoding = "cp1251")
    {
        // The browser sends one file and receives base64 so both text and binary outputs share one JSON response shape.
        var form = await request.ReadFormAsync();
        var file = form.Files["file"];

        if (file is null)
        {
            return Results.BadRequest(new { error = "File field is missing." });
        }

        if (file.Length == 0)
        {
            return Results.BadRequest(new { error = "File is empty." });
        }

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var direction = ConvertDirectionParser.Parse(mode);
        var textEncoding = TextEncodingResolver.Parse(encoding);
        var result = ConversionEngine.Convert(memory.ToArray(), direction, textEncoding);
        var outputName = BuildOutputName(file.FileName, result.Direction);
        var isText = result.Direction == ConvertDirection.ToText;

        return Results.Json(new
        {
            fileName = outputName,
            direction = result.Direction.ToString(),
            isText,
            base64 = Convert.ToBase64String(result.Bytes),
            preview = isText ? textEncoding.GetString(result.Bytes) : null
        });
    }

    private static string BuildOutputName(string inputName, ConvertDirection direction)
    {
        var name = Path.GetFileNameWithoutExtension(inputName);
        var extension = Path.GetExtension(inputName);
        var suffix = direction == ConvertDirection.ToText ? ".text" : ".bini";

        return name + suffix + extension;
    }
}
