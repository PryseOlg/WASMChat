using System.Net.Mime;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WASMChat.Data.Entities;
using WASMChat.Data;
using WASMChat.Data.Repositories;
using WASMChat.Server.Hubs;
using WASMChat.Server.Mappers;
using WASMChat.Server.Middlewares;
using WASMChat.Server.Options;
using WASMChat.Server.Pipelines;
using WASMChat.Server.Services;
using WASMChat.Server.Swagger;
using WASMChat.Server.Validators;

namespace WASMChat.Server;

public class Startup
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddErrorHandling();

        var connectionString = _config.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddRepositories<ApplicationDbContext>();
        
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<ApplicationUser>(options => 
                options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddDataProtection()
            .PersistKeysToDbContext<ApplicationDbContext>();

        services.AddAuthentication()
            .AddCustomOAuth(_config.GetSection("Authentication"))
            .AddIdentityServerJwt();
        services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        services.AddControllersWithViews()
            .AddJsonOptions(options => 
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping);
        
        services.AddRazorPages();

        services.AddServices();
        services.AddValidators();
        services.AddMappers();
        
        services.AddSignalR();

        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Append(MediaTypeNames.Application.Octet);
        });

        services.AddMediatR(mediatr =>
        {
            mediatr.RegisterServicesFromAssemblyContaining<Startup>();
        });
        
        services.AddScoped(typeof(LoggingPipelineBehaviour<,>));

        services.AddNgrok();

        services.AddSwaggerGen(ConfigureSwaggerGen); // Добавляет сервисы для сваггера
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseResponseCompression();
        
        if (_config.NgrokEnabled())
        {
            app.UseNgrok();
        }
        
        app.UseErrorHandling();

        if (_env.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();
    
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WasmChat Api v1");
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseAuthentication();

        app.UseEndpoints(e =>
        {
            e.MapHub<ChatHub>("api/hubs/chats");
            e.MapRazorPages();
            e.MapControllers();
            e.MapFallbackToFile("index.html");
        });
    }

    /// <summary>
    /// Configures default swagger gen to enable jwt auth.
    /// </summary>
    /// <param name="options"></param>
    private static void ConfigureSwaggerGen(SwaggerGenOptions options)
    {
        options.OperationFilter<SwaggerRequiredOperationFilter>();
        options.SchemaFilter<SwaggerRequiredSchemaFilter>();
        
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "WASMChat API",
            Version = "v1"
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement 
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        options.DescribeAllParametersInCamelCase();
    }
}