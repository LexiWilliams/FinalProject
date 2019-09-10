using System;
using System.Collections.Generic;

namespace FinalProject_Recipes.Models
{
    public partial class Friends
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }

        public virtual AspNetUsers Friend { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
