namespace GuitarStore.Api.Models
{
    public class Guitar
    {
        public Guitar(Guid uid, string name)
        {
            Uid = uid;
            Name = name;
        }

        public Guid Uid { get; set; }
        public string Name { get; set; }
    }
}
