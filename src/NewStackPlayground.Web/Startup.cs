using Manisero.CqrsGateway;
using Manisero.DatabaseAccess;
using Manisero.Logger;
using Manisero.Logger.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewStackPlayground.Application;
using NewStackPlayground.Application.Domain;
using NewStackPlayground.Gateway;
using NewStackPlayground.Web.ErrorHandling;
using NewStackPlayground.Web.LogsAndErrors;

namespace NewStackPlayground.Web
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }
        public static AppGateway AppGateway { get; private set; }

        private ILogger _logger;

        public Startup(
            IConfiguration configuration)
        {
            Configuration = configuration;

            var loggerConfig = new LoggerConfig();
            configuration.Bind("Logger", loggerConfig);

            loggerConfig.Db = new LoggerConfig.DbConfig
            {
                ConnectionString = configuration.GetConnectionString("Default"),
                AssembliesWithLogs = new[]
                {
                    typeof(CommandsGateway).Assembly,
                    typeof(IDatabaseAccessor).Assembly,
                    typeof(ILogger).Assembly,
                    typeof(Item).Assembly,
                    typeof(IItemRepository).Assembly,
                    typeof(AppGateway).Assembly,
                    typeof(Startup).Assembly
                }
            };

            LoggerFacade.Configure(loggerConfig);
            _logger = LoggerFacade.GetLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("api", null);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/api/swagger.json", "api"); });
            app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));

            app.UseMvc();

            var connectionString = Configuration.GetConnectionString("Default");
            AppGateway = new AppGateway(connectionString);

            appLifetime.ApplicationStarted.Register(ApplicationStarted);
            appLifetime.ApplicationStopped.Register(ApplicationStopped);
        }

        private void ApplicationStarted()
        {
            _logger.Log(new WebStartedLog());
        }

        private void ApplicationStopped()
        {
            AppGateway.Dispose();

            _logger.Log(new WebStoppedLog());
            LoggerFacade.CloseAndFlush();
        }
    }
}
