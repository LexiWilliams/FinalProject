using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_Recipes.Models
{

    public class CategoryList
    {

        public Category[] categories { get; set; }
    }

    public class Category
    {
        public string idCategory { get; set; }
        public string strCategory { get; set; }
        public string strCategoryThumb { get; set; }
        public string strCategoryDescription { get; set; }
    }

}
