using System;
using System.Collections.Generic;

namespace FinalProject_Recipes.Models
{
    public partial class Group
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string GroupName { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
