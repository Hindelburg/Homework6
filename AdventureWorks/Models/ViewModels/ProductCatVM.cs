using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorks.Models.ViewModels
{
    public class ProductCatVM
    {
        public Product prod;
        public ProductDescription prodD;
        public ProductPhoto image;
        public struct review
        {
            public string message { get; set; }
            public int rating { get; set; }
            public string name { get; set; }
            public string email { get; set; }
        }
        public review tmpReview;
        public List<review> reviews = new List<review>();


        public ProductCatVM(Product prodP, ProductDescription prodDP, ProductPhoto imageP)
        {
            image = imageP;
            prodD = prodDP;
            prod = prodP;
        }


        //public IEnumerable<ProductCategory> ReviewList { get; set; }
    }
}