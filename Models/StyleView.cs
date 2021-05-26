using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pfi.Models
{
    public class StyleView
    {
        /*--PROPRIÉTÉS--*/
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom du style est requis")]
        [Display(Name = "Style")]
        public string Name { get; set; }

        public virtual ICollection<Film> Films { get; set; }

        /*--CONSTRUCTEUR--*/
        public StyleView()
        {
            Name = "";
        }

        /*--FONCTIONS--*/
        public Style ToStyle()
        {
            return new Style()
            {
                Id = this.Id,
                Name = this.Name,
                Films = this.Films
            };
        }

    }
}