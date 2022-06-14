using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using csa.Model;

namespace csa.Function
{
    public static class BooksHandler
    {
        [FunctionName("ListBooksFunction")]
        public static IActionResult List(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "books")] HttpRequest req,
            ILogger log,
            [Sql("SELECT Id, Author, Title, Pages FROM dbo.Book",
                CommandType = System.Data.CommandType.Text,
                ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Book> books)
        {
            log.LogInformation("ListBooksFunction called");
            return new OkObjectResult(books);
        }

        [FunctionName("GetBookFunction")]
        public static IActionResult Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "books/{id}")] HttpRequest req,
            ILogger log,
            [Sql("SELECT Id, Author, Title, Pages FROM dbo.Book WHERE @Id = Id",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@Id={id}",
                ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Book> books)
        {
            log.LogInformation("GetBookFunction called");
            return new OkObjectResult(books);
        }

        [FunctionName("PutBookFunction")]
        public static IActionResult Put(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "books")] HttpRequest req,
            [Sql("[dbo].[Book]", ConnectionStringSetting = "SqlConnectionString")] out Book output,
            ILogger log)
        {
            log.LogInformation("PutBookFunction called");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            output = JsonConvert.DeserializeObject<Book>(requestBody);

            return new CreatedResult($"/api/books", output);
        }

    }

}
