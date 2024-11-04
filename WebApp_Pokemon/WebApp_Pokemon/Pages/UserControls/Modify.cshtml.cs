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

        private readonly string _connectionString;

        public ModifyModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void OnGet()
        {
            string id = Request.Query["id"];

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
                        }
                    }
                }
            }
        }

        public void OnPost()
        {
            Modify();
        }

        public void Modify()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "UPDATE Pokemons SET Name = @Name, Type = @Type, Level = @Level WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", Pokemon.Id);
                    cmd.Parameters.AddWithValue("@Name", Pokemon.Name);
                    cmd.Parameters.AddWithValue("@Type", Pokemon.Type);
                    cmd.Parameters.AddWithValue("@Level", Pokemon.Level);
                    cmd.ExecuteNonQuery();
                }
            }

            Response.Redirect("/MenuPages/Pokemons");
        }
    }
}
