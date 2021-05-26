using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pfi.Models
{
    public class AudienceView
    {
        /*--PROPRIÉTÉS--*/
        public int Id { get; set; }

        [Required(ErrorMessage = "L'audience est requis")]
        [Display(Name = "Audience")]
        public string Name { get; set; }

        public virtual ICollection<Film> Films { get; set; }

        /*--CONSTRUCTEUR--*/
        public AudienceView()
        {
            Name = "";
        }

        /*--FONCTIONS--*/
        public Audience ToAudience()
        {
            return new Audience()
            {
                Id = this.Id,
                Name = this.Name,
                Films = this.Films
            };
        }
    }
}