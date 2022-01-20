using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using APIBooks.Models.Books;
using Newtonsoft.Json;
using System.Web.Http.Results;


namespace APIBooks.Controllers
{
    public class BooksController : ApiController
    {
        //CREACION DE LOS SERVICIOS

        //Lista Libros
        private List<Books> GetBooks()
        {
            try
            {
                List<Books> result = new List<Books>();

                HttpClient client = new HttpClient();
                var data = client.GetAsync("https://fakerestapi.azurewebsites.net/api/v1/Books").ContinueWith(d =>
                {
                    if (d.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var content = d.Result.Content.ReadAsAsync<List<Books>>();
                        content.Wait();

                        result = content.Result;
                    }
                    else
                    {
                        result = null;
                    }
                });

                data.Wait();
                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Lista libros por ID
        private Books GetBooks(int id)
        {
            try
            {
                Books result = new Books();

                HttpClient client = new HttpClient();
                var data = client.GetAsync($"https://fakerestapi.azurewebsites.net/api/v1/Books/{id}").ContinueWith(d =>
                {
                    if (d.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var content = d.Result.Content.ReadAsAsync<Books>();
                        content.Wait();

                        result = content.Result;
                    }
                    else
                    {
                        result = null;
                    }
                });

                data.Wait();

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Crear Libro
        private bool CreateBook(Books books)
        {
            try
            {
                bool result = false;
                var bookJson = JsonConvert.SerializeObject(books);
                var data = new StringContent(bookJson, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();

                var response = client.PostAsync($"https://fakerestapi.azurewebsites.net/api/v1/Books", data).ContinueWith(d =>
                {
                    if (d.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                });

                response.Wait();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Editar Libro
        private bool EditBook(int id, Books books)
        {
            try
            {
                bool result = false;
                var bookJson = JsonConvert.SerializeObject(books);
                var data = new StringContent(bookJson, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();

                var response = client.PutAsync($"https://fakerestapi.azurewebsites.net/api/v1/Books/{id}", data).ContinueWith(d =>
                {
                    if (d.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                });

                response.Wait();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Borrar Libro

        private bool DeleteBook(int id)
        {
            try
            {
                bool result = false;

                HttpClient client = new HttpClient();
                var data = client.DeleteAsync($"https://fakerestapi.azurewebsites.net/api/v1/Books/{id}").ContinueWith(d =>
                {
                    if (d.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                });

                data.Wait();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Fin de los servicios

        //Creacion de los Metodos

        [HttpGet]
        public IEnumerable<Books> Books()
        {
            try
            {
                List<Books> books = GetBooks();
                return books;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Lista Books
        [HttpGet]
        public Books Books(int id)
        {
            try
            {
                Books books = GetBooks(id);
                return books;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //crear Books
        [HttpPost]
        public IHttpActionResult Post(Books books)
        {
            try
            {
                if (books == null)
                {
                    return BadRequest();
                }

                CreateBook(books);
                return Ok();
            }
            catch (Exception ex)
            {

                return Conflict();
            }
        }

        //Actualizar Libro
        [HttpPut]
        public IHttpActionResult Put(int id, Books books)
        {
            try
            {
                if (books == null)
                {
                    return BadRequest();
                }

                EditBook(id, books);
                return Ok();
            }
            catch (Exception)
            {

                return Conflict();
            }
        }

        //borrar libro
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                bool result = DeleteBook(id);
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
