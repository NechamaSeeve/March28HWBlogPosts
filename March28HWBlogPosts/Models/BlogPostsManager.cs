using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;

namespace March28HWBlogPosts.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public DateTime DateSubmitted { get; set; }
    }
    public class Comment
    {
        public int Commenterid { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public int BlogPostId { get; set; }
        
    }

    public class BlogPostsManager
    {
        private readonly string _connecionString;
        public BlogPostsManager(string connectionString)
        {
            _connecionString = connectionString;
        }
        

        public List<BlogPost> GetTopThreeBlogs(int num)
        {
            using var connecion = new SqlConnection(_connecionString);
            using var cmd = connecion.CreateCommand();
            cmd.CommandText = $@"select * FROM BlogPosts 
                   order by DateSubmitted Desc,id Desc
                  OFFSET {num} ROWS 
                   FETCH NEXT 3 ROWS ONLY ";
            connecion.Open();
            
            var reader = cmd.ExecuteReader();
            List<BlogPost> blogs = new();
            while (reader.Read())
            {
                blogs.Add(new()
                {
                    Id = (int)reader["Id"],
                    Text = (string)reader["Text"],
                    Title = (string)reader["Title"],
                    DateSubmitted = (DateTime)reader["DateSubmitted"]


                });

            }
            return blogs;

        }
        public int GetCount()
        {
            using var connecion = new SqlConnection(_connecionString);
            using var cmd = connecion.CreateCommand();
            cmd.CommandText = "Select Count(*) From BlogPosts";
            connecion.Open();
            int count = (int)cmd.ExecuteScalar();
            return count;
        }
        public BlogPost GetById(int Id)
        {
            using var connecion = new SqlConnection(_connecionString);
            using var cmd = connecion.CreateCommand();
            cmd.CommandText = "select * From BlogPosts Where Id = @id";
            cmd.Parameters.AddWithValue("@id", Id);
            connecion.Open();
            var reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }
            BlogPost blogPost = new BlogPost
            {
                Id = (int)reader["id"],
                Text = (string)reader["Text"],
                Title = (string)reader["Title"],
                DateSubmitted = (DateTime)reader["DateSubmitted"],
            };
            return blogPost;

        }
        public void AddComment(Comment comment)
        {
            using var connecion = new SqlConnection(_connecionString);
            using var cmd = connecion.CreateCommand();
            cmd.CommandText = "Insert into Comments(Text,CommenterName,BlogPostId) " +
                "Values(@text,@name, @id)";
            cmd.Parameters.AddWithValue("@text", comment.Text);
            cmd.Parameters.AddWithValue("@name", comment.Name);
            cmd.Parameters.AddWithValue("@id", comment.BlogPostId);
            
            connecion.Open();
            cmd.ExecuteNonQuery();
        }
        public List<Comment> GetCommentsByBloggerId(int id)
        {
            using var connecion = new SqlConnection(_connecionString);
            using var cmd = connecion.CreateCommand();
            cmd.CommandText = "Select * from Comments Where BlogPostId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connecion.Open();
            var reader = cmd.ExecuteReader();
            List<Comment> comments = new();
            while (reader.Read())
            {
                comments.Add(new()
                {
                    Commenterid = (int)reader["Id"],
                    Name = (string)reader["CommenterName"],
                    Text = (string)reader["Text"],
                    BlogPostId = (int)reader["Id"],
                   


                });
            }
            return comments;
        }
        public void AddBlogPost(BlogPost blogPost)
        {
            using var connecion = new SqlConnection(_connecionString);
            using var cmd = connecion.CreateCommand();
            cmd.CommandText = "Insert into BlogPosts(Text,Title,DateSubmitted) " +
                "Values(@text,@title, @date); SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@text", blogPost.Text);
            cmd.Parameters.AddWithValue("@title", blogPost.Title);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            connecion.Open();
            blogPost.Id = (int)(decimal)cmd.ExecuteScalar();
            
        }
        public int MostRecentBlog()
        {
            using var connecion = new SqlConnection(_connecionString);
            using var cmd = connecion.CreateCommand();
            cmd.CommandText = "Select Top (1) id from BlogPosts Order By datesubmitted desc, id Desc";
            connecion.Open();
            int newId = (int)cmd.ExecuteScalar();
            return newId;
        }
    }
}
