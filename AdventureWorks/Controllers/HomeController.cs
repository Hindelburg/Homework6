using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdventureWorks.Models;
using AdventureWorks.Models.ViewModels;

namespace AdventureWorks.Controllers
{
    public class HomeController : Controller
    {
        public static int lId;

        private ProductContext db = new ProductContext();

        public ActionResult Index(string main, string sub)
        {
            if (main == null && sub == null)
                return View(db.Products.ToList());
            else if (sub == null)
            {
                System.Diagnostics.Debug.WriteLine("Main no sub.");
                return View(db.Products.Where(s => s.ProductSubcategory.ProductCategoryID == (db.ProductCategories.Where(d => d.Name == main).Select(d => d.ProductCategoryID).FirstOrDefault())).ToList());
            }

            else
            {
                System.Diagnostics.Debug.WriteLine("Main and sub.");
                return View(db.Products.Where(s => s.ProductSubcategoryID == (db.ProductSubcategories.Where(d => d.Name == sub).Select(d => d.ProductSubcategoryID).FirstOrDefault())).Where(s => s.ProductSubcategory.ProductCategoryID == (db.ProductCategories.Where(d => d.Name == main).Select(d => d.ProductCategoryID).FirstOrDefault())));
            }
        }














        public ActionResult List(string main, string sub)
        {
            if (main == null && sub == null)
                return View(db.Products.ToList());
            else if (sub == null)
            {
                List<ProductCatVM> list = new List<ProductCatVM>();
                foreach (var i in db.Products.Where(s => s.ProductSubcategory.ProductCategoryID == (db.ProductCategories.Where(d => d.Name == main).Select(d => d.ProductCategoryID).FirstOrDefault())).ToList())
                {
                    ProductCatVM vm = new ProductCatVM(i, null, db.ProductProductPhotoes.Where(g => g.ProductID == i.ProductID).FirstOrDefault().ProductPhoto);

                    list.Add(vm);
                }

                System.Diagnostics.Debug.WriteLine("Main no sub.");
                return View(list);
            }

            else
            {
                List<ProductCatVM> list = new List<ProductCatVM>();
                foreach (var i in db.Products.Where(s => s.ProductSubcategoryID == (db.ProductSubcategories.Where(d => d.Name == sub).Select(d => d.ProductSubcategoryID).FirstOrDefault())).Where(s => s.ProductSubcategory.ProductCategoryID == (db.ProductCategories.Where(d => d.Name == main).Select(d => d.ProductCategoryID).FirstOrDefault())))
                {
                    ProductCatVM vm = new ProductCatVM(i, null, db.ProductProductPhotoes.Where(g => g.ProductID == i.ProductID).FirstOrDefault().ProductPhoto);

                    list.Add(vm);
                }

                System.Diagnostics.Debug.WriteLine("Main and sub.");
                return View(list);
            }
        }








        [HttpGet]
        public ActionResult Reviews(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            //db.ProductDescriptions.Where(s => s.ProductDescriptionID == (db.ProductModelProductDescriptionCultures.Where(d => d.ProductModelID == (db.Products.Where(f => f.ProductID == id).FirstOrDefault().ProductModelID)).FirstOrDefault().ProductDescriptionID));

            ProductCatVM vm = new ProductCatVM(db.Products.Where(s => s.ProductID == id).FirstOrDefault(), db.ProductDescriptions.Where(s => s.ProductDescriptionID == (db.ProductModelProductDescriptionCultures.Where(d => d.ProductModelID == (db.Products.Where(f => f.ProductID == id).FirstOrDefault().ProductModelID)).FirstOrDefault().ProductDescriptionID)).FirstOrDefault(), db.ProductProductPhotoes.Where(g => g.ProductID == id).FirstOrDefault().ProductPhoto);

            foreach (ProductReview p in db.ProductReviews.Where(s => s.ProductID == id))
            {
                vm.tmpReview.message = p.Comments;
                vm.tmpReview.rating = p.Rating;
                vm.tmpReview.name = p.ReviewerName;
                vm.tmpReview.email = p.EmailAddress;
                vm.reviews.Add(vm.tmpReview);
            }
            System.Diagnostics.Debug.WriteLine(id);
            lId = id ?? default(int);
            return View(vm);
            //return View(db.Products.Where(s => s.ProductID == id));
        }

        [HttpPost]
        public ActionResult Reviews(FormCollection form)
        {
            System.Diagnostics.Debug.WriteLine(lId);

            ProductCatVM vm = new ProductCatVM(db.Products.Where(s => s.ProductID == lId).FirstOrDefault(), db.ProductDescriptions.Where(s => s.ProductDescriptionID == (db.ProductModelProductDescriptionCultures.Where(d => d.ProductModelID == (db.Products.Where(f => f.ProductID == lId).FirstOrDefault().ProductModelID)).FirstOrDefault().ProductDescriptionID)).FirstOrDefault(), db.ProductProductPhotoes.Where(g => g.ProductID == lId).FirstOrDefault().ProductPhoto);


            ViewBag.RequestMethod = "POST";
            //int Rating = int.Parse(Request.Form["rating"]);
            string ReviewerName = Request.Form["name"];
            string EmailAddress = Request.Form["email"];
            string Comments = Request.Form["message"];
            int Rating = int.Parse(Request.Form["rating"]);




            var review = new ProductReview();
            review.ProductID = lId;


            review.ReviewerName = ReviewerName;
            review.EmailAddress = EmailAddress;
            review.Rating = Rating;
            review.Comments = Comments;
            review.ReviewDate = DateTime.Today;
            review.ModifiedDate = DateTime.Today;

            db.ProductReviews.Add(review);
            db.SaveChanges();

            foreach (ProductReview p in db.ProductReviews.Where(s => s.ProductID == lId))
            {
                vm.tmpReview.message = p.Comments;
                vm.tmpReview.rating = p.Rating;
                vm.tmpReview.name = p.ReviewerName;
                vm.tmpReview.email = p.EmailAddress;
                vm.reviews.Add(vm.tmpReview);
            }

            return View(vm);
        }















        [HttpGet]
        public ActionResult Product(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }

            //db.ProductDescriptions.Where(s => s.ProductDescriptionID == (db.ProductModelProductDescriptionCultures.Where(d => d.ProductModelID == (db.Products.Where(f => f.ProductID == id).FirstOrDefault().ProductModelID)).FirstOrDefault().ProductDescriptionID));

            ProductCatVM vm = new ProductCatVM(db.Products.Where(s => s.ProductID == id).FirstOrDefault(), db.ProductDescriptions.Where(s => s.ProductDescriptionID == (db.ProductModelProductDescriptionCultures.Where(d => d.ProductModelID == (db.Products.Where(f => f.ProductID == id).FirstOrDefault().ProductModelID)).FirstOrDefault().ProductDescriptionID)).FirstOrDefault(), db.ProductProductPhotoes.Where(g => g.ProductID == id).FirstOrDefault().ProductPhoto);

            foreach (ProductReview p in db.ProductReviews.Where(s => s.ProductID == id)) {
                vm.tmpReview.message = p.Comments;
                vm.tmpReview.rating = p.Rating;
                vm.tmpReview.name = p.ReviewerName;
                vm.tmpReview.email = p.EmailAddress;
                vm.reviews.Add(vm.tmpReview);
            }
            System.Diagnostics.Debug.WriteLine(id);
            lId = id ?? default(int);
            return View(vm);
            //return View(db.Products.Where(s => s.ProductID == id));
        }
    }
}
