using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Jwttoken.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Jwttoken.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            TempData["path"] = @"C:\ProgramData\users.txt";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string login, string password)
        {
            if(login == null || password == null) {TempData["AlertMessage"] = "Заполните все поля!"; return RedirectToAction("Registration"); }
            else
            {
                string role = CheckPassword(login, password);
                switch (role)
                {
                    case "user":
                        return RedirectPermanent("/User/UserView");

                    case "admin":
                        return RedirectPermanent("/Admin/AdminView");

                    default:
                        TempData["AlertMessage"] = "Неправильный Пароль!";
                        return RedirectToAction("Index");


                }


            }

        }

        private string[] ReadFile(string path) //считывание из файла построчно в string-массив
        {
            int linescount = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    linescount++;
                }
            }
            string[] text = new string[linescount];

            using (StreamReader fs = new StreamReader(path))
            {
                for (int i = 0; ; i++)
                {
                    string? line = fs.ReadLine();
                    if (line == null) break;
                    text[i] = line;
                }
            }

            return text;
        }

        private List<User> ConvertText(string path)//разбиение построчного строчного массива в лист экземляров юзера
        {
            string[] text = ReadFile(path);
            List<User> users = new List<User>();
            foreach (var elem in text)
            {
                string[] line = elem.Split('|');
                users.Add(new User { Login = line[0], Password = line[1], Role = line[2] });
            }
            return users;
        }

        private bool DublicateLogin(string login) //если хотя бьы один логин совпадает вернуть true
        {
            string path = (string)TempData["path"];
            if (System.IO.File.Exists(path))
            {
                var users = ConvertText(path);
                foreach (var user in users)
                {
                    if (user.Login == login) { return true; }
                }
                return false;
            }
            else return false;
        }

        private string CheckPassword(string login, string password)
        {
            var users = ConvertText((string)TempData["path"]);
            foreach (var user in users)
            {
                //а где верифи пассворд?
                
                if (login == user.Login && BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password) == true)
                { 
                        if (user.Role == "user") { return "user"; }
                        else { return "admin"; }
                }
            }
            TempData["AlertMessage"] = "Пользователя с таким логином не существует"; return null ;
        }

    }
}