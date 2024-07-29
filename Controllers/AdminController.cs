using Dapper;
using JetflixApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static System.Reflection.Metadata.BlobBuilder;

namespace JetflixApp.Controllers
{
    public class AdminController : Controller
    {
       

        public IActionResult Index()
        {
            using var connection = new SqlConnection(connectionString);
            var menus = connection.Query<FilmModel>("SELECT Filmler.*, Kategori FROM Filmler LEFT JOIN Kategoriler ON Filmler.KategoriId = Kategoriler.Id ORDER BY Kategoriler.Kategori ASC").ToList();
            return View(menus);
        }
        public IActionResult FilmEkle()
        {
            using var connection = new SqlConnection(connectionString);
            var category = connection.Query<FilmModel>("SELECT * FROM Kategoriler").ToList();
            return View(category);
        }
        [HttpPost]
        public IActionResult FilmEkle(FilmModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MessageCssClass = "alert-danger";
                ViewBag.Message = "Eksik veya hatalı işlem yaptın";
                return View("Message");
            }
            using var connection = new SqlConnection(connectionString);
            var menus = "INSERT INTO Filmler (FilmAdi, Yonetmen, ImgUrl, Detay, Tarih, KategoriId ) VALUES (@FilmAdi, @Yonetmen, @ImgUrl, @Detay, @Tarih, @kategoriId)";

            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", imageName);
            using var stream = new FileStream(path, FileMode.Create);
            model.Image.CopyTo(stream);
            model.ImgUrl = imageName;
            var data = new
            {
                model.FilmAdi,
                model.Yonetmen,
                model.Detay,
                model.ImgUrl,
                model.Tarih,
                model.KategoriId,
            };

            var rowsAffected = connection.Execute(menus, data);
            ViewBag.MessageCssClass = "alert-success";
            ViewBag.Message = "Film başarıyla eklendi.";
            return View("Message");
        }
        public IActionResult Kategori()
        {
            using var connection = new SqlConnection(connectionString);
            var category = connection.Query<FilmModel>("SELECT * FROM Kategoriler").ToList();

            return View(category);
        }
        public IActionResult KategoriDuzenle(int id)
        {
            using var connection = new SqlConnection(connectionString);
            var menus = connection.QuerySingleOrDefault<FilmModel>("SELECT * FROM Kategoriler WHERE Id = @Id", new { Id = id });
            return View(menus);
        }
        [HttpPost]
        public IActionResult KategoriDuzenle(FilmModel model)
        {
            using var connection = new SqlConnection(connectionString);

            var sql = "UPDATE Kategoriler SET Kategori = @Kategori WHERE Id=@Id";

            var parameters = new
            {
                model.Kategori,
                model.Id,
            };

            var affectedRows = connection.Execute(sql, parameters);
            ViewBag.Message = "Güncellendi.";
            ViewBag.MessageCssClass = "alert-success";
            return View("Message");
        }
        public IActionResult KategoriSil(int id)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = "DELETE FROM Kategoriler WHERE Id = @Id";

            var rowsAffected = connection.Execute(sql, new { Id = id });

            return RedirectToAction("Kategori");
        }
        public IActionResult FilmSil(int id)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = "DELETE FROM Filmler WHERE Id = @Id";

            var rowsAffected = connection.Execute(sql, new { Id = id });

            return RedirectToAction("Index");
        }
        public IActionResult FilmDuzenle(int id)
        {
            using var connection = new SqlConnection(connectionString);
            var menus = connection.QuerySingleOrDefault<FilmModel>("SELECT Filmler.*, Kategoriler.Kategori FROM Filmler LEFT JOIN Kategoriler ON Filmler.KategoriId = Kategoriler.Id WHERE Filmler.Id = @id", new { id = id });
            var category = connection.Query<FilmModel>("SELECT * FROM Kategoriler").ToList();
            ViewBag.Categories = category;

            return View(menus);
        }
        [HttpPost]
        public IActionResult FilmDuzenle(FilmModel model)
        {
            using var connection = new SqlConnection(connectionString);
            var imageName = model.ImgUrl;
            if (model.Image != null)
            {
                imageName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", imageName);
                using var stream = new FileStream(path, FileMode.Create);
                model.Image.CopyTo(stream);
            }
            var sql = "UPDATE Filmler SET FilmAdi = @FilmAdi, Detay = @Detay, Tarih=@Tarih, Yonetmen = @Yonetmen, KategoriId = @KategoriId, ImgUrl = @ImgUrl WHERE Id=@Id";

            var parameters = new
            {
                model.FilmAdi,
                model.Detay,
                model.Yonetmen,
                model.Tarih,
                model.KategoriId,
                model.Id,
                ImgUrl = imageName
            };

            var affectedRows = connection.Execute(sql, parameters);
            ViewBag.Message = "Güncellendi.";
            ViewBag.MessageCssClass = "alert-success";
            return View("Message");
        }
        public IActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult KategoriEkle(FilmModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MessageCssClass = "alert-danger";
                ViewBag.Message = "Eksik veya hatalı işlem yaptın";
                return View("Message");
            }
            using var connection = new SqlConnection(connectionString);
            var sql = "INSERT INTO Kategoriler (Kategori) VALUES (@Kategori)";
            var data = new
            {
                model.Kategori,
            };
            var rowsAffected = connection.Execute(sql, data);
            ViewBag.MessageCssClass = "alert-success";
            ViewBag.Message = "Eklendi.";
            return View("Message");
        }
    }
}
