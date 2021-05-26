using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pfi.Models
{
    public static class DBEntitiesExtensionsMethods
    {
        /*--------------------BD-------------------------*/
        private static DbContextTransaction Transaction
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return (DbContextTransaction)HttpContext.Current.Session["Transaction"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session["Transaction"] = value;
                }
            }
        }
        private static void BeginTransaction(Database1Entities DB)
        {
            if (Transaction != null)
                throw new Exception("Transaction en cours! Impossible d'en démarrer une nouvelle!");
            Transaction = DB.Database.BeginTransaction();
        }
        private static void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
            else
                throw new Exception("Aucune ransaction en cours! Impossible de mettre à jour la base de ddonnées!");
        }


        /*------------------USER-------------------------*/
        public static UserView ToUserView(this User user)
        {
            return new UserView()
            {
                Id = user.Id,
                AvatarId = user.AvatarId,
                UserName = user.UserName,
                FullName = user.FullName,
                Password = user.Password,
                Admin = user.Admin,
                Ratings = user.Ratings
            };
        }
        public static UserView AddUser(this Database1Entities DB, UserView userView)
        {
            userView.SaveAvatar();
            User user = userView.ToUser();
            user = DB.Users.Add(user);
            DB.SaveChanges();
            return user.ToUserView();
        }

        public static bool UserNameExist(this Database1Entities DB, string userName)
        {
            User user = DB.Users.Where(u => u.UserName == userName).FirstOrDefault();
            return (user != null);
        }
        public static User FindByUserName(this Database1Entities DB, string userName)
        {
            User user = DB.Users.Where(u => u.UserName == userName).FirstOrDefault();
            return user;
        }

        public static bool UpdateUser(this Database1Entities DB, UserView userView)
        {
            userView.SaveAvatar();
            User userToUpdate = DB.Users.Find(userView.Id);
            userView.CopyToUser(userToUpdate);
            DB.Entry(userToUpdate).State = EntityState.Modified;
            DB.SaveChanges();
            return true;
        }

        /*------------------ACTOR-------------------------*/
        public static ActorView ToActorView(this Actor actor)
        {
            return new ActorView()
            {
                Id = actor.Id,
                Name = actor.Name,
                CountryId = actor.CountryId,
                Country = actor.Country,
                Castings = actor.Castings,
                BirthDate = actor.BirthDate,
                PhotoId = actor.PhotoId,
                Sexe = actor.Sexe
            };
        }
        public static List<ActorView> ActorsList(this Film film)
        {
            List<ActorView> actors = new List<ActorView>();
            if (film.Castings != null)
            {
                foreach (Casting casting in film.Castings)
                {
                    actors.Add(casting.Actor.ToActorView());
                }
            }
            return actors.OrderBy(f => f.Name).ToList();
        }
        public static List<ActorView> ActorsList(this Database1Entities DB)
        {
            List<ActorView> actors = new List<ActorView>();
            if (DB.Actors != null)
            {
                foreach (Actor actor in DB.Actors)
                {
                    actors.Add(actor.ToActorView());
                }
            }
            return actors.OrderBy(f => f.Name).ToList();
        }
        public static List<ListItem> ActorListItems(this Film film)
        {
            List<ListItem> actors = new List<ListItem>();
            if (film.Castings != null)
            {
                foreach (Casting casting in film.Castings)
                {
                    actors.Add(new ListItem() { Id = casting.Actor.Id, Text = casting.Actor.Name });
                }
            }
            return actors;
        }
        public static List<ListItem> ActorListItems(this Database1Entities DB)
        {
            List<ListItem> actors = new List<ListItem>();
            if (DB.Actors != null)
            {
                foreach (Actor actor in DB.Actors)
                {
                    actors.Add(new ListItem() { Id = actor.Id, Text = actor.Name });
                }
            }
            return actors;
        }
        public static ActorView AddActor(this Database1Entities DB, ActorView actorView, List<int> filmsIdList)
        {
            actorView.SaveAvatar();
            Actor actor = new Actor();
            actorView.CopyToActor(actor);
            BeginTransaction(DB);
            actor = DB.Actors.Add(actor);
            DB.SaveChanges();
            SetActorCastings(DB, actor.Id, filmsIdList);
            Commit();
            return actor.ToActorView();
        }
        public static bool UpdateActor(this Database1Entities DB, ActorView actorView, List<int> filmsIdList)
        {
            actorView.SaveAvatar();
            Actor actor = DB.Actors.Find(actorView.Id);
            actorView.CopyToActor(actor);
            BeginTransaction(DB);
            DB.Entry(actor).State = EntityState.Modified;
            DB.SaveChanges();
            SetActorCastings(DB, actor.Id, filmsIdList);
            Commit();
            return true;
        }
        public static bool RemoveActor(this Database1Entities DB, ActorView actorView)
        {
            Actor actor = DB.Actors.Find(actorView.Id);
            BeginTransaction(DB);
            SetActorCastings(DB, actor.Id, null);
            DB.Actors.Remove(actor);
            DB.SaveChanges();
            Commit();
            return true;
        }
        private static bool SetActorCastings(Database1Entities DB, int actor_Id, List<int> filmsIdList)
        {
            DB.Castings.RemoveRange(DB.Castings.Where(c => c.ActorId == actor_Id));
            if (filmsIdList != null)
            {
                foreach (int film_Id in filmsIdList)
                {
                    Casting casting = new Casting() { ActorId = actor_Id, FilmId = film_Id };
                    DB.Castings.Add(casting);
                }
            }
            DB.SaveChanges();
            return true;
        }

        /*------------------FILMS-------------------------*/
        public static FilmView ToFilmView(this Film film)
        {
            return new FilmView()
            {
                Id = film.Id,
                Title = film.Title,
                Synopsis = film.Synopsis,
                Audience = film.Audience,
                AudienceId = film.AudienceId,
                Author = film.Author,
                Castings = film.Castings,
                RatingsAverage = film.RatingsAverage,
                Ratings = film.Ratings,
                Style = film.Style,
                StyleId = film.StyleId,
                PosterId = film.PosterId,
                NbRatings = film.NbRatings,
                Year = film.Year
            };
        }
        public static List<FilmView> FilmsList(this Actor actor)
        {
            List<FilmView> films = new List<FilmView>();
            if (actor.Castings != null)
            {
                foreach (Casting casting in actor.Castings)
                {
                    films.Add(casting.Film.ToFilmView());
                }
            }
            return films.OrderBy(f => f.Title).ToList();
        }
        public static List<FilmView> FilmsList(this Database1Entities DB)
        {
            List<FilmView> films = new List<FilmView>();
            if (DB.Films != null)
            {
                foreach (Film film in DB.Films)
                {
                    films.Add(film.ToFilmView());
                }
            }
            return films.OrderBy(f => f.Title).ToList();
        }
        public static List<ListItem> FilmListItems(this Actor actor)
        {
            List<ListItem> films = new List<ListItem>();
            if (actor.Castings != null)
            {
                foreach (Casting casting in actor.Castings)
                {
                    films.Add(new ListItem() { Id = casting.Film.Id, Text = casting.Film.Title });
                }
            }
            return films;
        }
        public static List<ListItem> FilmListItems(this Database1Entities DB)
        {
            List<ListItem> films = new List<ListItem>();
            if (DB.Films != null)
            {
                foreach (Film film in DB.Films)
                {
                    films.Add(new ListItem() { Id = film.Id, Text = film.Title });
                }
            }
            return films;
        }
        public static FilmView AddFilm(this Database1Entities DB, FilmView filmView, List<int> actorsIdList)
        {
            filmView.SavePoster();
            Film film = new Film();
            filmView.CopyToFilm(film);
            BeginTransaction(DB);
            film = DB.Films.Add(film);
            DB.SaveChanges();
            SetFilmCastings(DB, film.Id, actorsIdList);
            Commit();
            return film.ToFilmView();
        }
        public static bool UpdateFilm(this Database1Entities DB, FilmView filmView, List<int> actorsIdList)
        {
            filmView.SavePoster();
            Film film = DB.Films.Find(filmView.Id);
            filmView.CopyToFilm(film);
            BeginTransaction(DB);
            DB.Entry(film).State = EntityState.Modified;
            DB.SaveChanges();
            SetFilmCastings(DB, film.Id, actorsIdList);
            Commit();
            return true;
        }
        public static bool RemoveFilm(this Database1Entities DB, FilmView filmView)
        {
            Film film = DB.Films.Find(filmView.Id);
            BeginTransaction(DB);
            SetFilmCastings(DB, film.Id, null);
            DB.Films.Remove(film);
            DB.SaveChanges();
            Commit();
            return true;
        }
        private static bool SetFilmCastings(Database1Entities DB, int film_Id, List<int> actorsIdList)
        {
            DB.Castings.RemoveRange(DB.Castings.Where(c => c.FilmId == film_Id));
            if (actorsIdList != null)
            {
                foreach (int actor_Id in actorsIdList)
                {
                    Casting casting = new Casting() { ActorId = actor_Id, FilmId = film_Id };
                    DB.Castings.Add(casting);
                }
            }
            DB.SaveChanges();
            return true;
        }

        /*------------------RATING-------------------------*/
        public static RatingView ToRatingView(this Rating rating)
        {
            return new RatingView()
            {
                Id = rating.Id,
                FilmId = rating.FilmId,
                UserId = rating.UserId,
                Film = rating.Film,
                User = rating.User,
                Value = rating.Value,
                Comment = rating.Comment,
                RatingDate = rating.RatingDate
            };
        }
        public static RatingView AddRating(this Database1Entities DB, RatingView ratingview)
        {
            Rating rating = new Rating();
            ratingview.CopyToRating(rating);
            BeginTransaction(DB);
            rating = DB.Ratings.Add(rating);
            DB.SaveChanges();
            Commit();
            return rating.ToRatingView();
        }
        public static bool UpdateRating(this Database1Entities DB, RatingView ratingView)
        {
            Rating rating = DB.Ratings.Find(ratingView.Id);
            ratingView.CopyToRating(rating);
            BeginTransaction(DB);
            DB.Entry(rating).State = EntityState.Modified;
            DB.SaveChanges();
            Commit();
            return true;
        }
        public static bool RemoveRating(this Database1Entities DB, RatingView ratingView)
        {
            Rating rating = DB.Ratings.Find(ratingView.Id);
            BeginTransaction(DB);
            SetFilmCastings(DB, rating.Id, null);
            DB.Ratings.Remove(rating);
            DB.SaveChanges();
            Commit();
            return true;
        }
        public static List<RatingView> RatingsList(this Database1Entities DB)
        {
            List<RatingView> ratings = new List<RatingView>();
            if (DB.Ratings != null)
            {
                foreach (Rating r in DB.Ratings)
                {
                    ratings.Add(r.ToRatingView());
                }
            }
            return ratings.OrderBy(f => f.Film.Title).ToList();
        }
        public static List<RatingView> RatingsList(this Film film)
        {
            List<RatingView> ratings = new List<RatingView>();
            if (film.Ratings != null)
            {
                foreach (Rating r in film.Ratings)
                {
                    ratings.Add(r.ToRatingView());
                }
            }
            return ratings.OrderBy(f => f.Film.Title).ToList();
        }
        public static List<ListItem> RatingListItems(this Film film)
        {
            List<ListItem> ratingList = new List<ListItem>();
            if (film.Ratings != null)
            {
                foreach (Rating rating in film.Ratings)
                {
                    ratingList.Add(new ListItem() { Id = rating.Film.Id, Text = rating.Film.Title });
                }
            }
            return ratingList;
        }
        public static List<ListItem> RatingListItems(this Database1Entities DB)
        {
            List<ListItem> ratingList = new List<ListItem>();
            if (DB.Ratings != null)
            {
                foreach (Rating rating in DB.Ratings)
                {
                    ratingList.Add(new ListItem() { Id = rating.Id, Text = rating.Comment });
                }
            }
            return ratingList;
        }
    }
}
