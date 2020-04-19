namespace AssistanceRequestApp.Web
{
    using AssistanceRequestApp.DL.Context;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="Startup" />.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddMvcCore();
            services.AddDbContext<AssistanceRequestAppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("connectionString")));
            services.AddTransient<DL.Interface.IRequestDLRepository, DL.RequestDLRepository>();
            services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        /// <param name="loggerFactory">The loggerFactory<see cref="ILoggerFactory"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/myapp-{Date}.txt");
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AssistanceRequestAppDBContext>();
                context.Database.EnsureCreated();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //How to disable browser cache in ASP.NET core rc2
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache")
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
