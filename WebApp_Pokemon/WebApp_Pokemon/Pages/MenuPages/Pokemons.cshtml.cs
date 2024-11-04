using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using WebApp_Pokemon.Classes;
using Microsoft.Extensions.Configuration;

namespace WebApp_Pokemon.Pages.MenuPages
{
    public class PokemonsModel : PageModel
    {
        private readonly string _connectionString;
        public List<Pokemon> PokemonList = new List<Pokemon>();

        public PokemonsModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void OnGet()
        {
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
                                Level = reader.GetInt32(3)
                            };

                            PokemonList.Add(pokemon);
                        }
                    }
                }
            }
        }
    }
}
