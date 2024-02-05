using Heroes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Heroes.Controllers
{
    public class HeroeController : Controller
    {
        private HeroeConnection BD = new HeroeConnection();

        // GET: Heroe
        public ActionResult Index()
        {
            var listado = BD.Heroes.ToList();
            return View(listado);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            var heroe = BD.Heroes.FirstOrDefault(x=>x.IdHeroe == id);
            return View(heroe);
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(Models.Heroes heroe)
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var imagen = System.Web.HttpContext.Current.Request.Files["Imagen"];//igual al name q le enviamos del form
                string ruta = Server.MapPath("~/Content/Images/");
                string filename = imagen.FileName;
                //guardamos ruta y contenido
                imagen.SaveAs(ruta + filename);
                //guardamos en base de datos
                heroe.Imagen = "~/Content/Images/" + imagen.FileName;
                BD.Heroes.Add(heroe);
                BD.SaveChanges();
            }           
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var heroe = BD.Heroes.FirstOrDefault(x=>x.IdHeroe==id);
            BD.Heroes.Remove(heroe);
            BD.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}