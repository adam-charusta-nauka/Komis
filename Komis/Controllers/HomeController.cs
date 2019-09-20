using Komis.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Komis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISamochodReposiotory _samochodReposiotory;

        /*Dzięki temu że użyliśmy wstrzykiwania zależności to nie musimy dodawać klasy MockSamochodRepository przez konstruktor new, tylko dajemy kostruktorowi
         inrofmacje że potrzebuje obiektu na bazie interfejsu(klasy) ISamochodReposiotory, nazywa się to "wstrzyknięciem konstruktora"*/
        public HomeController(ISamochodReposiotory samochodRepository)
        {
            _samochodReposiotory = samochodRepository;
        }   

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
