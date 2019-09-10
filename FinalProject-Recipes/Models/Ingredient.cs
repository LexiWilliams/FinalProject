using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_Recipes.Models
{
    public class IngredientList
    {

        [JsonProperty(PropertyName = "meals")]
        public Ingredient[] ingredients { get; set; }
    }

    public class Ingredient
    {
        public string idIngredient { get; set; }
        public string strIngredient { get; set; }
        public string strDescription { get; set; }
        public string strType { get; set; }
    }
}
