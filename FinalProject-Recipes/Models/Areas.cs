using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_Recipes.Models
{

    public class AreaList
    {

        [JsonProperty(PropertyName = "meals")]
        public Meal[] meals { get; set; }
    }

    public class Area
    {
        public string strArea { get; set; }
    }

}
