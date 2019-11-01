using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Books.Domain.Abstract;
using Books.Domain.Concrete;
using Books.Domain.Entities;
using Books.WebUI.Models;
using Books.WebUI.Infrastructure;

namespace Books.WebUI.Controllers
{
    public class PictureController : Controller
    {
        private INodeRepository repository;

        public PictureController(INodeRepository repo)
        {
            repository = repo;
        }

        public ActionResult Index(int NodeID, bool caption = true, int pictureptr = 0, bool thumbnail = false)
        {
            bool haspict = false;

            IEnumerable<Node> nodes = repository.Nodes
                .Where(n => n.NodeID == NodeID);

            IEnumerable<Picture> pictures = repository.Pictures
                .Where(p => p.NodeID == NodeID);

            if (pictures.Count() > 0)
            {
                foreach (Picture p in pictures)
                {
                    p.Picture1 = "\\" + p.Picture1;
                }
                haspict = true;
            }

            PictureIndexViewModel model = new PictureIndexViewModel
            {
                Node = nodes.FirstOrDefault(),
                HasPicture = haspict,
                NoOfPictures = pictures.Count(),
                Pictures = pictures,
                PicturePointer = pictureptr,
                Caption = caption,
                Thumbnail = thumbnail
            };

            return View(model);
        }
	}
}