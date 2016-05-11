using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476_TP3_Pucacco_Alex.Models;
using System.IO;

namespace _420_476_TP3_Pucacco_Alex.Controllers
{
    public class ProductsController : Controller
    {
        private NORTHWNDEntities db = new NORTHWNDEntities();

        // GET: Products
        public ActionResult Index()
        {
            if (Session["Connected"] != null)
            {
                var products = db.Products.Include(p => p.Category).Include(p => p.Supplier);
                ViewBag.Category = new SelectList(db.Categories,"CategoryID","CategoryName");
                return View(products.ToList());
            }
            return RedirectToAction("Index", "Login");

            
        }
        [HttpPost]
        public ActionResult Index(int CategoryID)
        {
            ViewBag.Category = new SelectList(db.Categories, "CategoryID", "CategoryName");
            var products = db.Products.Where(p => p.CategoryID == CategoryID).Include(p => p.Category).Include(p => p.Supplier);
            return View(products.ToList());

        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Connected"]!=null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.image = product.Photo;
                return View(product);
            }
            return RedirectToAction("Index","Login");
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            if (Session["Connected"] != null)
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
                return View();
            }
            return RedirectToAction("Index", "Login");
            
        }

        // POST: Products/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Photo")] Product product)
        {
            
            if (Session["Connected"] != null)
            {
                if (Request.Files.Count > 0)
                {
                     var file = Request.Files[0];
                    //Vérifier le fichier soumis est non vide
                    if (file != null && file.ContentLength > 0)
                    {
                        if (MimeMapping.GetMimeMapping(file.FileName) == "image/jpg" || MimeMapping.GetMimeMapping(file.FileName) == "image/png" || MimeMapping.GetMimeMapping(file.FileName) == "image/jpeg")
                        {
                            //Récupérer le nom du fichier soumis
                            var fileName = Path.GetFileName(file.FileName);
                            //Récupérer l'extension du fichier soumis
                            var fileExtension = Path.GetExtension(file.FileName);
                            //Créer le chemin relatif à partir du dossier du projet pour la sauvegarde du fichier téléversé
                            var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                            file.SaveAs(path);
                            product.Photo = fileName;
                        }
                    }
                }
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);


               
                

                    return View(product);
            }
            return RedirectToAction("Index", "Login");
            
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Connected"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
                
                
                return View(product);
            }
            return RedirectToAction("Index", "Login");

           
        }

        // POST: Products/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Photo")] Product product)
        {
            if (Session["Connected"] != null)
            {
                var product2 = db.Products.Find(product.ProductID);

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    //Vérifier le fichier soumis est non vide
                    if (file != null && file.ContentLength > 0)
                    {
                        if (MimeMapping.GetMimeMapping(file.FileName) == "image/jpg" || MimeMapping.GetMimeMapping(file.FileName) == "image/png" || MimeMapping.GetMimeMapping(file.FileName) == "image/jpeg")
                        {
                            //Récupérer le nom du fichier soumis
                            var fileName = Path.GetFileName(file.FileName);
                            //Récupérer l'extension du fichier soumis
                            var fileExtension = Path.GetExtension(file.FileName);
                            //Créer le chemin relatif à partir du dossier du projet pour la sauvegarde du fichier téléversé
                            var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                            file.SaveAs(path);
                            product.Photo = fileName;
                        }
                    }
                }
                else
                {
                    if (!(product.Photo.Equals(product2.Photo)))
                    {
                        product.Photo = product2.Photo;
                    }
                }
                
                        if (ModelState.IsValid)
                {
                    
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
                return View(product);
            }
            return RedirectToAction("Index", "Login");
          
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Connected"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            return RedirectToAction("Index", "Login");
            
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Connected"] != null)
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Login");
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);

        }
    }
}
