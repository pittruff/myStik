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
using System.Threading;
using System.Net;
using System.IO;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using HtmlAgilityPack;
using MyStik;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for Fahrplan.xaml
    /// </summary>
    public partial class Fahrplan : UserControl
    {
        
        Thread t;
        public Fahrplan()
        {
            App.splashWindow.StatusUpdate("Fahrplan wird initialisiert.");
            InitializeComponent();
            
            Request("Lothstraße", new RoutedEventArgs());
            Thread t2 = new Thread(new System.Threading.ThreadStart(delegate()
            {
                do
                {
                    try
                    {
                        HttpWebRequest myRequest =
                      (HttpWebRequest)WebRequest.Create("http://www.newsletter-s-bahn-muenchen.de/db_newsticker.php");
                        myRequest.Method = "GET";
                        myRequest.ContentType = "application/x-www-form-urlencoded";



                        WebResponse response = myRequest.GetResponse();
                        //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                        Stream responseStream = response.GetResponseStream();


                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("ISO-8859-1"), false);

                        String responsestring = reader.ReadToEnd();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(responsestring);
                        HtmlNodeCollection tickernodes = doc.DocumentNode.SelectNodes("//td");
                        foreach (HtmlNode currentNode in tickernodes)
                        {
                            if (currentNode.InnerText != "")
                            {
                                StringBuilder str = new StringBuilder();
                                //MessageBox.Show(currentNode.InnerText);
                                str.Append(currentNode.InnerText);
                                //str.Remove(str.Length - 2,2);
                                //MessageBox.Show(str.ToString());
                                HtmlDocument subdoc = new HtmlDocument();

                                subdoc.LoadHtml(str.ToString());

                                //MessageBox.Show(subdoc.DocumentNode.InnerText);
                                this.Dispatcher.Invoke(new Action(delegate
                    { ticker.Text = (new HTMLCharacterConverter().Purge(subdoc.DocumentNode.InnerText)); }));

                            }
                        }
                    }
                    catch { }
                    Thread.Sleep(600000);
                    
                } while (0 == 0);

            }));

            t2.Start();
            App.splashWindow.StatusUpdate("Fahrplan ist bereit.");
        }

        private SurfaceWindow1 mainwindow;
        public SurfaceWindow1 Mainwindow
        {
            set { mainwindow = value; }
            get { return mainwindow; }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void RequestHbf(object sender, RoutedEventArgs e)
        {
            //Unterbricht den aktuellen Request und fordert einen neuen an
            try
            {
                t.Suspend();
                loadAnim.Visibility = Visibility.Hidden;
                
            }
            catch { }
            Request("Hauptbahnhof", e);
        }
        public void RequestLothStr(object sender, RoutedEventArgs e)
        {
            //Unterbricht den aktuellen Request und fordert einen neuen an
            try
            {
                t.Suspend();
                loadAnim.Visibility = Visibility.Hidden;
            }
            catch { }
            Request("Lothstraße", e);
        }
        private void RequestCustom(object sender, RoutedEventArgs e)
        {
            //Unterbricht den aktuellen Request und fordert einen neuen an
            try
            {
                t.Suspend();
                loadAnim.Visibility = Visibility.Hidden;
            }
            catch { }
            Request(null, e);
            
        }
        private void Request(string destination, RoutedEventArgs e)
        {
            //Parst den Fahrplan für ausgehende Verbindungen von der Haltestelle aus dem übergebenen string
           
                t = new Thread(new System.Threading.ThreadStart(delegate()
                {
                    
                    try
                    {
                        do
                        {
                        string strorig = "";

                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        this.destlabel.Content = "Verbindungen ab " + destination;
                        this.surfaceButton1.Content = "Bitte warten...";
                        this.Dispatcher.Invoke(new Action(delegate
                        {

                            loadAnim.Visibility = Visibility.Visible;
                        }));
                        if (destination == null)
                        {
                            strorig = this.start.Text;
                            this.destlabel.Content = "Verbindungen ab " + strorig;
                        }
                        else
                        {
                            strorig = destination;
                        }
                    }));
                    
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            surfaceListBox1.Items.Clear();
                            
                        }));
                        string postData = "haltestelle=" + strorig + "&ubahn=checked&bus=checked&tram=checked&sbahn=checked";

                    byte[] data = Encoding.GetEncoding("ISO-8859-1").GetBytes(postData);

                    // Prepare web request...
                    HttpWebRequest myRequest =
                      (HttpWebRequest)WebRequest.Create("http://www.mvg-live.de/ims/dfiStaticAuswahl.svc");
                    myRequest.Method = "POST";
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                    myRequest.ContentLength = data.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();

                    WebResponse response = myRequest.GetResponse();
                    //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    Stream responseStream = response.GetResponseStream();


                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("ISO-8859-1"), false);

                    String responsestring = reader.ReadToEnd();
                    //MessageBox.Show(responsestring);


                    HtmlDocument doc = new HtmlDocument();
                    //Console.WriteLine(responsestring);
                    doc.LoadHtml(responsestring);
                    reader.Close();
                    response.Close();
                    responseStream.Close();

                    HtmlNodeCollection divNodes = doc.DocumentNode.SelectNodes("//tr[@class='rowOdd'] | //tr[@class='rowEven' ]");
                    HtmlNodeCollection selectNodes = doc.DocumentNode.SelectNodes("//a[@href]");

                    if (divNodes != null)
                    {
                        //Fügt die einzelnen Einträge in die Liste ein
                        foreach (HtmlNode node in divNodes)
                        {

                            string lineNumber = node.SelectSingleNode("./td[@class='lineColumn']").InnerText;
                            string lineName = node.SelectSingleNode("./td[@class='stationColumn']").InnerText;
                            //MessageBox.Show(lineName);
                            string lineMinutes = node.SelectSingleNode("./td[@class='inMinColumn']").InnerText;
                            FahrplanItem item = new FahrplanItem(lineNumber, new HTMLCharacterConverter().Purge(lineName.Trim()), "In " + lineMinutes + " Min.");
                            this.Dispatcher.Invoke(new Action(delegate
                { this.surfaceListBox1.Items.Add(item); }));

                            //this.Dispatcher.Invoke(new Action(delegate
                            //{
                            //    if (start.Text != null)
                            //    {

                            //        //start.IsDropDownOpen = true;
                            //    }
                            //}));
                        }
                    }



                    //Zeigt Klickbare einträge der Verbesserungsvorschläge an, falls die Eingabe nicht gefunden wurde
                    foreach (HtmlNode node in selectNodes)
                    {
                        if (!(node.InnerText.Trim().Equals("Fahrten aktualisieren")) && !(node.InnerText.Trim().Equals("Impressum")))
                        {
                            FahrplanItem item = new FahrplanItem("", node.InnerText.Trim(), "");
                            
                            this.Dispatcher.Invoke(new Action(delegate
                            { 
                                this.surfaceListBox1.Items.Add(item);
                                this.destlabel.Content = "Haltestelle wählen:";
                                this.Dispatcher.Invoke(new Action(delegate
                                {

                                    loadAnim.Visibility = Visibility.Hidden;
                                }));
                                t.Suspend();
                            }));
                            

                        }
                    }

                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        this.Dispatcher.Invoke(new Action(delegate
                        {

                            loadAnim.Visibility = Visibility.Hidden;
                        }));
                        this.surfaceButton1.Content = "Absenden";
                    }));
                    Thread.Sleep(60000);
                    } while (1 == 1);
                    }
            catch { }
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        
                        loadAnim.Visibility = Visibility.Hidden;
                    }));
                }));
                t.Start();
            
                
            
        }

        

        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            
            resetPlans();
        }
        public void resetPlans()
        {
            tram.Height = 320;
            bus.Height = 320;
            sbahn.Height = 320;
            tram.Width = 450;
            bus.Width = 450;
            sbahn.Width = 450;

            tram.Orientation = 10;
            bus.Orientation = 5;
            sbahn.Orientation = 0;

            sbahn.Center = new Point(500, 400);
            bus.Center = new Point(550, 450);
            tram.Center = new Point(600, 500);
        }

        private void start_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            
            this.Mainwindow.Call_Keyboard(sender as TextBox, e);
        }

        private void start_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Mainwindow.Call_Keyboard(sender as TextBox, e);
        }

        

        private void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {

            Request((sender as SurfaceButton).Content.ToString(), e);
        }

        private void start_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                loadAnim.Visibility = Visibility.Hidden;
                t.Suspend();
                RequestCustom(null, null);
                
            }
        }


        
    }
}
