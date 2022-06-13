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
    public static class GetBooksFunction
    {
        [FunctionName("GetBooksFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "books/{id}")] HttpRequest req,
            ILogger log,
            [Sql("SELECT Id, Author, Title, Pages FROM dbo.Book WHERE @Id = Id",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@Id={id}",
                ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Book> books)
        {
            return new OkObjectResult(books);
        }
    }
}
