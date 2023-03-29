using Fit_Center.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using RestSharp;

namespace Fit_Center.Controllers
{
    public class UserController : Controller
    {

        private readonly RestClient Client = new RestClient("https://localhost:7275/api/Users/");

        // GET: UserController
        public async Task<IActionResult> AllUsers()
        {
            var UserRequest = new RestRequest("GetList", Method.Get);
            var UserResponse = Client.Execute(UserRequest);

            var userList = JsonConvert.DeserializeObject<List<User>>(UserResponse.Content);

            return View(userList);
        }

        // GET: LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        // GET: LogIn
        [HttpPost]
        public IActionResult LogIn(IFormCollection formCollection)
        {
            var UserRequest = new RestRequest("GetLogin", Method.Get);
            UserRequest.AddParameter("email", formCollection["Email"]);
            UserRequest.AddParameter("password", formCollection["Password"]);

            var UserResponse = Client.Execute(UserRequest);


            if(UserResponse.Content == "")
                return View();
            else
            {
                var user = JsonConvert.DeserializeObject<User>(UserResponse.Content);
                return View("IndexUser",user);
            }
        }

        // GET: SignIn
        public ActionResult SignIn()
        {
            return View();
        }


        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        public IActionResult Create(IFormCollection formCollection)
        {

            var UserRequest = new RestRequest("Insert", Method.Post);

            var Usuario = new User
            {
                Email = formCollection["Email"],
                Password = formCollection["Password"],
                FirstName = formCollection["FirstName"],
                LastName = formCollection["LastName"],
                BirthDate = DateTime.Parse(formCollection["BirthDate"]),
                Role = "ESTUDIANTE"
            };

            UserRequest.AddJsonBody(Usuario);
            var UserResponse = Client.Execute(UserRequest);

            return View("Inicio");
        }


        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User Usuario)
        {
            var db = new fit_centerContext();

            try
            {

                User UsuarioOriginal = db.Users.Find(db.Users.Where(u => u.UserId == id));
                Usuario.Role = UsuarioOriginal.Role;
                db.Users.Update(Usuario);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
