using GuitarStore.Api.Models;

namespace GuitarStore.Api.Repositories
{
    public class GuitarsRepository
    {
        public List<Guitar> Guitars { get; set; }

        public GuitarsRepository()
        {
            Guitars = 
            [
                new Guitar(Guid.Parse("3D423AAA-68E9-4A2F-A5BC-F20FDCC4FE3C"), "Ibanez"),
                new Guitar(Guid.Parse("3D423AAA-68E9-4A2F-A5BC-F21FDCC4FE3C"), "Schecter"),
                new Guitar(Guid.Parse("3D423AAA-68E9-4A2F-A5BC-F22FDCC4FE3C"), "Fender")
            ];
        }
    }
}
