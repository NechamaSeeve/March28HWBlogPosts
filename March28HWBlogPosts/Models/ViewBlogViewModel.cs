namespace March28HWBlogPosts.Models
{
    public class ViewBlogViewModel
    {
        public BlogPost BlogPost { get; set; }
        public List<Comment> Comments { get; set; }
        public string CommenterName { get; set; }
        public string[] SplitText { get; set; }
    }
}
