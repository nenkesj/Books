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

    public class AdminController : Controller
    {
        private INodeRepository repository;

        public AdminController(INodeRepository repo)
        {
            repository = repo;
        }

        public ViewResult Edit(int NodeID)
        {
            Node node = repository.Nodes
                .FirstOrDefault(n => n.NodeID == NodeID);

            TempData["message"] = null;
            ViewBag.SelectedNodeHeading = node.Heading;

            return View(node);
        }

        [HttpPost]
        public ActionResult Edit(Node node, int ParentNodeID = -1, int TreeLevel = -1)
        {

            int LinesNoOf, SentencesNoOf, ParagraphsNoOf;
            List<string> lines, sentences, paragrphs, newParagraphs;
            Paragraphs paragraphs;
            List<int> SentenceInParagraph;

            SentenceInParagraph = new List<int>();
            paragrphs = new List<string>();
            sentences = new List<string>();
            lines = new List<string>();
            newParagraphs = new List<string>();
            paragraphs = new Paragraphs();

            ViewBag.SelectedNodeHeading = node.Heading;

            paragraphs.TheText = node.NodeText;

            paragraphs.NoOfChars = paragraphs.TheText.Length;
            paragraphs.Paragrphs(out ParagraphsNoOf, ref paragrphs, out SentencesNoOf, ref sentences, ref SentenceInParagraph, out LinesNoOf, ref lines, 0, false, true, true, false, true, false, false);

            node.NodeText = paragraphs.TheAlteredText;

            if (ParentNodeID != -1)
            {
                node.TreeID = 2;
                node.TypeID = 5;
                node.ParentNodeID = ParentNodeID;
                node.TreeLevel = (short)TreeLevel;
            }
            if (ModelState.IsValid)
            {
                if (node.Heading.Length > 50) { node.Heading = node.Heading.Substring(0,50);};
                if (node.NodeID == 0)
                {
                    repository.Edit(node);
                    TempData["message"] = string.Format("Node: {0}, {1} ... has been created", node.NodeID, node.Heading);
                }
                else
                {
                    repository.Edit(node);
                    TempData["message"] = string.Format("Node: {0}, {1} ... has been edited", node.NodeID, node.Heading);
                }
                return RedirectToAction("Index", "Book", new { NodeID = node.NodeID });
            }
            else
            {
                // there is something wrong with the data values
                return View(node);
            }
        }

        public ViewResult UnDo(int NodeID)
        {
            Node node = repository.Nodes
                .FirstOrDefault(n => n.NodeID == NodeID);

            ViewBag.SelectedNodeHeading = node.Heading;

            node.NodeText = (string)Session["oldtext"];

            return View("Edit", node);
        }

        public ViewResult New(int NodeID)
        {
            Node currNode = repository.Nodes
                .FirstOrDefault(n => n.NodeID == NodeID);
            Node node = new Node();
            node.NodeID = 0;
            node.TreeID = 2;
            node.TypeID = 5;
            node.ParentNodeID = currNode.ParentNodeID;
            node.TreeLevel = currNode.TreeLevel;
            node.Heading = "Enter Heading Here";
            node.NodeText = "Enter Text Here";

            ViewBag.SelectedNodeHeading = node.Heading;

            return View("Edit", node);
        }

        public ViewResult NewChild(int NodeID)
        {
            Node currNode = repository.Nodes
                .FirstOrDefault(n => n.NodeID == NodeID);
            Node node = new Node();
            node.NodeID = 0;
            node.TreeID = 2;
            node.TypeID = 5;
            node.ParentNodeID = currNode.NodeID;
            node.TreeLevel = (short)(currNode.TreeLevel + 1);
            node.Heading = "Enter Heading Here";
            node.NodeText = "Enter Text Here";

            ViewBag.SelectedNodeHeading = node.Heading;

            return View("Edit", node);
        }

        public RedirectToRouteResult NewPicture(int NodeID, System.Web.HttpPostedFileWrapper picturefile, string picturetitle = "")
        {
            Node currNode = repository.Nodes
                .FirstOrDefault(n => n.NodeID == NodeID);

            repository.NewPicture(currNode.NodeID, picturefile.FileName.Substring(picturefile.FileName.IndexOf("images")), picturetitle);

            TempData["message"] = string.Format("Picture: {0} for Node: {1}, {2} ... has been created", picturetitle, currNode.NodeID, currNode.Heading);

            ViewBag.SelectedNodeHeading = currNode.Heading;

            return RedirectToAction("Index", "Book", new { NodeID = currNode.NodeID });
        }
    }
}