
namespace SeventySevenDiamondsBackend.Models {

    public class Fabric {

        public int Id { get; set; }
        public required string Name { get; set; }
        public int ItemId { get; set; }
        public virtual required Item Item { get; set; }

    }

}
