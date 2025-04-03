namespace March28HWBlogPosts.Models
{
    public class HomeViewModel
    {
       public IEnumerable<BlogPost> BlogPosts { get; set; }
        public decimal Page { get; set; }
        public bool TheFirstPage { get; set; }
        public bool AtTheEnd { get; set; }
       
    }
}
