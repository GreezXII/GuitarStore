using GraphQL;
using GraphQL.Types;
using GuitarStore.Api.Context;
using GuitarStore.Api.GraphQL;
using Microsoft.EntityFrameworkCore;

namespace GuitarStore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<GuitarsContext>(options => options.UseInMemoryDatabase("MyInMemoryDb"));
            builder.Services.AddTransient<GuitarType>();
            builder.Services.AddTransient<GuitarQuery>();
            builder.Services.AddTransient<GuitarInputType>();
            builder.Services.AddTransient<GuitarMutation>();
            builder.Services.AddSingleton<ISchema, GuitarSchema>();
            builder.Services.AddGraphQL(b => b
                .AddAutoSchema<GuitarSchema>()
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionDetails = true));

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<GuitarsContext>();
            context!.Database.EnsureCreated();

            app.UseGraphQL();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseGraphQLAltair();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
