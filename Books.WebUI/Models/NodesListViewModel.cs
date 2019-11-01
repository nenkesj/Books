using System.Collections.Generic;
using Books.Domain.Concrete;

namespace Books.WebUI.Models
{
    public class NodesListViewModel
    {
        public IEnumerable<Node> Nodes { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public bool Thumbnail { get; set; }
    }
}