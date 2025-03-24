
namespace SeventySevenDiamondsBackend.Models {
    public class UploadImageRequest {
        public required IFormFile File { get; set; }
        public int ItemId { get; set; }
        public int ColorId { get; set; }
        public int FabricId { get; set; }

    }

}
