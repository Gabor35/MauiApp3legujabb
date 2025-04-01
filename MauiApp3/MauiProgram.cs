using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Hosting;
using System;
using System.Net.Http;
using MauiApp3.Services;
using MauiApp3;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<BlazorWebView, BlazorWebViewHandler>();
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<HttpClient>(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://esemenyrendezo1.azurewebsites.net/api/")
            };
            return httpClient;
        });

        builder.Services.AddSingleton<ApiService>();

        return builder.Build();
    }
}
