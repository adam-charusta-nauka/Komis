using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Komis.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Komis.Controllers
{
    public class SamochodController : Controller
    {
        private readonly ISamochodRepository _samochodRepository;
        private IHostingEnvironment _env;

        public SamochodController(ISamochodRepository samochodRepository, IHostingEnvironment env)
        {
            _samochodRepository = samochodRepository;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_samochodRepository.PobierzWszystkieSamochody());
        }

        public IActionResult Details(int id)
        {
            var samochod = _samochodRepository.PobierzSamochodPoId(id);

            if (samochod == null)
                return NotFound();

            return View(samochod);

        }

        public IActionResult Create(string FileName)
        {
            if (!string.IsNullOrEmpty(FileName))
                ViewBag.ImgPath = "/Images/" + FileName;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Samochod samochod)
        {
            if (ModelState.IsValid)
            {
                _samochodRepository.DodajSamochod(samochod);
                return RedirectToAction(nameof (Index));
            }
            return View(samochod);
        }

        public IActionResult Edit(int Id, string fileName)
        {
            var samochod = _samochodRepository.PobierzSamochodPoId(Id);

            if (samochod == null)
                return NotFound();

            if (!string.IsNullOrEmpty(fileName))
                ViewBag.ImgPath = "/Images/" + fileName;
            else
                ViewBag.ImgPath = samochod.ZdjecieUrl;

            return View(samochod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Samochod samochod)
        {
            if (ModelState.IsValid)
            {
                _samochodRepository.EdytujSamochod(samochod);
                return RedirectToAction(nameof(Index));
            }
            return View(samochod);
        }

        public IActionResult Delete (int Id)
        {
            var samochod = _samochodRepository.PobierzSamochodPoId(Id);

            if (samochod == null)
                return NotFound();

            return View(samochod);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string ZdjecieUrl)
        {
            var samochod = _samochodRepository.PobierzSamochodPoId(id);
            _samochodRepository.UsunSamochod(samochod);

            //usuwanie pliku

            if (ZdjecieUrl != null)
            {
                var webRoot = _env.WebRootPath;
                var filePath = Path.Combine(webRoot.ToString() + ZdjecieUrl);
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormCollection form)
        {
            var webRoot = _env.WebRootPath;
            var filePath = Path.Combine(webRoot.ToString() + "\\Images\\" + form.Files[0].FileName);

            if(form.Files[0].FileName.Length > 0)
            {
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await form.Files[0].CopyToAsync(stream);
                }
            }

            if (Convert.ToString(form["Id"]) == string.Empty || Convert.ToString(form["Id"]) == "0")
                return RedirectToAction(nameof(Create), new { FileName = Convert.ToString(form.Files[0].FileName) });

            return RedirectToAction(nameof(Edit), new { FileName = Convert.ToString(form.Files[0].FileName), id = Convert.ToString(form["Id"])});
        }
    }
}