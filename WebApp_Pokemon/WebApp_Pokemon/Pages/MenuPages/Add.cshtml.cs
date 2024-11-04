using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WebApp_Pokemon.Classes;

namespace WebApp_Pokemon.Pages.MenuPages
{
    public class Add : PageModel
    {
        private readonly string _connectionString;
        private readonly string _imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/default.jpg");

        public Add(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void OnGet()
        {
            // Do nothing for GET requests
        }

        public void OnPost(IFormFile image)
        {
            var newPokemon = CreatePokemon(image);
            Response.Redirect("/MenuPages/Pokemons");
        }

        public Pokemon CreatePokemon(IFormFile image)
        {
            var newPokemon = new Pokemon
            {
                Name = Request.Form["name"],
                Type = Request.Form["type"],
                Level = int.TryParse(Request.Form["level"], out int level) ? level : 0
            };

            if (image != null && image.Length > 0)
            {
                if (!Directory.Exists(_imagesFolderPath))
                {
                    Directory.CreateDirectory(_imagesFolderPath);
                }

                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(_imagesFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                newPokemon.ImagePath = fileName;
            }
            else
            {
                newPokemon.ImagePath = "Images/default.jpg";
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "INSERT INTO Pokemons (Name, Type, Level, Image) VALUES (@Name, @Type, @Level, @Image)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", newPokemon.Name);
                    cmd.Parameters.AddWithValue("@Type", newPokemon.Type);
                    cmd.Parameters.AddWithValue("@Level", newPokemon.Level);
                    cmd.Parameters.AddWithValue("@Image", newPokemon.ImagePath);

                    cmd.ExecuteNonQuery();
                }
            }

            return newPokemon;
        }
    }
}
