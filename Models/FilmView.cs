using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace pfi.Models
{
    public class FilmView
    {
        /*--PROPRIÉTÉS--*/
        public int Id { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name ="Titre")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Résumé")]
        public string Synopsis { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Auteur")]
        public string Author { get; set; }
        public int AudienceId { get; set; }
        public int StyleId { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Année")]
        public int Year { get; set; }

        public string PosterId { get; set; }
        public double RatingsAverage { get; set; }

        [Required(ErrorMessage = "Nombre d'étoiles requis")]
        public int NbRatings { get; set; }

        [Display(Name = "Poster")]
        [Required(ErrorMessage = "Veuillez sélectionner un poster")]
        public string PosterImageData { get; set; }

        private ImageGUIDReference PosterReference { get; set; }

        [ForeignKey("AudienceId")]
        public virtual Audience Audience { get; set; }
        public virtual ICollection<Casting> Castings { get; set; }

        [ForeignKey("StyleId")]
        public virtual Style Style { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        /*--CONSTRUCTEUR--*/
        public FilmView()
        {
            Title = "";
            Author = "";
            Synopsis = "";
            Year = DateTime.Now.Year;
            if(Ratings != null)
                NbRatings = Ratings.Count;

            //RatingsAverage = NbRatings / 5;
            
            Style = new Style();
            Audience = new Audience();

            PosterReference = new ImageGUIDReference(@"/ImageData/Posters/", @"no_avatar.png");
            PosterReference.MaxSize = 512;
            PosterReference.HasThumbnail = false;
        }

        /*--FONCTIONS--*/
        public String GetPosterURL()
        {
            return PosterReference.GetURL(PosterId, false);
        }
        public void SavePoster()
        {
            PosterId = PosterReference.SaveImage(PosterImageData, PosterId);
        }
        public void RemovePoster()
        {
            PosterReference.Remove(PosterId);
        }
        public Film ToFilm()
        {
            return new Film()
            {
                Id = this.Id,
                Title = this.Title,
                Synopsis = this.Synopsis,
                Audience = this.Audience,
                AudienceId = this.AudienceId,
                Author = this.Author,
                Castings = this.Castings,
                RatingsAverage = this.RatingsAverage,
                Ratings = this.Ratings,
                Style = this.Style,
                StyleId = this.StyleId,
                PosterId = this.PosterId,
                NbRatings = this.NbRatings,
                Year = this.Year
            };
        }

        public void CopyToFilm(Film film)
        {
            film.Id = Id;
            film.Title = Title;
            film.Synopsis = Synopsis;
            film.Audience = Audience;
            film.AudienceId = AudienceId;
            film.Author = Author;
            film.Castings = Castings;
            film.RatingsAverage = RatingsAverage;
            film.Ratings = Ratings;
            film.Style = Style;
            film.StyleId = StyleId;
            film.PosterId = PosterId;
            film.NbRatings = NbRatings;
            film.Year = Year;
        }

        public void CopyToFilmView(FilmView film)
        {
            film.Id = Id;
            film.Title = Title;
            film.Synopsis = Synopsis;
            film.Audience = Audience;
            film.AudienceId = AudienceId;
            film.Author = Author;
            film.Castings = Castings;
            film.RatingsAverage = RatingsAverage;
            film.Ratings = Ratings;
            film.Style = Style;
            film.StyleId = StyleId;
            film.PosterId = PosterId;
            film.NbRatings = NbRatings;
            film.Year = Year;
        }
    }

}


