namespace JetflixApp.Models
{
    public class FilmModel
    {
        public int Id { get; set; }
        public string? FilmAdi { get; set; }
        public string? Yonetmen { get; set; }
        public string? ImgUrl { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime Tarih { get; set; }
        public string? Detay { get; set; }
        public int KategoriId { get; set; }
        public string? Kategori { get; set; }
    }
    //public class KategoriModel
    //{
    //    public int Id { get; set; }
    //    public string? KategoriAdi { get; set; }
    //    public List<FilmModel> Filmler { get; set; }
    //}
}
