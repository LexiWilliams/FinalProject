using System;
using System.Collections.Generic;

namespace FinalProject_Recipes.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            FavoriteRecipes = new HashSet<FavoriteRecipes>();
            FriendsFriend = new HashSet<Friends>();
            FriendsUser = new HashSet<Friends>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Diet { get; set; }
        public string Group { get; set; }
        public bool? Milk { get; set; }
        public bool? Eggs { get; set; }
        public bool? Fish { get; set; }
        public bool? Shellfish { get; set; }
        public bool? Treenuts { get; set; }
        public bool? Peanuts { get; set; }
        public bool? Wheat { get; set; }
        public bool? Soy { get; set; }

        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<FavoriteRecipes> FavoriteRecipes { get; set; }
        public virtual ICollection<Friends> FriendsFriend { get; set; }
        public virtual ICollection<Friends> FriendsUser { get; set; }
    }
}
