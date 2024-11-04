using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WebApp_Pokemon.Classes;

namespace WebApp_Pokemon.Pages.MenuPages
{
    public class ModifyModel : PageModel
    {
        [BindProperty]
        public Pokemon Pokemon { get; set; } = new Pokemon();
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        private readonly string _connectionString;

        public ModifyModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void OnGet()
        {
            string id = Request.Query["id"];
            LoadPokemon(id);
        }

        private void LoadPokemon(string id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Pokemons WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Pokemon.Id = reader.GetInt32(0);
                            Pokemon.Name = reader.GetString(1);
                            Pokemon.Type = reader.GetString(2);
                            Pokemon.Level = reader.GetInt32(3);
                            Pokemon.ImagePath = reader.IsDBNull(4) ? "Images/default.jpg" : reader.GetString(4);
                        }
                    }
                }
            }
        }

        public void OnPost()
        {
            if (Request.Form.ContainsKey("Save"))
            {
                UpdatePokemon();
                Response.Redirect("/MenuPages/Pokemons");
            }
            else if (Request.Form.ContainsKey("Cancel"))
            {
                Response.Redirect("/MenuPages/Pokemons");
            }
        }

        private void UpdatePokemon()
        {
            string existingImagePath = Pokemon.ImagePath;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "/Images/default.jpg");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }
                existingImagePath = uniqueFileName;
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "UPDATE Pokemons SET Name = @Name, Type = @Type, Level = @Level, Image = @Image WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", Pokemon.Id);
                    cmd.Parameters.AddWithValue("@Name", Pokemon.Name);
                    cmd.Parameters.AddWithValue("@Type", Pokemon.Type);
                    cmd.Parameters.AddWithValue("@Level", Pokemon.Level);
                    cmd.Parameters.AddWithValue("@Image", existingImagePath);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
