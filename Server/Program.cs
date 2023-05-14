using Serilog;
using WASMChat.Server.Mappers.Chats;

Host.CreateDefaultBuilder()
    .UseSerilog((ctx, logger) =>
    {
        logger.ReadFrom.Configuration(ctx.Configuration);
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<WASMChat.Server.Startup>();
    })
    .Build()
    .Run();
