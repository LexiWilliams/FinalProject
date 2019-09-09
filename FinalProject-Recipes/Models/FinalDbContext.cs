using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FinalProject_Recipes.Models
{
    public partial class FinalDbContext : DbContext
    {
        public FinalDbContext()
        {
        }

        public FinalDbContext(DbContextOptions<FinalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        public virtual DbSet<Diets> Diets { get; set; }
        public virtual DbSet<FavoriteRecipes> FavoriteRecipes { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=FinalDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Diet).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Group).HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.DietNavigation)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.Diet)
                    .HasConstraintName("FK__AspNetUser__Diet__778AC167");
            });

            modelBuilder.Entity<Diets>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Ingredient1).HasMaxLength(50);

                entity.Property(e => e.Ingredient10).HasMaxLength(50);

                entity.Property(e => e.Ingredient100).HasMaxLength(50);

                entity.Property(e => e.Ingredient101).HasMaxLength(50);

                entity.Property(e => e.Ingredient102).HasMaxLength(50);

                entity.Property(e => e.Ingredient103).HasMaxLength(50);

                entity.Property(e => e.Ingredient104).HasMaxLength(50);

                entity.Property(e => e.Ingredient105).HasMaxLength(50);

                entity.Property(e => e.Ingredient106).HasMaxLength(50);

                entity.Property(e => e.Ingredient107).HasMaxLength(50);

                entity.Property(e => e.Ingredient108).HasMaxLength(50);

                entity.Property(e => e.Ingredient109).HasMaxLength(50);

                entity.Property(e => e.Ingredient11).HasMaxLength(50);

                entity.Property(e => e.Ingredient110).HasMaxLength(50);

                entity.Property(e => e.Ingredient111).HasMaxLength(50);

                entity.Property(e => e.Ingredient112).HasMaxLength(50);

                entity.Property(e => e.Ingredient113).HasMaxLength(50);

                entity.Property(e => e.Ingredient114).HasMaxLength(50);

                entity.Property(e => e.Ingredient115).HasMaxLength(50);

                entity.Property(e => e.Ingredient116).HasMaxLength(50);

                entity.Property(e => e.Ingredient117).HasMaxLength(50);

                entity.Property(e => e.Ingredient118).HasMaxLength(50);

                entity.Property(e => e.Ingredient119).HasMaxLength(50);

                entity.Property(e => e.Ingredient12).HasMaxLength(50);

                entity.Property(e => e.Ingredient120).HasMaxLength(50);

                entity.Property(e => e.Ingredient121).HasMaxLength(50);

                entity.Property(e => e.Ingredient122).HasMaxLength(50);

                entity.Property(e => e.Ingredient123).HasMaxLength(50);

                entity.Property(e => e.Ingredient124).HasMaxLength(50);

                entity.Property(e => e.Ingredient125).HasMaxLength(50);

                entity.Property(e => e.Ingredient126).HasMaxLength(50);

                entity.Property(e => e.Ingredient127).HasMaxLength(50);

                entity.Property(e => e.Ingredient128).HasMaxLength(50);

                entity.Property(e => e.Ingredient129).HasMaxLength(50);

                entity.Property(e => e.Ingredient13).HasMaxLength(50);

                entity.Property(e => e.Ingredient130).HasMaxLength(50);

                entity.Property(e => e.Ingredient131).HasMaxLength(50);

                entity.Property(e => e.Ingredient132).HasMaxLength(50);

                entity.Property(e => e.Ingredient133).HasMaxLength(50);

                entity.Property(e => e.Ingredient134).HasMaxLength(50);

                entity.Property(e => e.Ingredient135).HasMaxLength(50);

                entity.Property(e => e.Ingredient136).HasMaxLength(50);

                entity.Property(e => e.Ingredient137).HasMaxLength(50);

                entity.Property(e => e.Ingredient138).HasMaxLength(50);

                entity.Property(e => e.Ingredient139).HasMaxLength(50);

                entity.Property(e => e.Ingredient14).HasMaxLength(50);

                entity.Property(e => e.Ingredient140).HasMaxLength(50);

                entity.Property(e => e.Ingredient141).HasMaxLength(50);

                entity.Property(e => e.Ingredient142).HasMaxLength(50);

                entity.Property(e => e.Ingredient143).HasMaxLength(50);

                entity.Property(e => e.Ingredient144).HasMaxLength(50);

                entity.Property(e => e.Ingredient145).HasMaxLength(50);

                entity.Property(e => e.Ingredient146).HasMaxLength(50);

                entity.Property(e => e.Ingredient147).HasMaxLength(50);

                entity.Property(e => e.Ingredient148).HasMaxLength(50);

                entity.Property(e => e.Ingredient149).HasMaxLength(50);

                entity.Property(e => e.Ingredient15).HasMaxLength(50);

                entity.Property(e => e.Ingredient150).HasMaxLength(50);

                entity.Property(e => e.Ingredient151).HasMaxLength(50);

                entity.Property(e => e.Ingredient152).HasMaxLength(50);

                entity.Property(e => e.Ingredient153).HasMaxLength(50);

                entity.Property(e => e.Ingredient154).HasMaxLength(50);

                entity.Property(e => e.Ingredient155).HasMaxLength(50);

                entity.Property(e => e.Ingredient156).HasMaxLength(50);

                entity.Property(e => e.Ingredient157).HasMaxLength(50);

                entity.Property(e => e.Ingredient158).HasMaxLength(50);

                entity.Property(e => e.Ingredient159).HasMaxLength(50);

                entity.Property(e => e.Ingredient16).HasMaxLength(50);

                entity.Property(e => e.Ingredient160).HasMaxLength(50);

                entity.Property(e => e.Ingredient161).HasMaxLength(50);

                entity.Property(e => e.Ingredient162).HasMaxLength(50);

                entity.Property(e => e.Ingredient163).HasMaxLength(50);

                entity.Property(e => e.Ingredient164).HasMaxLength(50);

                entity.Property(e => e.Ingredient165).HasMaxLength(50);

                entity.Property(e => e.Ingredient166).HasMaxLength(50);

                entity.Property(e => e.Ingredient167).HasMaxLength(50);

                entity.Property(e => e.Ingredient168).HasMaxLength(50);

                entity.Property(e => e.Ingredient169).HasMaxLength(50);

                entity.Property(e => e.Ingredient17).HasMaxLength(50);

                entity.Property(e => e.Ingredient170).HasMaxLength(50);

                entity.Property(e => e.Ingredient171).HasMaxLength(50);

                entity.Property(e => e.Ingredient172).HasMaxLength(50);

                entity.Property(e => e.Ingredient173).HasMaxLength(50);

                entity.Property(e => e.Ingredient174).HasMaxLength(50);

                entity.Property(e => e.Ingredient175).HasMaxLength(50);

                entity.Property(e => e.Ingredient176).HasMaxLength(50);

                entity.Property(e => e.Ingredient177).HasMaxLength(50);

                entity.Property(e => e.Ingredient178).HasMaxLength(50);

                entity.Property(e => e.Ingredient179).HasMaxLength(50);

                entity.Property(e => e.Ingredient18).HasMaxLength(50);

                entity.Property(e => e.Ingredient180).HasMaxLength(50);

                entity.Property(e => e.Ingredient181).HasMaxLength(50);

                entity.Property(e => e.Ingredient182).HasMaxLength(50);

                entity.Property(e => e.Ingredient183).HasMaxLength(50);

                entity.Property(e => e.Ingredient184).HasMaxLength(50);

                entity.Property(e => e.Ingredient185).HasMaxLength(50);

                entity.Property(e => e.Ingredient186).HasMaxLength(50);

                entity.Property(e => e.Ingredient187).HasMaxLength(50);

                entity.Property(e => e.Ingredient188).HasMaxLength(50);

                entity.Property(e => e.Ingredient189).HasMaxLength(50);

                entity.Property(e => e.Ingredient19).HasMaxLength(50);

                entity.Property(e => e.Ingredient190).HasMaxLength(50);

                entity.Property(e => e.Ingredient191).HasMaxLength(50);

                entity.Property(e => e.Ingredient192).HasMaxLength(50);

                entity.Property(e => e.Ingredient193).HasMaxLength(50);

                entity.Property(e => e.Ingredient194).HasMaxLength(50);

                entity.Property(e => e.Ingredient195).HasMaxLength(50);

                entity.Property(e => e.Ingredient196).HasMaxLength(50);

                entity.Property(e => e.Ingredient197).HasMaxLength(50);

                entity.Property(e => e.Ingredient198).HasMaxLength(50);

                entity.Property(e => e.Ingredient199).HasMaxLength(50);

                entity.Property(e => e.Ingredient2).HasMaxLength(50);

                entity.Property(e => e.Ingredient20).HasMaxLength(50);

                entity.Property(e => e.Ingredient200).HasMaxLength(50);

                entity.Property(e => e.Ingredient201).HasMaxLength(50);

                entity.Property(e => e.Ingredient202).HasMaxLength(50);

                entity.Property(e => e.Ingredient203).HasMaxLength(50);

                entity.Property(e => e.Ingredient204).HasMaxLength(50);

                entity.Property(e => e.Ingredient205).HasMaxLength(50);

                entity.Property(e => e.Ingredient206).HasMaxLength(50);

                entity.Property(e => e.Ingredient207).HasMaxLength(50);

                entity.Property(e => e.Ingredient208).HasMaxLength(50);

                entity.Property(e => e.Ingredient209).HasMaxLength(50);

                entity.Property(e => e.Ingredient21).HasMaxLength(50);

                entity.Property(e => e.Ingredient210).HasMaxLength(50);

                entity.Property(e => e.Ingredient211).HasMaxLength(50);

                entity.Property(e => e.Ingredient212).HasMaxLength(50);

                entity.Property(e => e.Ingredient213).HasMaxLength(50);

                entity.Property(e => e.Ingredient214).HasMaxLength(50);

                entity.Property(e => e.Ingredient215).HasMaxLength(50);

                entity.Property(e => e.Ingredient216).HasMaxLength(50);

                entity.Property(e => e.Ingredient217).HasMaxLength(50);

                entity.Property(e => e.Ingredient218).HasMaxLength(50);

                entity.Property(e => e.Ingredient219).HasMaxLength(50);

                entity.Property(e => e.Ingredient22).HasMaxLength(50);

                entity.Property(e => e.Ingredient220).HasMaxLength(50);

                entity.Property(e => e.Ingredient221).HasMaxLength(50);

                entity.Property(e => e.Ingredient222).HasMaxLength(50);

                entity.Property(e => e.Ingredient223).HasMaxLength(50);

                entity.Property(e => e.Ingredient224).HasMaxLength(50);

                entity.Property(e => e.Ingredient225).HasMaxLength(50);

                entity.Property(e => e.Ingredient226).HasMaxLength(50);

                entity.Property(e => e.Ingredient227).HasMaxLength(50);

                entity.Property(e => e.Ingredient228).HasMaxLength(50);

                entity.Property(e => e.Ingredient229).HasMaxLength(50);

                entity.Property(e => e.Ingredient23).HasMaxLength(50);

                entity.Property(e => e.Ingredient230).HasMaxLength(50);

                entity.Property(e => e.Ingredient231).HasMaxLength(50);

                entity.Property(e => e.Ingredient24).HasMaxLength(50);

                entity.Property(e => e.Ingredient25).HasMaxLength(50);

                entity.Property(e => e.Ingredient26).HasMaxLength(50);

                entity.Property(e => e.Ingredient27).HasMaxLength(50);

                entity.Property(e => e.Ingredient28).HasMaxLength(50);

                entity.Property(e => e.Ingredient29).HasMaxLength(50);

                entity.Property(e => e.Ingredient3).HasMaxLength(50);

                entity.Property(e => e.Ingredient30).HasMaxLength(50);

                entity.Property(e => e.Ingredient31).HasMaxLength(50);

                entity.Property(e => e.Ingredient32).HasMaxLength(50);

                entity.Property(e => e.Ingredient33).HasMaxLength(50);

                entity.Property(e => e.Ingredient34).HasMaxLength(50);

                entity.Property(e => e.Ingredient35).HasMaxLength(50);

                entity.Property(e => e.Ingredient36).HasMaxLength(50);

                entity.Property(e => e.Ingredient37).HasMaxLength(50);

                entity.Property(e => e.Ingredient38).HasMaxLength(50);

                entity.Property(e => e.Ingredient39).HasMaxLength(50);

                entity.Property(e => e.Ingredient4).HasMaxLength(50);

                entity.Property(e => e.Ingredient40).HasMaxLength(50);

                entity.Property(e => e.Ingredient41).HasMaxLength(50);

                entity.Property(e => e.Ingredient42).HasMaxLength(50);

                entity.Property(e => e.Ingredient43).HasMaxLength(50);

                entity.Property(e => e.Ingredient44).HasMaxLength(50);

                entity.Property(e => e.Ingredient45).HasMaxLength(50);

                entity.Property(e => e.Ingredient46).HasMaxLength(50);

                entity.Property(e => e.Ingredient47).HasMaxLength(50);

                entity.Property(e => e.Ingredient48).HasMaxLength(50);

                entity.Property(e => e.Ingredient49).HasMaxLength(50);

                entity.Property(e => e.Ingredient5).HasMaxLength(50);

                entity.Property(e => e.Ingredient50).HasMaxLength(50);

                entity.Property(e => e.Ingredient51).HasMaxLength(50);

                entity.Property(e => e.Ingredient52).HasMaxLength(50);

                entity.Property(e => e.Ingredient53).HasMaxLength(50);

                entity.Property(e => e.Ingredient54).HasMaxLength(50);

                entity.Property(e => e.Ingredient55).HasMaxLength(50);

                entity.Property(e => e.Ingredient56).HasMaxLength(50);

                entity.Property(e => e.Ingredient57).HasMaxLength(50);

                entity.Property(e => e.Ingredient58).HasMaxLength(50);

                entity.Property(e => e.Ingredient59).HasMaxLength(50);

                entity.Property(e => e.Ingredient6).HasMaxLength(50);

                entity.Property(e => e.Ingredient60).HasMaxLength(50);

                entity.Property(e => e.Ingredient61).HasMaxLength(50);

                entity.Property(e => e.Ingredient62).HasMaxLength(50);

                entity.Property(e => e.Ingredient63).HasMaxLength(50);

                entity.Property(e => e.Ingredient64).HasMaxLength(50);

                entity.Property(e => e.Ingredient65).HasMaxLength(50);

                entity.Property(e => e.Ingredient66).HasMaxLength(50);

                entity.Property(e => e.Ingredient67).HasMaxLength(50);

                entity.Property(e => e.Ingredient68).HasMaxLength(50);

                entity.Property(e => e.Ingredient69).HasMaxLength(50);

                entity.Property(e => e.Ingredient7).HasMaxLength(50);

                entity.Property(e => e.Ingredient70).HasMaxLength(50);

                entity.Property(e => e.Ingredient71).HasMaxLength(50);

                entity.Property(e => e.Ingredient72).HasMaxLength(50);

                entity.Property(e => e.Ingredient73).HasMaxLength(50);

                entity.Property(e => e.Ingredient74).HasMaxLength(50);

                entity.Property(e => e.Ingredient75).HasMaxLength(50);

                entity.Property(e => e.Ingredient76).HasMaxLength(50);

                entity.Property(e => e.Ingredient77).HasMaxLength(50);

                entity.Property(e => e.Ingredient78).HasMaxLength(50);

                entity.Property(e => e.Ingredient79).HasMaxLength(50);

                entity.Property(e => e.Ingredient8).HasMaxLength(50);

                entity.Property(e => e.Ingredient80).HasMaxLength(50);

                entity.Property(e => e.Ingredient81).HasMaxLength(50);

                entity.Property(e => e.Ingredient82).HasMaxLength(50);

                entity.Property(e => e.Ingredient83).HasMaxLength(50);

                entity.Property(e => e.Ingredient84).HasMaxLength(50);

                entity.Property(e => e.Ingredient85).HasMaxLength(50);

                entity.Property(e => e.Ingredient86).HasMaxLength(50);

                entity.Property(e => e.Ingredient87).HasMaxLength(50);

                entity.Property(e => e.Ingredient88).HasMaxLength(50);

                entity.Property(e => e.Ingredient89).HasMaxLength(50);

                entity.Property(e => e.Ingredient9).HasMaxLength(50);

                entity.Property(e => e.Ingredient90).HasMaxLength(50);

                entity.Property(e => e.Ingredient91).HasMaxLength(50);

                entity.Property(e => e.Ingredient92).HasMaxLength(50);

                entity.Property(e => e.Ingredient93).HasMaxLength(50);

                entity.Property(e => e.Ingredient94).HasMaxLength(50);

                entity.Property(e => e.Ingredient95).HasMaxLength(50);

                entity.Property(e => e.Ingredient96).HasMaxLength(50);

                entity.Property(e => e.Ingredient97).HasMaxLength(50);

                entity.Property(e => e.Ingredient98).HasMaxLength(50);

                entity.Property(e => e.Ingredient99).HasMaxLength(50);
            });

            modelBuilder.Entity<FavoriteRecipes>(entity =>
            {
                entity.Property(e => e.RecipeId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteRecipes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FavoriteR__UserI__74AE54BC");
            });

            modelBuilder.Entity<Friends>(entity =>
            {
                entity.Property(e => e.FriendId).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.FriendsFriend)
                    .HasForeignKey(d => d.FriendId)
                    .HasConstraintName("FK__Friends__FriendI__71D1E811");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FriendsUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Friends__UserId__70DDC3D8");
            });

            modelBuilder.Entity<Ingredients>(entity =>
            {
                entity.Property(e => e.IdIngredient)
                    .IsRequired()
                    .HasColumnName("idIngredient")
                    .HasMaxLength(100);

                entity.Property(e => e.StrIngredient)
                    .IsRequired()
                    .HasColumnName("strIngredient")
                    .HasMaxLength(100);

                entity.Property(e => e.StrType)
                    .HasColumnName("strType")
                    .HasMaxLength(100);
            });
        }
    }
}
