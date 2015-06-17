using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MetStation.Models;

namespace MetStation.Controllers
{
    public class HomeController : Controller //Controler Home
    {
        public ActionResult Index() //Strona główna
        {
            ViewBag.Message = "Strona poświecona wynikom pomiarów ze stacji meteo.";

            return View();
        }

        public ActionResult About() //zakładka About
        {
            ViewBag.Message = "Po co to jest?";

            return View();
        }

        public ActionResult Contact() //zakładka kontakt
        {
            ViewBag.Message = "Kontakt:";

            return View();
        }


        [HttpPost]
        public ViewResult Register(Uzytkownicy User) //Rejestracja użytkownika
        {
            if (ModelState.IsValid)
            {
                using (var dc = new StacjaMeteoEntities2())
                {
                    dc.Uzytkownicy.Add(User);
                    dc.SaveChanges();
                    ModelState.Clear();
                    User = null;
                    ViewBag.Message = "Zarejestrowałeś się!";
                }
            }
            return View();
        }

        public ViewResult Register() //Wynik
        {
            return View();
        }

        public ActionResult WeatherResult()  //strona z pomiarami ze stacji
        {
            var entities = new StacjaMeteoEntities2();

            return View(entities.Pomiary.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Uzytkownicy u) //logowanie
        {
            if (ModelState.IsValid)
            {
                using (var dc = new StacjaMeteoEntities2())
                {
                    var v =
                        dc.Uzytkownicy.Where(a => a.Uzytkownik.Equals(u.Uzytkownik) && a.Haslo.Equals(u.Haslo))
                            .FirstOrDefault();
                    if (v != null)
                    {
                        FormsAuthentication.SetAuthCookie(u.Uzytkownik, false);

                        return RedirectToAction("WeatherResult");
                    }
                }
            }
            return View(u);
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}

