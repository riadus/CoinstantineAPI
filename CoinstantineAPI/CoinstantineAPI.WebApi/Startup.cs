using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Aidrops;
using CoinstantineAPI.BitcoinTalkProvider;
using CoinstantineAPI.Blockchain;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Countries;
using CoinstantineAPI.Data;
using CoinstantineAPI.Database;
using CoinstantineAPI.DataProvider.TelegramProvider;
using CoinstantineAPI.DataProvider.TwitterProvider;
using CoinstantineAPI.DiscordBot;
using CoinstantineAPI.Documents;
using CoinstantineAPI.Email;
using CoinstantineAPI.Encryption;
using CoinstantineAPI.Games;
using CoinstantineAPI.Notifications;
using CoinstantineAPI.Scan;
using CoinstantineAPI.Scheduler;
using CoinstantineAPI.Statistics;
using CoinstantineAPI.Translations;
using CoinstantineAPI.Users;
using CoinstantineAPI.VerifyCaptcha;
using CoinstantineAPI.WebApi.Controllers;
using CoinstantineAPI.WebApi.DTO;
using CoinstantineAPI.WebApi.Formatter;
using CoinstantineAPI.WebApi.Mappers;
using CoinstantineAPI.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Azure.KeyVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage;

namespace CoinstantineAPI.WebApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironement;
        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironement = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        private const string AllowLocalhostOrigins = "allowLocalhostOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(x => x.MultipartBodyLengthLimit = 1_074_790_400);
            services.AddApplicationInsightsTelemetry();
            services.AddCors(options =>
            {
                options.AddPolicy(AllowLocalhostOrigins,
                builder =>
                {
                    builder.WithOrigins(Constants.WebsiteUrl)
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var csvFormatterOptions = new CsvFormatterOptions();
			services.AddMvc(options =>
            {
				options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions));
				options.OutputFormatters.Add(new CsvOutputFormatter(csvFormatterOptions));
				options.FormatterMappings.SetMediaTypeMappingForFormat("csv", Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
            });

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
                options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.Jwt)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudience = Constants.AzureClientId,
                        ValidIssuer = Constants.AzureTenant,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            RegisterServices(services);

            ConfigureAutomapper();
        }

        protected void RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<CoinstantineContext>(options =>
            {
                //options.UseSqlite("Data source=Coinstantine.db");
                options.UseSqlServer(Constants.ConnectionDb);
                options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Transient);
            services.AddSingleton<IKeyVaultCrypto>(sp =>
            {
                async Task<string> callback(string authority, string resource, string scope)
                {
                    var appId = Constants.VaultClientResource;
                    var appSecret = Constants.VaultClientCred;
                    var authContext = new AuthenticationContext(authority);

                    var credential = new ClientCredential(appId, appSecret);
                    var authResult = await authContext.AcquireTokenAsync(resource, credential);
                    return authResult.AccessToken;
                }

                var client = new KeyVaultClient(callback);
                return new KeyVaultCrypto(client);
            });

            services.AddAutoMapper(Assembly.GetEntryAssembly());

            var cloudStorageAccount = CloudStorageAccount.Parse(Constants.AzureFilesEndpoint);

            services.AddSingleton(cloudStorageAccount);

            var physicalProvider = _hostingEnvironement.ContentRootFileProvider;
            var embeddedProvider =
                new EmbeddedFileProvider(typeof(DocumentProvider).Assembly);
            var compositeProvider =
                new CompositeFileProvider(physicalProvider, embeddedProvider);
            services.AddSingleton<IFileProvider>(compositeProvider);

            services.AddTransient<IContextProvider, ContextProvider>();
			services.AddSingleton<IMapper<Translation, TranslationDTO>, TranslationMapper>();
            services.AddSingleton<IMapper<IEnumerable<Translation>, TranslationsDTO>, TranslationsMapper>();
            services.AddTransient<IUserResolverService, UserResolverService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddBitoinTalkProvider();
            services.AddCountriesProvider();
            services.AddTelegramProvider();
            services.AddTwitterProvider();
            services.AddAirdropServices();
            services.AddBlockchainServices();
            services.AddDocumentProvider();
            services.AddEncryptionServices();
            services.AddNotifications();
            services.AddScanServices();
            services.AddSchedulerServices();
            services.AddUsersServices();
            services.AddReCaptchaValidation();
            services.AddEmailServices();
            services.AddTranslations();
            services.AddDiscordBot();
            services.AddStatisticsServices();
            services.AddGameServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IUserCreationService userCreationService, IDiscordBot discordBot)
        {
            if (Constants.ApiEnvironment == "Localhost")
            {
                app.UseDeveloperExceptionPage();
            }   
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseCors(AllowLocalhostOrigins);
            app.UseStaticFiles();
            app.UseMvc();

            await userCreationService.CreateAdmin();
            await discordBot.InitializeAsync();
        }

        private static bool _mapperInitialised;
        private void ConfigureAutomapper()
        {
            if(_mapperInitialised)
            {
                return;
            }
            _mapperInitialised = true;

        }
    }
}
