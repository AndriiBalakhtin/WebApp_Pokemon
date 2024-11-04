using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WebApp_Pokemon.Classes;

namespace WebApp_Pokemon.Pages.MenuPages
{
    public class PokemonsModel : PageModel
    {
        private readonly string _connectionString;
        public List<Pokemon> PokemonList { get; set; } = new List<Pokemon>();

        public PokemonsModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void OnGet()
        {
            LoadPokemons();
        }

        public void OnPost()
        {
            if (Request.Form.ContainsKey("Delete"))
            {
                int id = int.Parse(Request.Form["id"]);
                DeletePokemon(id);
            }
            LoadPokemons();
        }

        private void LoadPokemons()
        {
            PokemonList.Clear();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Pokemons";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pokemon = new Pokemon
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Type = reader.GetString(2),
                                Level = reader.GetInt32(3),
                                ImagePath = reader.IsDBNull(4) ? "Images/default.jpg" : reader.GetString(4)
                            };

                            PokemonList.Add(pokemon);
                        }
                    }
                }
            }
        }

        private void DeletePokemon(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Pokemons WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
