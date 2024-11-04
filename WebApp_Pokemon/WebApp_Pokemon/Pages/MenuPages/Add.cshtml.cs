using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WebApp_Pokemon.Classes;

namespace WebApp_Pokemon.Pages.MenuPages
{
    public class Add : PageModel
    {
        private readonly string _connectionString;

        public Add(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var newPokemon = new Pokemon
            {
                Name = Request.Form["name"],
                Type = Request.Form["type"],
                Level = int.TryParse(Request.Form["level"], out int level) ? level : 0
            };

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "INSERT INTO Pokemons (Name, Type, Level) VALUES (@Name, @Type, @Level)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", newPokemon.Name);
                    cmd.Parameters.AddWithValue("@Type", newPokemon.Type);
                    cmd.Parameters.AddWithValue("@Level", newPokemon.Level);

                    cmd.ExecuteNonQuery();
                }
            }

            Response.Redirect("/MenuPages/Pokemons");
        }
    }
}
