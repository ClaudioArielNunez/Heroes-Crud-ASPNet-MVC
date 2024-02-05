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
        public ActionResult Actualizar(int id)
        {
            var heroe = BD.Heroes.FirstOrDefault(x => x.IdHeroe == id);

            return View(heroe);
        }

        [HttpPost]
        public ActionResult Actualizar(Models.Heroes heroe)
        {
            var heroeActualizar = BD.Heroes.FirstOrDefault(x=>x.IdHeroe==heroe.IdHeroe);
            heroeActualizar.Poder = heroe.Poder;
            heroeActualizar.Historia = heroe.Historia;
            heroeActualizar.Nombre = heroe.Nombre;
            heroeActualizar.Debilidad = heroe.Debilidad;
            heroeActualizar.Universo = heroe.Universo;

            // Verificar si hay archivos adjuntos en la solicitud o si son nulos
            if (System.Web.HttpContext.Current.Request.Files["Imagen"] != null
                  && System.Web.HttpContext.Current.Request.Files["Imagen"].ContentLength > 0)
            {
                var imagen = System.Web.HttpContext.Current.Request.Files["Imagen"];
                string ruta = Server.MapPath("~/Content/Images/");
                string filename = imagen.FileName;
                imagen.SaveAs(ruta + filename);

                heroeActualizar.Imagen = "~/Content/Images/" + imagen.FileName;                    
            }

            BD.SaveChanges();
            return RedirectToAction("Detalle", new {id=heroe.IdHeroe});
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