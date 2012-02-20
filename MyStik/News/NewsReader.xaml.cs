using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.ComponentModel;
using System.Xml;
using System.Threading;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;
using MyStik;
using Microsoft.Surface.Presentation.Controls.Primitives;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for NewsReader.xaml
    /// </summary>
    public partial class NewsReader : UserControl
    {
        private ObservableCollection<NewsItemClass> sourceItems =new ObservableCollection<NewsItemClass>();
        private ObservableCollection<NewsItemClass> targetItems = new ObservableCollection<NewsItemClass>();
        private XmlNodeList itemNodes;
        
        private SurfaceWindow1 _hauptformular;
        public SurfaceWindow1 Hauptformular
        
        {
            get { return _hauptformular; }
            set { _hauptformular = value; }
        }
        public NewsReader()
        {
            App.splashWindow.StatusUpdate("NewsReader wird initialisiert.");
            InitializeComponent();
            surfaceListBox1.DataContext = sourceItems;
            scatterView1.DataContext = targetItems;
            //Thread t = new Thread(GetNews);
            //t.Start();,
            try
            {
                GetNews("http://www.spiegel.de/schlagzeilen/index.rss");
               
            }
            catch
            { }
            //button1.IsEnabled=false;
            App.splashWindow.StatusUpdate("NewsReader ist bereit.");
            
        }
        public void SelectFeed(object sender, RoutedEventArgs e)
        {
            //Löst GetNews für den gewählten Feed ((sender as SurfaceButton).Tag) aus 
            
            try
            {
                label1.Foreground = Brushes.WhiteSmoke;
                label2.Foreground = Brushes.WhiteSmoke;
                label3.Foreground = Brushes.WhiteSmoke;
                label4.Foreground = Brushes.WhiteSmoke;
                
                if (sender == button1)
                {
                    label1.Foreground= Brushes.Black;
                    //button1.IsChecked = false;
                    button2.IsChecked = false;
                    button3.IsChecked = false;
                    button4.IsChecked = false;
                }
                if (sender == button2)
                {
                    label2.Foreground = Brushes.Black;
                    button1.IsChecked = false;
                    //button2.IsChecked = false;
                    button3.IsChecked = false;
                    button4.IsChecked = false;
                }
                if (sender == button3)
                {
                    label3.Foreground = Brushes.Black;
                    button1.IsChecked = false;
                    button2.IsChecked = false;
                    //button3.IsChecked = false;
                    button4.IsChecked = false;
                }
                if (sender == button4)
                {
                    label4.Foreground = Brushes.Black;
                    button1.IsChecked = false;
                    button2.IsChecked = false;
                    button3.IsChecked = false;
                    //button4.IsChecked = false;
                }
            }
            catch { }
            sourceItems.Clear();
            try
            {
                GetNews((sender as SurfaceToggleButton).Tag.ToString());
                
            }
            catch 
            { 
            
            }
            
        }
        public void GetNews(string feedUrl)
        {
            
            sourceItems.Clear();
            //Ladezustand (Animation, deaktivierte Bedienelemente)
            Thread t = new Thread(new System.Threading.ThreadStart(delegate()
            {
                
                this.Dispatcher.Invoke(new Action(delegate
            {
                loadAnim.Visibility = Visibility.Visible;
                button1.IsEnabled = false;
                button2.IsEnabled = false;
                button3.IsEnabled = false;
                button4.IsEnabled = false;
                surfaceListBox1.IsHitTestVisible = false;
            }));
                //Parsen des RSS feeds
                try
                {
                    XmlDocument doc = new XmlDocument();
                    
                    doc.Load(feedUrl);
                    itemNodes = doc.GetElementsByTagName("item");
                    //Erzeugen der NewsItems
                    foreach (XmlNode currentNode in itemNodes)
                    {


                        string titleString;
                        string catString;
                        string descString;
                        string imgString;
                        string urlString;
                        try
                        {
                            titleString = currentNode.SelectSingleNode("title").InnerText;
                        }
                        catch
                        {
                            titleString = "";
                        }
                        try
                        {
                            catString = currentNode.SelectSingleNode("category").InnerText;
                        }
                        catch
                        {
                            catString = "";
                        }
                        try
                        {
                            descString = currentNode.SelectSingleNode("description").InnerText;
                        }
                        catch
                        {
                            descString = "";
                        }
                        try
                        {
                            imgString = currentNode.SelectSingleNode("enclosure/@url").Value;
                        }
                        catch
                        {
                            imgString = "";
                        }
                        try
                        {
                            urlString = currentNode.SelectSingleNode("link").InnerText;
                        }
                        catch
                        {
                            urlString = "";
                        }

                        
                        this.Dispatcher.Invoke(new Action(delegate
                {
                    //Hinzufügen eines NewsItems zur Liste
                    sourceItems.Add(new NewsItemClass(titleString, catString, removeTags(descString), imgString, urlString));

                }));
                    }
                }
                catch { }
                //Beenden des Ladezustands
                this.Dispatcher.Invoke(new Action(delegate
            {
                loadAnim.Visibility = Visibility.Hidden;
                button1.IsEnabled = true;
                button2.IsEnabled = true;
                button3.IsEnabled = true;
                button4.IsEnabled = true;
                surfaceListBox1.IsHitTestVisible = true;
            }));
               
            }));
            t.Start();
            

            

        }

        private void HideSVI(object sender, RoutedEventArgs e)
        {
            //Entfernen eines NewsItems
            foreach (NewsItemClass currentItem in targetItems)
            {
                if (currentItem.Title.Equals((sender as SurfaceButton).Tag))
                {
                    targetItems.Remove(currentItem);
                    break;
                }
            }
        }
        private void ItemInit(object sender, RoutedEventArgs e)
        {
            //Initalisierung eines News Items (Animationen, Position)
            ScatterViewItem item = sender as ScatterViewItem;
            TranslateTransform offsetTransform = new TranslateTransform();
            item.RenderTransform = offsetTransform;
            Random rnd =new Random();
            item.CanScale = false;
            item.SingleInputRotationMode = SingleInputRotationMode.ProportionalToDistanceFromCenter;
            
            item.Orientation = rnd.Next(30)-15; 
            Point startPoint = new Point(1820 - item.ActualWidth / 2, 400);
            Point endPoint = new Point(840 - item.ActualWidth / 2, 450);
            item.Center = endPoint;
            FillBehavior fillBehavior = FillBehavior.Stop;
            
            PointAnimation offsetYAnimation = new PointAnimation(startPoint, endPoint, new Duration(TimeSpan.FromSeconds(.5)), fillBehavior);
            offsetYAnimation.BeginTime = TimeSpan.FromSeconds(0);
            item.BeginAnimation(ScatterViewItem.CenterProperty, offsetYAnimation);
            
            DoubleAnimation scaleAnimation = new DoubleAnimation(60, 500, new Duration(TimeSpan.FromSeconds(.5)), fillBehavior);
            scaleAnimation.BeginTime = TimeSpan.FromSeconds(0);
            item.BeginAnimation(ScatterViewItem.HeightProperty, scaleAnimation);

            
        }
        private void Show_Item(object sender, SelectionChangedEventArgs e)
        {
            //Darstellen eines Items auf der Lese-ScatterView
            bool alreadyExists = false;
            
            foreach (NewsItemClass currentItem in targetItems)
            {
                
                    if (currentItem.Equals(surfaceListBox1.SelectedItem))
                    {
                        alreadyExists = true;
                    };
                
            }
            if (!alreadyExists)
            {
                if(sourceItems.Count>0)
                {
                    targetItems.Add(surfaceListBox1.SelectedItem as NewsItemClass);
                }
            }
        }

        public void button5_Click(object sender, RoutedEventArgs e)
        {
           //Leeren der Lese-ScatterView
            targetItems.Clear();
        }

    //Regular Expression zum Entfernen von HTML-Tags aus Nachrichten-Texten(z.B. Links direkt im Text)
    const string pattern = "<.*?>";
 
    static string removeTags (string inputString)
    {
    return Regex.Replace 
    (inputString, pattern, string.Empty);
    }

    private void QRButton_Click(object sender, RoutedEventArgs e)
    {
        //Zeigt per Google API den Link zum Vollen Artikel als QR-Code an
        //um eine "Weiterleitung" auf ein Smartphone zu ermöglichen
        try
        {
            
                if ((sender as SurfaceButton).Tag.ToString().Equals(""))
                { MessageBox.Show("Kein Link vorhanden."); }
                else
                {
                    this.Hauptformular.ShowQR((sender as SurfaceButton).Tag.ToString());
                }

        }
        catch
        {
            MessageBox.Show("Kein Link vorhanden.");
        }

    }

    }
}
