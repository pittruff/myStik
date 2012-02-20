using System.Windows.Threading;
using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading;
namespace myUserControls
{
    public partial class CrawlTextBox
    {

        
        public CrawlTextBox()
        {
            this.InitializeComponent();
            crawlLabel.UpdateLayout();
            TranslateTransform offsetTransform = new TranslateTransform();

            DoubleAnimation offsetXAnimation = new DoubleAnimation(1920, -(crawlLabel.ActualWidth), new Duration(TimeSpan.FromSeconds(30*(crawlLabel.ActualWidth/5000))));
            offsetXAnimation.RepeatBehavior = RepeatBehavior.Forever;
            offsetXAnimation.BeginTime = TimeSpan.FromSeconds(0);
            offsetTransform.BeginAnimation(TranslateTransform.XProperty, offsetXAnimation);
            

            crawlLabel.RenderTransform = offsetTransform;
        }
        public void CrawlText(string crawltext)
        {
           
            crawlLabel.Content = crawltext;
            crawlLabel.UpdateLayout();
            crawlLabel.BeginInit();
            this.InitializeComponent();
            TranslateTransform offsetTransform = new TranslateTransform();
            //MessageBox.Show(crawlLabel.Content.ToString());

            DoubleAnimation offsetXAnimation = new DoubleAnimation(1920, -(crawlLabel.ActualWidth), new Duration(TimeSpan.FromSeconds(30 * (crawlLabel.ActualWidth / 5000))));
            offsetXAnimation.RepeatBehavior = RepeatBehavior.Forever;
            offsetXAnimation.BeginTime = TimeSpan.FromSeconds(0);
            offsetTransform.BeginAnimation(TranslateTransform.XProperty, offsetXAnimation);


            crawlLabel.RenderTransform = offsetTransform;
        }
        // This will contain the Displaying text
        private string displayString;
        public string DisplayString { get { return displayString; } set { displayString = value; } }


    }

}