using System.Collections.Generic;
using Books.Domain.Concrete;
using Books.WebUI.Infrastructure;

namespace Books.WebUI.Models
{
    public class AdminEditViewModel
    {
        public Node Node { get; set; }
        public bool Format { get; set; }
    }
}