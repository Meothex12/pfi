using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace pfi.Models
{
    public enum choixSexe { Homme, Femme, Autres };

    public class ActorView
    {
        /*--PROPRIÉTÉS--*/
        public int Id { get; set; }

        [Display(Name ="Nom")]
        [Required(ErrorMessage ="Requis")]
        public string Name { get; set; }

        [Display(Name = "Pays")]
        [Required(ErrorMessage = "Choix de pays requis")]
        public int CountryId { get; set; }

        [Display(Name = "Date de naissance")]
        [Required(ErrorMessage = "Requis")]
        [DataType(DataType.Date)]
        public System.DateTime BirthDate { get; set; }

        [Display(Name = "Sexe")]
        [Required(ErrorMessage = "Requis")]
        public int Sexe
        {
            get; set;
        }

        [Required(ErrorMessage = "Requis")]
        public choixSexe Sex { get; set; }


        public string PhotoId { get; set; }

        [Display(Name = "Photo")]
        [Required(ErrorMessage = "Veuillez sélectionner une image")]
        public string PhotoImageData { get; set; }

        private ImageGUIDReference AvatarReference { get; set; }

        [JsonIgnore]
        [Display(Name = "Pays")]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public virtual ICollection<Casting> Castings { get; set; }

        /*--CONSTRUCTEUR--*/
        public ActorView()
        {
            AvatarReference = new ImageGUIDReference(@"/ImageData/Photos/", @"no_avatar.png");
            AvatarReference.MaxSize = 512;
            AvatarReference.HasThumbnail = false;
        }

        public String GetPhotoURL()
        {
            return AvatarReference.GetURL(PhotoId, false);
        }
        public void SaveAvatar()
        {
            PhotoId = AvatarReference.SaveImage(PhotoImageData, PhotoId);
        }
        public void RemoveAvatar()
        {
            AvatarReference.Remove(PhotoId);
        }


        /*--FONCTIONS--*/
        public Actor ToActor()
        {
            return new Actor()
            {
                Id = this.Id,
                Name = this.Name,
                CountryId = this.CountryId,
                Country = this.Country,
                Castings = this.Castings,
                BirthDate = this.BirthDate,
                PhotoId = this.PhotoId,
                Sexe = this.Sexe
            };
        }

        public void CopyToActor(Actor acteur)
        {
            acteur.Id = Id;
            acteur.Name = Name;
            acteur.CountryId = CountryId;
            acteur.Country = Country;
            acteur.Castings = Castings;
            acteur.BirthDate = BirthDate;
            acteur.PhotoId = PhotoId;
            acteur.Sexe = Sexe;
        }

        public void CopyToActorView(ActorView acteur)
        {
            acteur.Id = Id;
            acteur.Name = Name;
            acteur.CountryId = CountryId;
            acteur.Country = Country;
            acteur.Castings = Castings;
            acteur.BirthDate = BirthDate;
            acteur.PhotoId = PhotoId;
            acteur.Sexe = Sexe;
        }
    }
}