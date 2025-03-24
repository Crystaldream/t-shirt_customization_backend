
namespace SeventySevenDiamondsBackend.Models {

    public class Color {

        public int Id { get; set; }
        public required string Name { get; set; }
        public int ItemId { get; set; }
        public virtual required Item Item { get; set; }

    }

}
