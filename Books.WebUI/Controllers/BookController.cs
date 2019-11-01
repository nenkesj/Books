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
    public class BookController : Controller
    {
        private INodeRepository repository;

        public BookController(INodeRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(int NodeID, string Display = "Details", bool picturefixed = false, int pictureptr = 0, string searchkey = "")
        {
            bool haspict = false;
            bool hassumm = false;
            bool haschild = false;
            bool hasparent = true;
            bool showingdetails = true;
            bool showingsummary = false;
            bool hasnofigpara = true;
            bool hasnotabpara = true;
            bool itHasATableHeading = false;
            bool tableStarted = false;
            bool NodeIDOK = false;
            int noofchildren = 0;
            int listStartedAt = -1;
            int tableStartedAt = -1;

            int LinesNoOf, SentencesNoOf, ParagraphsNoOf, newNoOfParagraphs;
            string p;
            List<string> lines, sentences, paragrphs, newParagraphs;
            Paragraphs paragraphs;
            List<int> SentenceInParagraph;
            List<Node> MatchingNodes;
            IEnumerable<Node> nodes;
            IEnumerable<Summary> summaries;
            IEnumerable<Key> keys;
            IEnumerable<Node> children;
            IEnumerable<Picture> pictures;

            MatchingNodes = new List<Node>();
            SentenceInParagraph = new List<int>();
            paragrphs = new List<string>();
            sentences = new List<string>();
            lines = new List<string>();
            newParagraphs = new List<string>();
            paragraphs = new Paragraphs();

            if (searchkey == "")
            {
                Session["SearchKey"] = searchkey;
                Session["SearchMsg"] = "";
                nodes = repository.Nodes.Where(n => n.NodeID == NodeID);
                summaries = repository.Summaries.Where(n => n.NodeID == NodeID);
                pictures = repository.Pictures.Where(pic => pic.NodeID == NodeID);
                children = repository.Nodes.Where(n => n.ParentNodeID == NodeID);
            }
            else
            {
                keys = repository.Keys.Where(k => k.TreeID == 2 && k.KeyText == searchkey);
                int noofkeys = keys.Count();

                if (keys.Count() > 0)
                {
                    MatchingNodes.Clear();
                    foreach (Key k in keys)
                    {
                        Node n = repository.Nodes.Single(node => node.NodeID == k.NodeID);
                        MatchingNodes.Add(n);
                        if (k.NodeID == NodeID)
                        {
                            NodeIDOK = true;
                        }
                    }
                    if (!NodeIDOK)
                    {
                        Session["SearchReturn"] = NodeID;
                        NodeID = MatchingNodes.FirstOrDefault().NodeID;
                    }
                    children = MatchingNodes;
                    int noofchild = children.Count();
                    int noofmatchingnodes = MatchingNodes.Count();
                    Session["SearchMsg"] = "Searched on - ";
                    Session["SearchKey"] = searchkey;
                }
                else
                {
                    Session["SearchMsg"] = "No Hits on - ";
                    Session["SearchKey"] = searchkey;
                    children = repository.Nodes.Where(n => n.ParentNodeID == NodeID);
                }
                nodes = repository.Nodes.Where(n => n.NodeID == NodeID);
                summaries = repository.Summaries.Where(n => n.NodeID == NodeID);
                pictures = repository.Pictures.Where(pic => pic.NodeID == NodeID);
            }

            if (Display == "Details")
            {
                paragraphs.TheText = nodes.FirstOrDefault().NodeText;
                showingdetails = true;
                showingsummary = false;
            }

            if (Display == "Summary")
            {
                paragraphs.TheText = summaries.FirstOrDefault().Summary1;
                showingsummary = true;
                showingdetails = false;
            }

            paragraphs.NoOfChars = paragraphs.TheText.Length;
            paragraphs.Paragrphs(out ParagraphsNoOf, ref paragrphs, out SentencesNoOf, ref sentences, ref SentenceInParagraph, out LinesNoOf, ref lines, 0, false, true, true, false, true, false, true);

            if (children.Count() > 0)
            {
                haschild = true;
                noofchildren = children.Count();
            }

            if (pictures.Count() > 0)
            {
                haspict = true;
            }

            if (summaries.Count() > 0)
            {
                hassumm = true;
            }

            if (nodes.FirstOrDefault().TreeLevel == 1)
            {
                hasparent = false;
            }

            newNoOfParagraphs = 0;
            for (int i = 0; i < ParagraphsNoOf; i++)
            {
                p = paragrphs.ToArray()[i];
                if (p.Length > 2)
                {
                    if (p.Substring(0, 3).ToLower() == "fig") { hasnofigpara = false; }
                    if (p.Substring(0, 3).ToLower() == "tab") 
                    { 
                        hasnotabpara = false;
                        itHasATableHeading = true; 
                        tableStarted = false;
                    }
                }
                if (p.Substring(0, 1) == "¤" || p.Substring(0, 1) == "¥" || p.IndexOf((Char)9) > 0)
                {
                    if (p.Substring(0, 1) == "¤" || p.Substring(0, 1) == "¥")
                    {
                        tableStartedAt = -1;
                        if (listStartedAt == -1)
                        {
                            listStartedAt = newNoOfParagraphs;
                            newParagraphs.Add("");
                            newNoOfParagraphs++;
                        }
                        if (listStartedAt > -1)
                        {
                            newParagraphs[listStartedAt] += p;
                        }
                    }
                    if (p.IndexOf((Char)9) > 0)
                    {
                        if (itHasATableHeading)
                        {
                            listStartedAt = -1;
                            if (tableStartedAt == -1)
                            {
                                tableStarted = true;
                                tableStartedAt = newNoOfParagraphs;
                                newParagraphs.Add("");
                                newNoOfParagraphs++;
                            }
                            if (tableStartedAt > -1)
                            {
                                if (p.Substring(0, 1) == "§")
                                {
                                    newParagraphs[tableStartedAt] += "£" + p.Substring(1);
                                }
                                else
                                {
                                    newParagraphs[tableStartedAt] += "£" + p;
                                }
                            }
                        }
                        else
                        {
                            listStartedAt = -1;
                            tableStartedAt = -1;
                            if (p.Length > 2)
                            {
                                if (p != "§\r\n")
                                {
                                    newNoOfParagraphs++;
                                    newParagraphs.Add(p);
                                }
                            }
                            if (tableStarted)
                            {
                                itHasATableHeading = false;
                                tableStarted = false;
                            }
                        }
                    }
                }
                else
                {
                    listStartedAt = -1;
                    tableStartedAt = -1;
                    if (p.Length > 2)
                    {
                        if (p != "§\r\n")
                        {
                            newNoOfParagraphs++;
                            newParagraphs.Add(p);
                        }
                    }
                    if (tableStarted)
                    {
                        itHasATableHeading = false;
                        tableStarted = false;
                    }
                }
            }


            BookIndexViewModel model = new BookIndexViewModel
            {
            Node = nodes.FirstOrDefault(),
            NoOfParagraphs = newNoOfParagraphs,
            Paragraphs = newParagraphs,
            Paragraph = "",
            HasPicture = haspict,
            NoOfPictures = pictures.Count(),
            PicturePointer = pictureptr,
            Pictures = pictures,
            PictureTitle = "",
            PictureFile = null,
            PictureFixed = picturefixed,
            HasSummary = hassumm,
            HasChildren = haschild,
            NoOfChildren = noofchildren,
            HasParent = hasparent,
            HasNoFigPara = hasnofigpara,
            HasNoTabPara = hasnotabpara,
            ShowingDetails = showingdetails,
            ShowingSummary = showingsummary,
            SearchKey = searchkey
            };

            return View(model);
        }

        public RedirectToRouteResult Up(int NodeID)
        {
            Node node = repository.Nodes
                .FirstOrDefault(n => n.NodeID == NodeID);

            Node parent = repository.Nodes
                .FirstOrDefault(n => n.NodeID == node.ParentNodeID);

            ViewBag.SelectedNodeHeading = parent.Heading;

            return RedirectToAction("Index", new {NodeID = node.ParentNodeID});
        }

        public RedirectToRouteResult Down(int NodeID, string category = null)
        {
            ViewBag.OldCategory = category;

            Node node = repository.Nodes
                .FirstOrDefault(n => n.ParentNodeID == NodeID);

            ViewBag.SelectedNodeHeading = node.Heading;

            return RedirectToAction("Index", new { NodeID = node.NodeID });
        }
	}
}