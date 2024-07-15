using GraphQL;
using GraphQL.Types;
using GuitarStore.Api.Models;
using GuitarStore.Api.Repositories;

namespace GuitarStore.Api.GraphQL
{
    public class GuitarSchema : Schema
    {
        public GuitarSchema()
        {
            Query = new GuitarQuery();
        }
    }

    public class GuitarQuery : ObjectGraphType
    {
        public GuitarQuery()
        {
            Field<ListGraphType<GuitarType>>("guitars")
                .Argument<StringGraphType>("name")
                .Resolve(ResolveGuitars);
        }

        private IEnumerable<Guitar> ResolveGuitars(IResolveFieldContext<object?> resolver)
        {
            var guitarsRepository = new GuitarsRepository();
            var arg = resolver.GetArgument<string>("name");
            if (arg is not null)
                return guitarsRepository.Guitars.Where(g => g.Name == arg);

            return guitarsRepository.Guitars;
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
}
