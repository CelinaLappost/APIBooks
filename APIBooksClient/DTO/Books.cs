using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIBooksClient.DTO
{
    public class Books
    {
        public int id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int PageCount { get; set; }

        public string Excerpt { get; set; }

        public DateTime PublishDate { get; set; }

        public Books()
        {

        }

        public Books(int ID, string TITLE, string DESCRIPTION, int PAGECOUNT, string EXCERPT, DateTime PUBLISHDATE)
        {
            id = ID;
            Title = TITLE;
            Description = DESCRIPTION;
            PageCount = PAGECOUNT;
            Excerpt = EXCERPT;
            PublishDate = PUBLISHDATE;
        }
    }
}