using Dapper;
using JetflixApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace JetflixApp.Controllers
{
    public class HomeController : Controller
    {

      

        public IActionResult Index()
        {
            using var connection = new SqlConnection(connectionString);
            var posts = connection.Query<FilmModel>("SELECT Filmler.*, Kategoriler.Kategori FROM Filmler LEFT JOIN Kategoriler ON Filmler.KategoriId = Kategoriler.Id ").ToList();
            return View(posts);
        }
        public IActionResult Kategoriler()
        {
            using var connection = new SqlConnection(connectionString);
            var posts = connection.Query<FilmModel>("SELECT Filmler.*, Kategoriler.Kategori FROM Filmler LEFT JOIN Kategoriler ON Filmler.KategoriId = Kategoriler.Id ").ToList();
            return View(posts);
        }
        public IActionResult Detay(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }


            // Connect to the database 
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT Filmler.*, Kategoriler.Kategori FROM Filmler LEFT JOIN Kategoriler ON Filmler.KategoriId = Kategoriler.Id WHERE Filmler.Id = @Id";
                var filmModel = connection.QuerySingleOrDefault<FilmModel>(sql, new { Id = id });


                return View(filmModel);
            }

        }

    }
}
