using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Books.Domain.Abstract;
using Books.Domain.Concrete;
using Books.Domain.Entities;
using Books.WebUI.Models;

namespace Books.WebUI.Controllers
{

    public class NavController : Controller
    {

        private INodeRepository repository;

        public NavController(INodeRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(int NodeID)
        {
            List<Node> MatchingNodes;
            IEnumerable<Key> keys;
            IEnumerable<Node> siblings;
            MatchingNodes = new List<Node>();

            Node node = repository.Nodes.FirstOrDefault(n => n.NodeID == NodeID);

            if ((string)Session["SearchKey"] == "")
            {
                siblings = repository.Nodes.Where(n => n.ParentNodeID == node.ParentNodeID);
            }
            else
            {
                keys = repository.Keys.Where(k => k.TreeID == 2 && k.KeyText == (string)Session["SearchKey"]);
                MatchingNodes.Clear();
                if (keys.Count() > 0)
                {
                    foreach (Key k in keys)
                    {
                        Node n = repository.Nodes.Single(nde => nde.NodeID == k.NodeID);
                        MatchingNodes.Add(n);
                    }
                }
                siblings = MatchingNodes;
            }

            ViewBag.SelectedNodeHeading = node.Heading;

            return PartialView(siblings);
        }
    }
}