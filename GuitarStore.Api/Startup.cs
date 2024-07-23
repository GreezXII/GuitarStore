using GraphQL;
using GraphQL.Types;
using GuitarStore.Api.Context;
using GuitarStore.Api.GraphQL;
using Microsoft.EntityFrameworkCore;

namespace GuitarStore.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<GuitarsContext>(options => options.UseInMemoryDatabase("MyInMemoryDb"));
            services.AddTransient<GuitarType>();
            services.AddTransient<GuitarQuery>();
            services.AddTransient<GuitarInputType>();
            services.AddTransient<GuitarMutation>();
            services.AddTransient<GuitarSubscription>();
            services.AddSingleton<ISchema, GuitarSchema>();
            services.AddGraphQL(b => b
                .AddAutoSchema<GuitarSchema>()
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionDetails = true));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<GuitarsContext>();
            context!.Database.EnsureCreated();

            app.UseWebSockets();
            app.UseGraphQL();
            if (env.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseGraphQLAltair();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
