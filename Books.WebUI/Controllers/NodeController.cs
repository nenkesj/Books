using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Books.Domain.Abstract;
using Books.Domain.Concrete;
using Books.WebUI.Models;

namespace Books.WebUI.Controllers
{
    public class NodeController : Controller
    {
        private INodeRepository repository;
        public int PageSize = 4; 

        public NodeController(INodeRepository nodeRepository) {
            this.repository = nodeRepository;
        }


        public ViewResult List(int page = 1) 
        {
            IEnumerable<Node> n;

            n = repository.Nodes
                .Where(p => p.TreeLevel == 1) 
                .OrderBy(p => p.Heading)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            NodesListViewModel model = new NodesListViewModel {
            Nodes = n,
            PagingInfo = new PagingInfo {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = repository.Nodes.Where(p => p.TreeLevel == 1).Count()
                },
            Thumbnail = true

            };
            return View(model);
        }
	}
}