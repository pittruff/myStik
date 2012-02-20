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
using MyStik;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Surface.Presentation.Controls.Primitives;
using System.IO;
using System.Windows.Media.Effects;
using HtmlAgilityPack;
using System.Net;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PdfToImage;
using MySql.Data.MySqlClient;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for Infos.xaml
    /// </summary>
    public partial class Infos : UserControl
    {
        public int todayID;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        DispatcherTimer datetimeTimer = new DispatcherTimer();
        ICollectionView view;
        private  ObservableCollection<InfoItem> infoItems = new ObservableCollection<InfoItem>();
        //private ObservableCollection<InfoItem> infoBackup = new ObservableCollection<InfoItem>();
        private SurfaceWindow1 _hauptformular;
        public MySqlConnection myConnection = new MySqlConnection
                                        (
                                            "UID=mystik;" +
                                            "PASSWORD=puchan@2.067;" +
                                            "SERVER=gauss.wi.hm.edu;" +
                                            "PORT=3306;" +
                                            "DATABASE=mystik;"
                                        );
        public SurfaceWindow1 Hauptformular
        {
            get { return _hauptformular; }
            set { _hauptformular = value; }
        }
        public Infos()
        {
            App.splashWindow.StatusUpdate("InfoScreen wird initialisiert.");
            InitializeComponent();
            ViewNewestDoc();

            timer.Interval = TimeSpan.FromHours(5);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            timer2.Interval = TimeSpan.FromMinutes(10);
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Start();

            datetimeTimer.Interval = TimeSpan.FromSeconds(1);
            datetimeTimer.Tick += new EventHandler(datetimeTimer_Tick);
            datetimeTimer.Start();
            
            App.splashWindow.StatusUpdate("InfoScreen ist bereit.");
            
        }
        

        void datetimeTimer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Content = DateTime.Now.ToShortTimeString();
            DateLabel.Content = DateTime.Now.ToLongDateString();
        }

        void timer2_Tick(object sender, EventArgs e)
        {
            ConvertPDFs();
            GenerateItems();
        }

        void timer_Tick(object sender, EventArgs e)
        {
           //Erneuert alle 5 Stunden die Mensa Speisekarte
            Thread t = new Thread(getMensaMenu);
           t.Start();
            

        }

        public void ConvertPDFs()
        {
            //Erzeugt Items für den Container aus den PDF Datein im Documents/Info Verzeichnis
            //Der Dateiname bestimmt den Darstellungsnamen, der Unterordner die Kategorie
            //Da PDFs hier direkt nur über ein ActiveX Steuerelement dargestellt werden könnten, welches dem User Zugriff auf Webbrowser und Dateisystem (Links im Dokument, "Speichern" Dialog) ermöglicht,
            //werden PDF Dateien vorher in ein png gerendert und diese stattdessen dargestellt.
            try
            {
                string[] PDFDeleteArray = Directory.GetFiles(@"Documents\Info\Aktuelles\", "*.pdf", SearchOption.AllDirectories);

                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = "SELECT * FROM `contentPDF`";
                MySqlDataReader Reader;
                myConnection.Open();
                Reader = command.ExecuteReader();
                WebClient wc = new WebClient();

                List<string> SQLList = new List<string>();
                while (Reader.Read())
                {

                    if (!File.Exists(@"Documents\Info\Aktuelles\" + Reader.GetValue(0) + ".pdf"))
                    {
                        try
                        {
                            wc.DownloadFileAsync(new Uri("http://gauss.wi.hm.edu/~mystik/uploadPDF/" + Reader.GetValue(0) + ".pdf"), @"Documents\Info\Aktuelles\" + Reader.GetValue(0) + ".pdf");
                        }
                        catch { }
                    }
                    SQLList.Add(Reader.GetValue(0).ToString());
                }

                myConnection.Close();
                foreach (string file in PDFDeleteArray)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    bool delete = true;
                    //MessageBox.Show(fileInfo.Name.Replace(fileInfo.Extension, ""));
                    foreach (string SQLName in SQLList)
                    {
                        if (fileInfo.Name.Replace(fileInfo.Extension, "").Equals(SQLName))
                        { delete = false; }

                    }
                    if (delete)
                    {

                        try
                        {
                            File.Delete(file);
                        }
                        catch { }
                    }
                }

                string[] PNGDeleteArray = Directory.GetFiles(@"Documents\Info\Aktuelles", "*.png", SearchOption.AllDirectories);

                foreach (string file in PNGDeleteArray)
                {
                    try
                    {
                        
                        this.Dispatcher.Invoke(new Action(delegate
              {
                  //if (infoItems.Count() > 0)
                  //{
                  //    int j = 0;
                  //    do
                  //    {
                  //        infoItems[j].Bitmap.
                  //        infoItems[j].fileInfo
                          
                  //        j++;
                  //    } while (j < infoItems.Count() - 1);
                  //}
                      infoItems = new ObservableCollection<InfoItem>();
                  //view = CollectionViewSource.GetDefaultView(infoItems);
                  //docContainer.DataContext = "docContainer";
                  //view.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                  //docContainer.ItemsSource = view;
                  
                  File.Delete(file);
              }));
                    }
                    catch (Exception e) { Console.WriteLine("deletepngs: " + e.Message); }
                }

                try
                {
                    string[] PDFArray = Directory.GetFiles(@"Documents\Info", "*.pdf", SearchOption.AllDirectories);
                    PDFConvert converter = new PDFConvert();
                    foreach (string file in PDFArray)
                    {

                        FileInfo fileInfo = new FileInfo(file);
                        if (!File.Exists(fileInfo.FullName.Replace(".pdf", ".png")))
                        {

                            converter.RenderingThreads = 1;


                            converter.TextAlphaBit = 4;
                            converter.OutputToMultipleFile = true;
                            converter.GraphicsAlphaBit = 4;
                            converter.FirstPageToConvert = 1;
                            converter.LastPageToConvert = 2;
                            converter.FitPage = true;
                            converter.ResolutionX = 200;
                            converter.ResolutionY = 200;
                            converter.OutputFormat = "png16m";

                            converter.Convert(fileInfo.FullName, fileInfo.FullName.Replace(".pdf", ".png"));
                        }
                    }
                }
                catch { }






            }
            catch (Exception e) { Console.WriteLine("CreatePDFS: "+ e.Message); }
        }
        
        public void GenerateItems()
        {
            //Erzeugt Items für den Container aus den rtf Datein im Documents/Info Verzeichnis
            //Der Dateiname bestimmt den Darstellungsnamen, der Unterordner die Kategorie
            
            try
            {
                infoItems.Clear();
                if (view != null)
                {
                    view.GroupDescriptions.Clear();
                }
            view = CollectionViewSource.GetDefaultView(infoItems);
            docContainer.DataContext = "docContainer";
            view.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            docContainer.ItemsSource = view;

            infoItems.Clear();
            foreach (string folder in Directory.GetDirectories(@"Documents\Info"))
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if(fileInfo.Extension.Equals(".rtf")||fileInfo.Extension.Equals(".png"))
                    {
                    InfoItem item = new InfoItem(folder, fileInfo.Name, folder.Replace("Documents\\Info\\", ""));
                    
                    
            infoItems.Add(item);
            //infoBackup = infoItems;
                    }
                }
            }

            }
            catch (Exception e) { Console.WriteLine("GenerateItems: " + e.Message); }   

            
        }
        

        public void ViewNewestDoc()
        {
            //Zeigt das neueste rtf an
            var newItem = new RichTextBox();
            ScatterViewItem newItemSVI = new ScatterViewItem();
            try
            {
                
               
                FileInfo currentfile = new FileInfo("Documents\\Info\\Anleitung\\Info.rtf");
                DateTime date = currentfile.LastWriteTime;
                
                foreach (string path in Directory.GetFiles("Documents\\Info","*.rtf"))
                {
                    
                    
                    FileInfo file = new FileInfo(path);
                    
                    if (file.LastWriteTime > date)
                    {
                        
                        currentfile = file;
                    
                    }
                    
                    date = currentfile.LastWriteTime;
                   
                }
                
                FileStream fs = new FileStream(currentfile.FullName, FileMode.Open, FileAccess.Read);
                TextRange RTBText = new TextRange(newItem.Document.ContentStart, newItem.Document.ContentEnd);
                RTBText.Load(fs, DataFormats.Rtf);
            }
            catch {}
            newItemSVI.Content = newItem;
            newItem.Background = Brushes.Transparent;
            newItem.IsHitTestVisible = false;
            newItemSVI.Width = 491;
            newItemSVI.Height = 700;
            
            newItemSVI.Background = Brushes.White;
            newItemSVI.IsHitTestVisible = true;
            
            newItemSVI.Orientation = 0;
            newItemSVI.Center = new Point(490,390);
            newItemSVI.CanScale = false;
            newItemSVI.CanRotate = false;
            newItemSVI.CanMove = false;
            viewer.Items.Add(newItemSVI);
            newItemSVI.PreviewTouchUp += new EventHandler<TouchEventArgs>(newItemSVI_PreviewTouchUp);
            newItemSVI.PreviewTouchDown += new EventHandler<TouchEventArgs>(newItemSVI_PreviewTouchDown);
            DropShadowEffect effect = new DropShadowEffect();
            effect.BlurRadius = 20;
            newItemSVI.Effect = effect;

            
        }
        private void ScatterView_Drop(object sender, Microsoft.Surface.Presentation.SurfaceDragDropEventArgs e)
        {
           
            //Darstellen des Dokuments das in den Viewer(ScatterView) gedropped wird.
            //Leeren des Viewers
            viewer.Items.Clear();
            //Bestimmen des Pfades des darzustellenden Dokuments aus e.Cursor.Data
            FileInfo file = new FileInfo(System.IO.Path.Combine((e.Cursor.Data as InfoItem).FolderPath, (e.Cursor.Data as InfoItem).fileName));
            //Erstellt ein ScatterViewItem als Träger für das Dokument
            ScatterViewItem newItemSVI = new ScatterViewItem();
            //Überprüfen ob es sich um ein als png gerendertes pdf handelt, oder ein rtf
            if (file.Extension.Equals(".rtf"))
            {
                //Erstellen und füllen einer RichTextBox im ScatterViewItem falls rtf
                var newItem = new RichTextBox();
                newItem.Background = Brushes.Transparent;
                FileStream fs = new FileStream(System.IO.Path.Combine((e.Cursor.Data as InfoItem).FolderPath, (e.Cursor.Data as InfoItem).fileName), FileMode.Open, FileAccess.Read);
                TextRange RTBText = new TextRange(newItem.Document.ContentStart, newItem.Document.ContentEnd);
                RTBText.Load(fs, DataFormats.Rtf);
                newItem.IsHitTestVisible = false;
                newItemSVI.Content = newItem;
                newItemSVI.Background = Brushes.White;
                //newItemSVI.BeginAnimation(ScatterViewItem.HeightProperty, animY);

                
            }
            else
            {
                //Konvertierung des Datei-strings in ein Bild im ScatterViewItem falls png (bzw. nicht rtf)
                Image image = new Image();
                //image.Source = (new ImageSourceConverter().ConvertFromString(file.FullName)) as ImageSource;
                
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(file.FullName);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            image.Source = bitmap;
        
                newItemSVI.Content = image;
                newItemSVI.Background = Brushes.Transparent;
            }

            newItemSVI.Width = 491;
            newItemSVI.Height = 700;
            
            //ImageBrush imgBrush = new ImageBrush();
            //imgBrush.ImageSource = (new ImageSourceConverter().ConvertFromString("Images\\noun_project_450.png")) as ImageSource;
            //newItemSVI.Background = imgBrush;
            
            //Erzeugen und einblenden des ScatterViewItems das das Dokument beinhaltet, zuweisen des TouchVerhaltens
            newItemSVI.IsHitTestVisible = true;
            newItemSVI.Orientation = 0;
            newItemSVI.CanScale = false;
            newItemSVI.CanRotate = false;
            newItemSVI.CanMove = false;
            DropShadowEffect effect = new DropShadowEffect();
            effect.BlurRadius = 20;
            newItemSVI.Effect = effect;
            //newItemSVI.Center = e.Cursor.GetPosition(viewer);
            newItemSVI.Center = new Point(490, 390);
            viewer.Items.Add(newItemSVI);
            //DoubleAnimation animRot = new DoubleAnimation(0, 0, new Duration(TimeSpan.FromSeconds(.5)), FillBehavior.Stop);
            DoubleAnimation animX = new DoubleAnimation(156, 491, new Duration(TimeSpan.FromSeconds(.5)), FillBehavior.Stop);
            DoubleAnimation animY = new DoubleAnimation(200, 700, new Duration(TimeSpan.FromSeconds(.5)), FillBehavior.Stop);
            PointAnimation animCenter = new PointAnimation(e.Cursor.GetPosition(viewer), new Point(490, 390), new Duration(TimeSpan.FromSeconds(.5)), FillBehavior.Stop);
            //DoubleAnimation animCenterY = new DoubleAnimation(e.Cursor.GetPosition(viewer).Y, 350, new Duration(TimeSpan.FromSeconds(.5)), FillBehavior.Stop);
            //newItemSVI.BeginAnimation(ScatterViewItem.OrientationProperty, animRot);
            newItemSVI.BeginAnimation(ScatterViewItem.WidthProperty, animX);
            newItemSVI.BeginAnimation(ScatterViewItem.HeightProperty, animY);
            newItemSVI.BeginAnimation(ScatterViewItem.CenterProperty, animCenter);
            infoItems.Remove((e.Cursor.Data as InfoItem));
            infoItems.Add(new InfoItem((e.Cursor.Data as InfoItem).FolderPath, (e.Cursor.Data as InfoItem).fileName, (e.Cursor.Data as InfoItem).GroupName));
            //MessageBox.Show((e.Cursor.Data as InfoItem).FolderPath+"\\"+(e.Cursor.Data as InfoItem).fileName);
            //GenerateItems();
            newItemSVI.PreviewTouchUp += new EventHandler<TouchEventArgs>(newItemSVI_PreviewTouchUp);
            newItemSVI.PreviewTouchDown += new EventHandler<TouchEventArgs>(newItemSVI_PreviewTouchDown);
        }

        void newItemSVI_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //Deaktivieren der MenuButtons im Moment des Berührens des dargestellten Dokuments
            _hauptformular.button1.IsEnabled = false;
            _hauptformular.button2.IsEnabled = false;
            _hauptformular.button3.IsEnabled = false;
            _hauptformular.button4.IsEnabled = false;
            _hauptformular.homeButton.IsEnabled = false;
            _hauptformular.button6.IsEnabled = false;
            _hauptformular.button7.IsEnabled = false;
        }

        void newItemSVI_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            //Vergrößern/Verkleinern des gewählten Dokuments durch TouchEvent
            
            ScatterViewItem displayedItem = (sender as ScatterViewItem);

            double targetwidth = 758;
            double targetheight = 1080;
            Point targetcenter = new Point(490, 390);

            double initialwidth = 491;
            double initialheight = 700;
            Point initialcenter = new Point(400, 390);
            double duration = .25;


            //Vergrößern, wenn Darstellung klein ist.
            if (displayedItem.Height == 700)
            {
                targetheight = 1080;
                targetwidth = 758;
                targetcenter = new Point(400, 390);

                initialwidth = 491;
                initialheight = 700;
                initialcenter = new Point(490, 390);

                displayedItem.Height = targetheight;
                displayedItem.Width = targetwidth;
                displayedItem.Center = targetcenter;
                
                //Menübuttons deaktivieren
                _hauptformular.button1.IsEnabled = false;
                _hauptformular.button2.IsEnabled = false;
                _hauptformular.button3.IsEnabled = false;
                _hauptformular.button4.IsEnabled = false;
                _hauptformular.homeButton.IsEnabled = false;
                _hauptformular.button6.IsEnabled = false;
                _hauptformular.button7.IsEnabled = false;
                
                
            }
            //Verkleiner, wenn Darstellung groß ist.
            else
            {
                targetheight = 700;
                targetwidth = 491;
                targetcenter = new Point(490, 390);

                initialwidth = 758;
                initialheight = 1080;
                initialcenter = new Point(400, 390);
                
                displayedItem.Height = 700;
                displayedItem.Width = 491;
                displayedItem.Center = new Point(490, 390);
                
                //Menüttons, bis auf den Infoscreen Button aktivieren
                _hauptformular.button1.IsEnabled = true;
                _hauptformular.button2.IsEnabled = true;
                _hauptformular.button3.IsEnabled = true;
                _hauptformular.button4.IsEnabled = true;
                _hauptformular.homeButton.IsEnabled = false;
                _hauptformular.button6.IsEnabled = true;
                _hauptformular.button7.IsEnabled = true;

            

            }
            //Animation ausführen
            DoubleAnimation animX = new DoubleAnimation(initialwidth, targetwidth, new Duration(TimeSpan.FromSeconds(duration)), FillBehavior.Stop);
            DoubleAnimation animY = new DoubleAnimation(initialheight, targetheight, new Duration(TimeSpan.FromSeconds(duration)), FillBehavior.Stop);
            PointAnimation animCenter = new PointAnimation(initialcenter, targetcenter, new Duration(TimeSpan.FromSeconds(duration)), FillBehavior.Stop);

            displayedItem.BeginAnimation(ScatterViewItem.WidthProperty, animX);
            displayedItem.BeginAnimation(ScatterViewItem.HeightProperty, animY);
            displayedItem.BeginAnimation(ScatterViewItem.CenterProperty, animCenter);
        }

        
        
        public void getMensaMenu()
        {
            //Lädt die Speisekarte der Mensa per HttpWebRequest und verarbeitet den HTML-Inhalt zu passenden strings, die in SurfaceListBoxItems dargestellt werden.
            //Einblenden der Lade-Animation
            this.Dispatcher.Invoke(new Action(delegate
                {
                    loadAnim.Visibility = Visibility.Visible;
                }));
            int i = 0;
               try{

                   HttpWebRequest myRequest =
                       (HttpWebRequest)WebRequest.Create("http://www.studentenwerk-muenchen.de/mensa/speiseplan/speiseplan_431_-de.html#heute");
                   //HttpWebRequest myRequest =
                   //    (HttpWebRequest)WebRequest.Create("http://www.studentenwerk-muenchen.de/mensa/speiseplan/speiseplan_411_-de.html#heute");
               myRequest.Method = "GET";
               myRequest.ContentType = "application/x-www-form-urlencoded";
               


               WebResponse response = myRequest.GetResponse();
               //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
               Stream responseStream = response.GetResponseStream();


               StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"), false);

               String responsestring = reader.ReadToEnd();

               HtmlDocument doc = new HtmlDocument();
               doc.LoadHtml(responsestring);
               HtmlNodeCollection menunodes = doc.DocumentNode.SelectNodes("//table[@class='menu']");
               timer.Interval = TimeSpan.FromHours(1);
               this.Dispatcher.Invoke(new Action(delegate
              {
                  //Löschen der bestehenden MensaMenu-Einträge
                  MensaMenu.Items.Clear();
              }));
               //Erstellen der neuen Einträge
                   foreach (HtmlNode currentNode in menunodes)
               {
                   
                   this.Dispatcher.Invoke(new Action(delegate
                {
                   StackPanel panel = new StackPanel();
                   string date = "";
                   string dish = "";
                   string desc = "";
                   string type = "";
                   date = currentNode.SelectSingleNode(".//strong").InnerText.Trim();
                   panel.FlowDirection = FlowDirection.LeftToRight;
                      
                   Grid grid = new Grid();
                   grid.IsHitTestVisible = false;
                   Rectangle rectangle = new Rectangle();
                   //StackPanel panel = new StackPanel(); 
                   Grid grid2 = new Grid();
                   rectangle.Fill = Brushes.Gray;
                    if (date.Contains(DateTime.Now.ToShortDateString()))
                   {
                       //MessageBox.Show(i.ToString());
                       todayID = i;
                       rectangle.Fill = Brushes.DarkGray;
                   }
                   //rectangle.RadiusX = 10;
                   //rectangle.RadiusY = 10;
                   //rectangle.Opacity = 0;
                   rectangle.Margin=new Thickness(0);
                   //DropShadowEffect effect = new DropShadowEffect();
                   //effect.BlurRadius = 20;
                   //rectangle.Effect = effect;
                   grid.Children.Add(rectangle);
                   
                   
                   panel.HorizontalAlignment = HorizontalAlignment.Center;
                   grid2.Children.Add(panel);
                   TextBlock dateLabel = new TextBlock();
                   grid.Children.Add(grid2);
                   grid2.Margin = new Thickness(10);
                   dateLabel.Foreground = Brushes.Black;

                   MensaMenu.Items.Add(grid);

                   dateLabel.FontSize = 20;
                   dateLabel.FontWeight = FontWeights.DemiBold;
                   dateLabel.Width = 250;
                   if(!date.Equals(""))
                   {
                   panel.Children.Add(dateLabel);
                   Border border1 = new Border();
                   border1.Height = 10;
                   panel.Children.Add(border1);
                   dateLabel.Text = date;
                   }
           
                   
                   

                   
                   
          
                   
                    //Html-Parsing, Füllen der strings des aktuellen Menüeintrags mit den entsprechenden Werten
                   foreach (HtmlNode dishNode in currentNode.SelectNodes(".//tr"))
                   {
                       //try
                       //{
                           if (dishNode.SelectSingleNode("./td[@class='gericht']") != null)
                           {
                               dish = dishNode.SelectSingleNode("./td[@class='gericht']").InnerText.Trim();
                           }
                       //}
                       //catch { }

                       //try
                       //{
                           if (dishNode.SelectSingleNode("./td[@class='beschreibung']/span[@style='float:left']") != null)
                           {
                               desc = dishNode.SelectSingleNode("./td[@class='beschreibung']/span[@style='float:left']").InnerText.Trim();
                           }

                       //}
                       //catch { }

                       //try
                       //{
                           if (dishNode.SelectSingleNode("./td[@class='beschreibung']/span[@class='fleisch']") != null)
                           {
                               type = dishNode.SelectSingleNode("./td[@class='beschreibung']/span[@class='fleisch']").InnerText.Replace("&nbsp;", "").Trim();
                           }
                       //}
                       //catch { }

                       //try
                       //{
                           if (dishNode.SelectSingleNode("./td[@class='beschreibung']/span[@class='fleischlos']") != null)
                           {
                               type = dishNode.SelectSingleNode("./td[@class='beschreibung']/span[@class='fleischlos']").InnerText.Replace("&nbsp;", "").Trim();
                           }
                       //}
                       //catch { }

           //Definition der Schriftelemente des Menüeintrags
                       TextBlock dishLabel = new TextBlock();
                       TextBlock descLabel = new TextBlock();
                       TextBlock typeLabel = new TextBlock();

                       dishLabel.Text = dish;
                       descLabel.Text = desc;
                       typeLabel.Text = type;
               
                       dishLabel.FontSize = 15;
                       dishLabel.Width = 250;
                       descLabel.FontSize = 15;
                       descLabel.FontWeight = FontWeights.DemiBold;
                       typeLabel.FontSize = 15;
                       typeLabel.Width = 250;
                       

                       descLabel.Width = 250;
                       descLabel.TextWrapping = TextWrapping.Wrap;
                       descLabel.Foreground = Brushes.Black;
                       dishLabel.Foreground = Brushes.WhiteSmoke;
                       typeLabel.Foreground = Brushes.WhiteSmoke;

                       if (!dish.Equals(""))
                       {
                           panel.Children.Add(dishLabel);

                           Border border2 = new Border();
                           border2.Height = 5;
                           panel.Children.Add(border2);
                       }
                       if (!desc.Equals(""))
                       {
                           panel.Children.Add(descLabel);

                           Border border3 = new Border();
                           border3.Height = 2.5;
                           panel.Children.Add(border3);
                       }
                       if (!type.Equals(""))
                       {
                           panel.Children.Add(typeLabel);


                           Border border4 = new Border();
                           border4.Height = 10;
                           panel.Children.Add(border4);
                       }
           
                       

                       
                      
                       
                       
                   

                   }
                   if (MensaMenu.Items.Count > 0)
                   {
                       //Scrollt die SurfaceListBox auf den heutigen Eintrag
                       MensaMenu.ScrollIntoView(MensaMenu.Items[todayID]);
                       
                   }
              }));
               i=i+1;
               }
               }
               catch { }
            //Ausblenden der Lade-Animation
            this.Dispatcher.Invoke(new Action(delegate
              {   
            loadAnim.Visibility = Visibility.Hidden;
              }));
        }
               

        private void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            //Scrollt die SurfaceListBox auf den heutigen Eintrag
            try
               {
                   MensaMenu.ScrollIntoView(MensaMenu.Items[todayID]); 
               }
               catch
               {}
                
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            Console.WriteLine("Converting...");
            ConvertPDFs();
            GenerateItems();
            getMensaMenu();
        }

       

       
            
            }
                
            
             
            
        
        
    
}
