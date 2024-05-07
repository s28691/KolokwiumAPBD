using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KolokwiumAPBD.Controllers;
[Route("api/[controller]")]
[ApiController]
public class booksController : ControllerBase
{
    private readonly string _connectionString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True";

    [HttpGet]
    public IActionResult getBooks(int id)
    {
        List<String> books = new List<string>();
        List<String> genres = new List<string>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT books.title FROM books_genres JOIN books ON FK_book=books.PK JOIN genres ON genres.PK=books_genres.FK_genre WHERE PK = @id", connection);
            command.Parameters.AddWithValue("@Id", id);
            var reader = command.ExecuteReader();
            int title = reader.GetOrdinal("title");
            while (reader.Read())
            {
                books.Add(reader.GetString(title));
            }
            SqlCommand command2 = new SqlCommand("SELECT genres.name FROM books_genres JOIN books ON FK_book=books.PK JOIN genres ON genres.PK=books_genres.FK_genre WHERE PK = @id", connection);
            command.Parameters.AddWithValue("@Id", id);
            var reader2 = command.ExecuteReader();
            int genre = reader.GetOrdinal("title");
            while (reader2.Read())
            {
                genres.Add(reader.GetString(genre));
            }
        }
        books.AddRange(genres);
        return Ok(books);
    }

    [HttpPost]
    public IActionResult addBook()
    {
        
        return Ok();
    }
}