using March28HWBlogPosts.Models;
using Microsoft.AspNetCore.Mvc;

namespace March28HWBlogPosts.Controllers
{
    public class Admin : Controller
    {
        private string _connectionString = @"Data Source=10.211.55.2; Initial Catalog=BlogPosts;User Id=sa;Password=Foobar1@;TrustServerCertificate=true;";
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitPost(BlogPost blogPost)
        {
            var mgr = new BlogPostsManager(_connectionString);
            mgr.AddBlogPost(blogPost);
            return Redirect($"/home/ViewBlog?Id={blogPost.Id}");
        }
    }
}
