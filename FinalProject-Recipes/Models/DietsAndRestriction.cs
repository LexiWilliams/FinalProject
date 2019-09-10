using System;
using System.Collections.Generic;

namespace FinalProject_Recipes.Models
{
    public partial class DietsAndRestriction
    {
        public string Id { get; set; }
        public bool? Vegan { get; set; }
        public bool? Vegetarian { get; set; }
        public bool? Keto { get; set; }
        public bool? Paleo { get; set; }
        public bool? GlutenFree { get; set; }
        public bool? Pescatarian { get; set; }
        public bool? Milk { get; set; }
        public bool? Eggs { get; set; }
        public bool? Shellfish { get; set; }
        public bool? Fish { get; set; }
        public bool? Soy { get; set; }
        public bool? Wheat { get; set; }
        public bool? Treenuts { get; set; }
        public bool? Peaunts { get; set; }
    }
}
