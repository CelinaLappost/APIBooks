using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using APIBooksClient.DTO;
using Newtonsoft.Json;
using System.Web.Http;
using System.Net.Http.Formatting;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using System.Web.Mvc.Ajax;

namespace APIBooksClient.Controllers
{
    public class BooksController : Controller
    {
        public BooksController()
        {
            
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        //Servicios 

        //Lista libros
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

        //Lista Libros por ID
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

        //Creacion de los Metodos

        public ActionResult Index(int? id)
        {
            List<Books> book = new List<Books>();
            if (id != null)
            {
                Books books = GetBooks((int)id);

                if (books != null) book.Add(books);
            }
            else
            {
                book = GetBooks();
            }
            return View(book);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Books());
        }
        public ActionResult Detail(int id)
        {
            Books books = GetBooks(id);
            return View(books);
        }

        [HttpPost]
        public ActionResult Create(Books books)
        {
            bool result = false;
            if (books != null)
            {
                result = CreateBook(books);
                TempData["mensaje"] = "Se creo el libro correctamente!";
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
            if (!result)
            {
                ViewBag.Error = "Ha ocurrido un error!";
                return View(books);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Books books = GetBooks(id);

            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        [HttpPost]
        public ActionResult Edit(int id, Books book)
        {
            if (book == null)
            {
                return HttpNotFound();
            }

            bool result = EditBook(id, book);
            TempData["mensaje"] = "Se edito el libro correctamente!";
            return RedirectToAction("Index");

            if (!result)
            {
                ViewBag.Error = "Ha ocurrido un error!";

                return View(book);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            bool result = DeleteBook(id);
            TempData["mensaje"] = "Se elimino el libro correctamente!";
            return RedirectToAction("Index");
            if (!result)
            {
                ViewBag.Error = "Ha ocurrido un error!";
            }

            List<Books> books = GetBooks();

            return View("Index", books);
        }


    }
}