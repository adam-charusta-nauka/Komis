using Komis.Models;
using Komis.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Komis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISamochodRepository _samochodReposiotory;

        /*Dzięki temu że użyliśmy wstrzykiwania zależności to nie musimy dodawać klasy MockSamochodRepository przez konstruktor new, tylko dajemy kostruktorowi
         inrofmacje że potrzebuje obiektu na bazie interfejsu(klasy) ISamochodReposiotory, nazywa się to "wstrzyknięciem konstruktora"*/
        public HomeController(ISamochodRepository samochodRepository)
        {
            _samochodReposiotory = samochodRepository;
        }   

        // GET: /<controller>/
        public IActionResult Index()
        { 
            var samochody = _samochodReposiotory.PobierzWszystkieSamochody().OrderBy(s => s.Marka);

            var homeVM = new HomeVM()
            {
                Tytul = "Przegląd Samochodów",
                Samochody = samochody.ToList()
            };

            return View(homeVM);
        }

        public IActionResult Szczegoly(int id)
        {
            var samochod = _samochodReposiotory.PobierzSamochodPoId(id);

            if (samochod == null)
                return NotFound();

            return View(samochod);
        }
    }
}
