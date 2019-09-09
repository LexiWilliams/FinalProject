using System;
using System.Collections.Generic;

namespace FinalProject_Recipes.Models
{
    public partial class FavoriteRecipes
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RecipeId { get; set; }
        public int? Rating { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
