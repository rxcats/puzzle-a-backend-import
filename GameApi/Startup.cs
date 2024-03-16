using GameApi.Extensions;
using GameApi.Filter;
using GameApi.Options;
using GameApi.Provider;
using GameApi.Service;
using GameRedis.Session;
using GameRepository;
using GameRepository.Repos;
using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using ZLogger;

namespace GameApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private ILogger<Startup> _logger;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddMvcOptions(options =>
            {
                options.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
                options.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
                options.Filters.Add(typeof(GlobalServiceExceptionFilter));
            });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "GameApi", Version = "v1"}); });

            DatabaseConnectionOptions databaseConnectionOptions = new();
            _configuration.GetSection(nameof(DatabaseConnectionOptions))
                .Bind(databaseConnectionOptions);

            services.AddPooledDbContextFactory<GameDatabaseContext>(options =>
            {
                var connectionString = $@"Server={databaseConnectionOptions.Server};
                    Port={databaseConnectionOptions.Port};
                    Uid={databaseConnectionOptions.User};
                    Pwd={databaseConnectionOptions.Password};
                    Database={databaseConnectionOptions.Database};";

                options
                    .UseMySql(connectionString, new MySqlServerVersion(databaseConnectionOptions.Version))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .LogTo(_logger.ZLogInformation);
            });

            RedisConnectionOptions redisConnectionOptions = new();
            _configuration.GetSection(nameof(RedisConnectionOptions))
                .Bind(redisConnectionOptions);

            services.AddSingleton(_ =>
            {
                var connectionString = $@"{redisConnectionOptions.Server}:{redisConnectionOptions.Port},
                    password={redisConnectionOptions.Password}";
                var connect = ConnectionMultiplexer.Connect(connectionString);
                return connect.GetDatabase();
            });

            services.AddScoped<ValidateAccessTokenAttribute>();

            services.AddSingleton<ISessionProvider, SessionProvider>();

            services.AddSingleton<IUserInfoRepository, UserInfoRepository>();

            services.AddSingleton<IScoreBoardRepository, ScoreBoardRepository>();

            services.AddSingleton<AuthService>();

            services.AddSingleton<LeaderBoardService>();

            services.AddSingleton<FirebaseProvider>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameApi v1"));
            }

            app.UseStatusCodePages();

            app.UseJsonApiLogging();
            app.UseMessagePackApiLogging();

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}