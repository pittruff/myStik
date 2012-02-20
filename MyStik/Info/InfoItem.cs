using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.IO;
using System.Windows.Media.Imaging;

namespace myUserControls
{
    class InfoItem
    {
        public string fileName
        {
            get;
            private set;
        }
        public string FolderPath
    
        {
            get;
            private set;
        }
        public FileInfo fileInfo;
        public string shortName
        {
            get
            {


                return fileName.Replace(".rtf", "").Replace(".png","");
            
            }
            
        }    
        private Canvas canvas;
        private ImageSource bitmapfromstring(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
        private ImageSource bitmap;
        public string GroupName
        {
            get;
            private set;
        }
        public InfoItem(string FolderPath, string fileName, string groupName)
        {
            try
            {
                this.FolderPath = FolderPath;
                this.fileName = fileName;
                fileInfo = new FileInfo(Path.Combine(System.IO.Path.Combine(FolderPath, fileName)));
                GroupName = groupName;

                if (fileInfo.Extension.Equals(".png"))
                {
                    //this.bitmap = (new ImageSourceConverter().ConvertFromString(fileInfo.FullName)) as ImageSource;
                    this.bitmap = bitmapfromstring(new Uri(fileInfo.FullName));
                }
                else
                {
                    this.bitmap = (new ImageSourceConverter().ConvertFromString(@"Images\noun_project_450.png")) as ImageSource;
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        public InfoItem(InfoItem target)
        {
            if (target != null)
            {
                canvas = target.canvas;
                fileName = target.fileName;
                GroupName = target.GroupName;
            }
        }
        public Canvas _Canvas
        {
            get
            {
                
                
                return canvas;
            }
            
        }
        public ImageSource Bitmap
        {
            get
            {

                
                return this.bitmap;
            }
            set
            { this.bitmap = value; }

        }
    }
}
