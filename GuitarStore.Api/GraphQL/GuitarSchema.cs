using GraphQL;
using GraphQL.Types;
using GuitarStore.Api.Context;
using GuitarStore.Api.Context.Entities;

namespace GuitarStore.Api.GraphQL
{
    public class GuitarSchema : Schema
    {
        public GuitarSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<GuitarQuery>();
            Mutation = serviceProvider.GetRequiredService<GuitarMutation>();
        }
    }

    public class GuitarQuery : ObjectGraphType
    {
        public GuitarQuery(IServiceProvider serviceProvider)
        {
            Field<ListGraphType<GuitarType>>("guitars")
                .Argument<StringGraphType>("name")
                .Resolve(ResolveGuitars);
        }

        private List<Guitar> ResolveGuitars(IResolveFieldContext<object?> context)
        {
            var scope = context.RequestServices?.CreateScope() ?? throw new NullReferenceException();
            var dbContext = scope.ServiceProvider.GetRequiredService<GuitarsContext>() ?? throw new ArgumentNullException(nameof(GuitarsContext));
            
            var arg = context.GetArgument<string>("name");
            if (arg is not null)
                return dbContext.Guitars.Where(g => g.Name == arg).ToList();
            
            return dbContext.Guitars.ToList();
        }
    }

    public class GuitarType : ObjectGraphType<Guitar>
    {
        public GuitarType()
        {
            Field(g => g.Uid);
            Field(g => g.Name);
        }
    }

    public class GuitarMutation : ObjectGraphType
    {
        public GuitarMutation(IServiceProvider serviceProvider)
        {
            Field<GuitarType>("createGuitar")
                .Argument<GuitarInputType>("guitar")
                .Resolve(CreateGuitar);
        }

        private Guitar CreateGuitar(IResolveFieldContext<object?> context)
        {
            using var scope = context.RequestServices?.CreateScope() ?? throw new NullReferenceException();
            var dbContext = scope.ServiceProvider.GetService<GuitarsContext>() ?? throw new NullReferenceException(nameof(GuitarsContext));
            var arg = context.GetArgument<GuitarInputType>("guitar");
            var newGuitar = new Guitar() { Uid = Guid.NewGuid(), Name = arg.Name };
            dbContext.Guitars.Add(newGuitar);
            dbContext.SaveChanges();            
            return newGuitar;
        }
    }

    public class GuitarInputType : InputObjectGraphType
    {
        public GuitarInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("name");
        }
    }
}
