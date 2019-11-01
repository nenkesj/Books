using System.Collections.Generic;
using Books.Domain.Concrete;
using Books.WebUI.Infrastructure;

namespace Books.WebUI.Models
{
    public class PictureIndexViewModel
    {
        public Node Node { get; set; }
        public bool HasPicture { get; set; }
        public int NoOfPictures { get; set; }
        public IEnumerable<Picture> Pictures { get; set; }
        public int PicturePointer { get; set; }
        public bool Caption { get; set; }
        public bool Thumbnail { get; set; }
    }
}