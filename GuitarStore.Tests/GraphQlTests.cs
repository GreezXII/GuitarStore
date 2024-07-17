using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using GuitarStore.Api;
using GuitarStore.Api.Context.Entities;
using System.Diagnostics.CodeAnalysis;

namespace GuitarStore.Tests
{
    [TestClass]
    public class GraphQlTests
    {
        public TestContext? TestContext { get; set; }

        private static GraphQLHttpClient _client = null!;
        private static string _baseAddress = null!;

        [StringSyntax("graphql")]
        private string query = """
            {
                guitars
                {
                    uid
                    name
                }
            }
            """;

        [StringSyntax("graphql")]
        private string mutation = """
            mutation {
                createGuitar(guitar: { name: "Gibson" }) {
                    uid
                    name
                }
            }
            """;

        [TestMethod]
        [ClassInitialize]
        public static void ClassInitMethod(TestContext tx)
        {
            Task.Run(() => Program.Main(null!));
            _baseAddress = "http://localhost:5000";
            _client = new GraphQLHttpClient($"{_baseAddress}/graphql", new SystemTextJsonSerializer());
        }

        [TestMethod]
        public async Task QueryGuitars_Success()
        {
            var guitars = await GetAllGuitars();
            Assert.AreEqual(guitars.Any(), true);
        }

        [TestMethod]
        public async Task CreateGuitar_Success()
        {
            var request = new GraphQLRequest { Query = mutation };
            var response = await _client.SendQueryAsync<Guitar>(request);
            var guitars = await GetAllGuitars();
            Assert.AreEqual(guitars.Count, 4);
        }

        private async Task<List<Guitar>> GetAllGuitars()
        {
            var request = new GraphQLRequest { Query = query };
            var response = await _client.SendQueryAsync(request, () => new { Guitars = new List<Guitar>()});
            return response.Data.Guitars;
        }
    }
}