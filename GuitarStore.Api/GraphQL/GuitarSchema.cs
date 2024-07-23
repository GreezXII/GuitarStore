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
            Subscription = serviceProvider.GetRequiredService<GuitarSubscription>();
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
            Observables.ObservableGuitar = new ObservableGuitar(newGuitar);
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

    public class GuitarSubscription: ObjectGraphType
    {
        public GuitarSubscription()
        {
            Field<GuitarType, Guitar>("guitarAdded")
                .ResolveStream(ResolveStream);
        }

        private IObservable<Guitar> ResolveStream(IResolveFieldContext<object?> context)
        {
            using var scope = context.RequestServices?.CreateScope() ?? throw new NullReferenceException(); ;
            var dbContext = scope.ServiceProvider.GetRequiredService<GuitarsContext>();
            if (Observables.ObservableGuitar is null)
            {
                Observables.ObservableGuitar = new ObservableGuitar(dbContext.Guitars.FirstOrDefault());
            }
            return Observables.ObservableGuitar;
        }
    }

    public class ObservableGuitar : IObservable<Guitar>
    {
        private readonly List<IObserver<Guitar>> _observers = new();
        private readonly Guitar _guitar = new();

        public ObservableGuitar(Guitar guitar)
        {
            _guitar = guitar;
        }

        public IDisposable Subscribe(IObserver<Guitar> observer)
        {
            _observers.Add(observer);
            observer.OnNext(_guitar);
            return new Unsubscriber<Guitar>(_observers, observer);
        }
    }

    internal sealed class Unsubscriber<Guitar> : IDisposable
    {
        private readonly IList<IObserver<Guitar>> _observers;
        private readonly IObserver<Guitar> _observer;

        internal Unsubscriber(
            IList<IObserver<Guitar>> observers,
            IObserver<Guitar> observer) => (_observers, _observer) = (observers, observer);

        public void Dispose() => _observers.Remove(_observer);
    }

    public static class Observables
    {
        public static ObservableGuitar ObservableGuitar { get; set; }
    }
}
