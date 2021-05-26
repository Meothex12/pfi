﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pfi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Mvc;

    public partial class Database1Entities : DbContext
    {
        public Database1Entities()
            : base("name=Database1Entities")
        {
        }
    
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    throw new UnintentionalCodeFirstException();
        //}
    
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Audience> Audiences { get; set; }
        public virtual DbSet<Casting> Castings { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Style> Styles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public SelectList CountriesToSelectList()
        {
            var items = Countries.Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name }).ToList();
            items.Insert(0, new SelectListItem { Value = "", Text = "" });
            return new SelectList(items, "Value", "Text");
        }

    }
}
