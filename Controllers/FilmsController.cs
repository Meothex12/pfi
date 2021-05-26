using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pfi.Models;
using pfi.Controllers;
using System.Web.Mvc;

namespace pfi.Controllers
{
    public class FilmsController : Controller
    {
        private Database1Entities DB = new Database1Entities();

        [UserAccess]
        public ActionResult Index()
        {
            return View(DB.FilmsList());
        }

        [UserAccess]
        public ActionResult FilmsList(List<FilmView> films)
        {
            return PartialView(films);
        }

        [AdminAccess]
        public ActionResult Create()
        {
            ViewBag.SelectedActors = new List<ListItem>();
            ViewBag.Actors = DB.ActorListItems();
            return View(new FilmView());
        }

        [HttpPost]
        public ActionResult Create(FilmView filmView, List<int> SelectedItems)
        {
            DB.AddFilm(filmView, SelectedItems);
            return RedirectToAction("Index");
        }

        [UserAccess]
        public ActionResult Details(int id)
        {
            Film film = DB.Films.Find(id);

            ViewBag.Actors = film.ActorsList();
            ViewBag.Ratings = film.RatingsList();
            ViewBag.SelectedRatings = DB.RatingListItems();

            return View(film.ToFilmView());
        }

        [AdminAccess]
        public ActionResult Edit(int id)
        {
            Film film = DB.Films.Find(id);
            ViewBag.SelectedActors = film.ActorListItems();
            ViewBag.Actors = DB.ActorListItems();
            return View(film.ToFilmView());
        }

        [HttpPost]
        public ActionResult Edit(FilmView filmView, List<int> SelectedItems)
        {
            DB.UpdateFilm(filmView, SelectedItems);
            return RedirectToAction("Index");
        }

        [AdminAccess]
        public ActionResult Delete(int id)
        {
            return View(DB.Films.Find(id).ToFilmView());
        }
        [HttpPost]
        public ActionResult Delete(FilmView filmView)
        {
            DB.RemoveFilm(filmView);
            return RedirectToAction("Index");
        }

        /*--Liste de tout les ratings d'un film--*/
        [UserAccess]
        public ActionResult Rating(List<RatingView> rating)
        {
            ViewBag.Ratings = DB.RatingListItems();
            return View(rating);
        }

        [HttpPost]
        public ActionResult AddRating(RatingView model)
        {
            UserView onlineUser = OnlineUsers.CurrentUser;
            if (model.Comment == null)
                model.Comment = "";
            model.User = onlineUser.ToUser();
            model.UserId = onlineUser.Id;
            model.Film = DB.Films.Find(model.FilmId);
            DB.AddRating(model);
            return RedirectToAction($"Details/{model.FilmId}");
        }

        [UserAccess]
        public ActionResult EditRating(int id)
        {
            return View(DB.Ratings.Find(id).ToRatingView());
        }

        [HttpPost]
        public ActionResult EditRating(RatingView rating)
        {
            rating.UserId = OnlineUsers.GetSessionUser().Id;
            DB.UpdateRating(rating);
            return RedirectToAction($"Details/{rating.FilmId}");
        }

        [HttpPost]
        public ActionResult DeleteRating(RatingView rating)
        {
            UserView onlineUser = OnlineUsers.CurrentUser;
            rating.User = onlineUser.ToUser();
            rating.Film = DB.Films.Find(rating.FilmId);
            DB.RemoveRating(rating);
            return RedirectToAction($"Details/{rating.FilmId}");
        }
    }
}