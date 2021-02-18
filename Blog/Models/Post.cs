using System;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string Image { get; set; } = "";
        public int PostViews { get; set; } = 0;
        public int NewVal { get; set; } = 0;

        public DateTime Created { get; set; } = DateTime.Now;

    }
}
