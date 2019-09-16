using FinalProject_Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinalProject_Recipes.Controllers
{
    public class RecipeMethods
    {
        public static HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.themealdb.com");
            return client;
        }
        public static List<string> AddIngredients(Meal item)
        {
            var ingredients = new List<string>();
            if (item.strIngredient1 != null)
            {
                ingredients.Add(item.strIngredient1.ToLower());
            }
            if (item.strIngredient2 != null)
            {
                ingredients.Add(item.strIngredient2.ToLower());
            }
            if (item.strIngredient3 != null)
            {
                ingredients.Add(item.strIngredient3.ToLower());
            }
            if (item.strIngredient4 != null)
            {
                ingredients.Add(item.strIngredient4.ToLower());
            }
            if (item.strIngredient5 != null)
            {
                ingredients.Add(item.strIngredient5.ToLower());
            }
            if (item.strIngredient6 != null)
            {
                ingredients.Add(item.strIngredient6.ToLower());
            }
            if (item.strIngredient7 != null)
            {
                ingredients.Add(item.strIngredient7.ToLower());
            }
            if (item.strIngredient8 != null)
            {
                ingredients.Add(item.strIngredient8.ToLower());
            }
            if (item.strIngredient9 != null)
            {
                ingredients.Add(item.strIngredient9.ToLower());
            }
            if (item.strIngredient10 != null)
            {
                ingredients.Add(item.strIngredient10.ToLower());
            }
            if (item.strIngredient11 != null)
            {
                ingredients.Add(item.strIngredient11.ToLower());
            }
            if (item.strIngredient12 != null)
            {
                ingredients.Add(item.strIngredient12.ToLower());
            }
            if (item.strIngredient13 != null)
            {
                ingredients.Add(item.strIngredient13.ToLower());
            }
            if (item.strIngredient14 != null)
            {
                ingredients.Add(item.strIngredient14.ToLower());
            }
            if (item.strIngredient15 != null)
            {
                ingredients.Add(item.strIngredient15.ToLower());
            }
            if (item.strIngredient16 != null)
            {
                ingredients.Add(item.strIngredient16.ToLower());
            }
            if (item.strIngredient17 != null)
            {
                ingredients.Add(item.strIngredient17.ToLower());
            }
            if (item.strIngredient18 != null)
            {
                ingredients.Add(item.strIngredient18.ToLower());
            }
            if (item.strIngredient19 != null)
            {
                ingredients.Add(item.strIngredient19.ToLower());
            }
            if (item.strIngredient20 != null)
            {
                ingredients.Add(item.strIngredient20.ToLower());
            }
            return ingredients;
        }
    }
}
