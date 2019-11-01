using System.Web.Mvc;
using Books.Domain.Entities;

namespace Books.WebUI.Infrastructure.Binders
{

    public class BookModelBinder : IModelBinder
    {
        private const string sessionKey = "Book";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {

            // get the Book from the session

            Book book = null;
            if (controllerContext.HttpContext.Session != null)
            {
                book = (Book)controllerContext.HttpContext.Session[sessionKey];
            }
            // create the Book if there wasn't one in the session data
            if (book == null)
            {
                book = new Book();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = book;
                }
            }
            // return the book
            return book;
        }
    }
}
