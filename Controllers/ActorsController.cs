using pfi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pfi.Controllers
{
    public class ActorsController : Controller
    {
        private Database1Entities DB = new Database1Entities();

        [UserAccess]
        public ActionResult Index()
        {
            return View(DB.ActorsList());
        }

        [UserAccess]
        public ActionResult ActorsList(List<ActorView> actors)
        {
            return PartialView(actors);
        }

        [AdminAccess]
        public ActionResult Create()
        {
            ViewBag.Countries = DB.CountriesToSelectList();
            ViewBag.SelectedFilms = new List<ListItem>();
            ViewBag.Films = DB.FilmListItems();
            return View(new ActorView());
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id, Country")] ActorView actorView, List<int> SelectedItems/*Liste des Id des films sélectionés*/)
        {
            actorView.Sexe = (int)actorView.Sex;
            DB.AddActor(actorView, SelectedItems);
            return RedirectToAction("Index");
        }

        [UserAccess]
        public ActionResult Details(int id)
        {
            Actor actor = DB.Actors.Find(id);

            ViewBag.Films = actor.FilmsList();
            ViewBag.Countries = DB.CountriesToSelectList();

            return View(actor.ToActorView());
        }

        [AdminAccess]
        public ActionResult Edit(int id)
        {
            Actor actor = DB.Actors.Find(id);
            ViewBag.SelectedFilms = actor.FilmListItems();
            ViewBag.Countries = DB.CountriesToSelectList();
            ViewBag.Films = DB.FilmListItems();
            return View(actor.ToActorView());
        }

        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Country")] ActorView actorView, List<int> SelectedItems/*Liste des Id des films sélectionés*/)
        {
            actorView.Sexe = (int)actorView.Sex;
            DB.UpdateActor(actorView, SelectedItems);
            return RedirectToAction("Index");
        }

        [AdminAccess]
        public ActionResult Delete(int id)
        {
            return View(DB.Actors.Find(id).ToActorView());
        }

        [HttpPost]
        public ActionResult Delete(ActorView actorView)
        {
            DB.RemoveActor(actorView);
            return RedirectToAction("Index");
        }
    }
}