
namespace SeventySevenDiamondsBackend.Models {

    public class Item {

        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Color> Colors { get; set; } = [];
        public ICollection<Fabric> Fabrics { get; set; } = [];
        public ICollection<Image> Images { get; set; } = [];
        
    }

}
