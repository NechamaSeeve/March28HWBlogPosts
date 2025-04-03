using March28HWBlogPosts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace March28HWBlogPosts.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=10.211.55.2; Initial Catalog=BlogPosts;User Id=sa;Password=Foobar1@;TrustServerCertificate=true;";
        public IActionResult Index(int page = 1)
        {
            var mgr = new BlogPostsManager(_connectionString);
            var vm = new HomeViewModel();
            vm.Page = page;
            vm.TheFirstPage = vm.Page <= 1;
            IEnumerable<BlogPost> blogPosts = mgr.GetTopThreeBlogs((page - 1) * 3);
            int count = mgr.GetCount();
            vm.AtTheEnd = ((mgr.GetCount()+2) / 3) == vm.Page;
            
            vm.BlogPosts = blogPosts;
            return View(vm);
        }
        public IActionResult ViewBlog(int Id)
        {
            var mgr = new BlogPostsManager(_connectionString);
            var vm = new ViewBlogViewModel();

            vm.BlogPost = mgr.GetById(Id);
            if (vm.BlogPost == null)
            {
                return Redirect("/home/index");
            }
            vm.Comments = mgr.GetCommentsByBloggerId(vm.BlogPost.Id);
            vm.CommenterName = Request.Cookies["commeneter-Name"];
            vm.SplitText = vm.BlogPost.Text.Split("\n");
          
            return View(vm);
        }
        [HttpPost]
      public IActionResult AddComment(Comment comment)
        {
            Response.Cookies.Append("commeneter-Name", comment.Name, new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.Today.AddDays(1))
            });
            var mgr = new BlogPostsManager(_connectionString);
           
            mgr.AddComment(comment);
            return Redirect($"/home/ViewBlog?Id={comment.BlogPostId}");
        }
        public IActionResult MostRecent(int newId)
        {
            var mgr = new BlogPostsManager(_connectionString);
            newId = mgr.MostRecentBlog();
            return Redirect($"/home/ViewBlog?Id={newId}");
        }


    }
}
