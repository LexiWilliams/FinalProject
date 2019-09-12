using System;
using System.Collections.Generic;

namespace FinalProject_Recipes.Models
{
    public partial class IngredientList
    {
        public string Id { get; set; }
        public bool? Vegan { get; set; }
        public bool? Vegetarian { get; set; }
        public bool? Keto { get; set; }
        public bool? Paleo { get; set; }
        public bool? GlutenFree { get; set; }
        public bool? Pescatarian { get; set; }
    }
}
