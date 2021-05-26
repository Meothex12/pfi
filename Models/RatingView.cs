using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace pfi.Models
{
    public class RatingView
    {
        /*--PROPRIÉTÉS--*/
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Valeurs")]
        [Required(ErrorMessage = "Requis")]
        public int Value { get; set; }

        [Display(Name = "Commentaire")]
        [Required(ErrorMessage = "Requis")]
        public string Comment { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime RatingDate { get; set; }

        [ForeignKey("FilmId")]
        public virtual Film Film { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /*--CONSTRUCTEUR--*/
        public RatingView()
        {
            Value = 0;
            Comment = "";
            RatingDate = DateTime.Now;
        }

        /*--FONCTIONS--*/
 

        public Rating ToRating()
        {
            return new Rating()
            {
                Id = this.Id,
                FilmId = this.FilmId,
                UserId = this.UserId,
                Film = this.Film,
                User = this.User,
                Value = this.Value,
                Comment = this.Comment,
                RatingDate = this.RatingDate
            };
        }

        public void CopyToRating(Rating rating)
        {
            rating.Id = Id;
            rating.FilmId = FilmId;
            rating.UserId = UserId;
            rating.Film = Film;
            rating.User = User;
            rating.Value = Value;
            rating.Comment = Comment;
            rating.RatingDate = RatingDate;
        }

        public void CopyToRatingView(RatingView rating)
        {
            rating.Id = Id;
            rating.FilmId = FilmId;
            rating.UserId = UserId;
            rating.Film = Film;
            rating.User = User;
            rating.Value = Value;
            rating.Comment = Comment;
            rating.RatingDate = RatingDate;
        }
    }
}