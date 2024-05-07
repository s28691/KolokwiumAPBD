using KolokwiumAPBD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KolokwiumAPBD.Controllers;
[Route("api/[controller]")]
[ApiController]
public class booksController : ControllerBase
{
    private readonly string _connectionString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True";

    [HttpGet("{id:int}/genres")]
    public IActionResult getBooks(int id)
    {
        List<String> books = new List<string>();
        List<String> genres = new List<string>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT books.title FROM books_genres JOIN books ON FK_book=books.PK JOIN genres ON genres.PK=books_genres.FK_genre WHERE books.PK = @id", connection);
            command.Parameters.AddWithValue("@Id", id);
            var reader = command.ExecuteReader();
            int title = reader.GetOrdinal("title");
            while (reader.Read())
            {
                books.Add(reader.GetString(title));
            }
            SqlCommand command2 = new SqlCommand("SELECT genres.name FROM books_genres JOIN books ON FK_book=books.PK JOIN genres ON genres.PK=books_genres.FK_genre WHERE books.PK = @id", connection);
            command2.Parameters.AddWithValue("@Id", id);
            var reader2 = command2.ExecuteReader();
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
    public IActionResult addBook(Book book)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand("INSERT INTO books(title) VALUES (@title)", connection);
            command.Parameters.AddWithValue("@title", book.title);
            command.ExecuteNonQuery();
            SqlCommand getPKforBook = new SqlCommand("SELECT pk from books WHERE title = @title", connection);
            getPKforBook.Parameters.AddWithValue("@title", book.title);
            var reader = getPKforBook.ExecuteReader();
            var PKord = reader.GetOrdinal("pk");
            var PK = 0;
            while(reader.Read())
            {
                PK = reader.GetInt32(PKord);
            }
            for (int a = 0; a < book.genres.Count; a++)
            {
                SqlCommand command2 = new SqlCommand("INSERT INTO books_genres(FK_book, FK_genre) VALUES (@pk, @genre) ", connection);
                command2.Parameters.AddWithValue("@pk", PK);
                command2.Parameters.AddWithValue("@genre", book.genres[a]);
                command2.ExecuteNonQuery();
            }
        }
        return StatusCode(201);
    }
}