using System;

namespace DbProject
{
    public class BlogPost
    {
        public BlogPost()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public int Number { get; set; }
    }
}