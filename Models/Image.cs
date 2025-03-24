
using System.Text.Json.Serialization;
using SeventySevenDiamondsBackend.Models;

namespace SeventySevenDiamondsBackend {

    public class Image 
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ColorId { get; set; }
        public int FabricId { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }

        [JsonIgnore]
        public virtual Item Item { get; set; } = null!;
    }

}
