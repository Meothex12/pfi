using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using pfi.Models;

namespace pfi.Models { 
    public class UserView
    {
        /*--PROPRIÉTÉS--*/
        private const string REGEX_Identification = @"^((?!^Name$)[-a-zA-Z0-9àâäçèêëéìîïòôöùûüÿñÀÂÄÇÈÊËÉÌÎÏÒÔÖÙÛÜ_. '])+$";

        public int Id { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Nom d'usager")]
        [RegularExpression(REGEX_Identification, ErrorMessage = "Contains forbidden characters.")]
        public string UserName { get; set; }

       
        [Display(Name = "Mot de passe")]
        [StringLength(50, ErrorMessage = "Password must contains at least {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Nouveau mot de passe")]
        [StringLength(50, ErrorMessage = "Password must contains at least {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Requis")]
        [StringLength(50, ErrorMessage = "Password must contains at least {2} characters.", MinimumLength = 6)]
        [Display(Name = "Confirmation")]
        [Compare("NewPassword", ErrorMessage = "Password confirmation doesn't match with new password.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Nom")]
        public string FullName { get; set; }

        public bool Admin { get; set; }

        public string AvatarId { get; set; }

        private ImageGUIDReference AvatarReference { get; set; }

        [Display(Name = "Avatar")]
        [Required(ErrorMessage = "Veuillez sélectionner une image")]
        public string AvatarImageData { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        /*--CONSTRUCTEUR--*/
        public UserView()
        {
            UserName = "";
            FullName = "";
            Password = "";
            ConfirmPassword = "";

            AvatarReference = new ImageGUIDReference(@"/ImageData/Avatars/", @"no_avatar.png");
            AvatarReference.MaxSize = 512;
            AvatarReference.HasThumbnail = false;
        }

        public String GetAvatarURL()
        {
            return AvatarReference.GetURL(AvatarId, false);
        }

        public void SaveAvatar()
        {
            AvatarId = AvatarReference.SaveImage(AvatarImageData, AvatarId);
        }
        public void RemoveAvatar()
        {
            AvatarReference.Remove(AvatarId);
        }

        public User ToUser()
        {   
            return new User()
            {
                Id = this.Id,
                AvatarId = this.AvatarId,
                UserName = this.UserName,
                FullName = this.FullName,
                Password = this.Password,
                Admin = this.Admin,

                Ratings = this.Ratings
            };
        }
        public void CopyToUser(User user)
        { 
            user.Id = Id;
            user.AvatarId = AvatarId;
            user.UserName = UserName;
            user.FullName = FullName;
            user.Password = Password;
            user.Admin = Admin;
            user.Ratings = Ratings;
        }
        public void CopyToUserView(UserView user)
        { 
            user.Id = Id;
            user.AvatarId = AvatarId;
            user.UserName = UserName;
            user.FullName = FullName;
            user.Password = Password;
            user.Admin = Admin;
            user.Ratings = Ratings;
        }
    }
}