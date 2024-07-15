using GraphQL;
using GraphQL.Types;
using GuitarStore.Api.GraphQL;

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
            builder.Services.AddTransient<GuitarType>();
            builder.Services.AddTransient<GuitarQuery>();
            builder.Services.AddSingleton<ISchema, GuitarSchema>();
            builder.Services.AddGraphQL(b => b
                .AddAutoSchema<GuitarSchema>()
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionDetails = true));

            var app = builder.Build();
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
