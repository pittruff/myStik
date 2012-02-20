using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myUserControls
{
    public class NewsItemClass
    {
        private string title;
        private string category;
        private string description;
        private string image;
        private string url;

        
        
        public NewsItemClass(string title, string category, string description, string image, string url)
        {
            this.title = title;
            this.category = category;
            this.description = description;
            this.image = image;
            this.url = url;
            

        }
        

        public string Title
        {
            get { return title; }
        }

        public string Category
        {
            get { return category; }
        }

        public string Description
        {
            get { return description; }
        }
        public string Image
        {
            get { return image; }
        }
        public string Url
        {
            get { return url; }
        }
    }
}
