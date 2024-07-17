using System.ComponentModel.DataAnnotations;

namespace GuitarStore.Api.Context.Entities
{
    public class Guitar
    {
        [Key]
        public Guid Uid { get; set; }
        public string Name { get; set; } = null!;
    }
}
