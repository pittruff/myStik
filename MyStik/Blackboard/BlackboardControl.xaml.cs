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
using System.Xml;
using System.Collections.ObjectModel;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Surface.Presentation.Controls.Primitives;
using System.Windows.Media.Animation;
using MyStik;
using MySql.Data.MySqlClient;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for BlackboardControl.xaml
    /// </summary>
    
    public partial class BlackboardControl : UserControl
    {
        private ObservableCollection<BlackboardItemControl> bbItems = new ObservableCollection<BlackboardItemControl>();
        private ObservableCollection<BlackboardItemControl> displayedItems = new ObservableCollection<BlackboardItemControl>();
        private SurfaceWindow1 _hauptformular;
        public SurfaceWindow1 Hauptformular
        {
            get { return _hauptformular; }
            set { _hauptformular = value; }
        }
        
        public BlackboardControl()  
        {
            App.splashWindow.StatusUpdate("Schwarzes Brett wird initialisiert.");
            InitializeComponent();
            blackboardScatterView.ItemsSource = displayedItems;
            DoubleAnimation demoAnim = new DoubleAnimation(.2, 1, new Duration(TimeSpan.FromSeconds(.7)), FillBehavior.Stop);
            demoAnim.AccelerationRatio = .5;
            demoAnim.DecelerationRatio = .5;
            demoAnim.EasingFunction = new SineEase();
            demoAnim.RepeatBehavior = RepeatBehavior.Forever;
            demoAnim.AutoReverse = true;
            demoLabel.BeginAnimation(OpacityProperty, demoAnim);
            App.splashWindow.StatusUpdate("Schwarzes Brett ist bereit.");
            
        }







        public void parseBlackboardXml(string filename)
        {
            XmlNodeList itemNodes;

            XmlDocument doc = new XmlDocument();


            try
            {
                doc.Load(filename);


                //StreamReader sr = new StreamReader();
                //sr.
            }
            catch 
            {
                //MessageBox.Show(e.Message);
            }

            itemNodes = doc.GetElementsByTagName("item");

            foreach (XmlNode node in itemNodes)
            {

                BlackboardItemControl bbc = new BlackboardItemControl();

                // parse node occuring once

                XmlNode titleNode = node.SelectSingleNode("title");
                if (titleNode != null)
                    bbc.Title = titleNode.InnerText;

                XmlNode typeNode = node.SelectSingleNode("type");
                if (typeNode != null)
                    bbc.Type = typeNode.InnerText;

                XmlNode categoryNode = node.SelectSingleNode("category");
                if (categoryNode != null)
                    bbc.Category = categoryNode.InnerText;

                XmlNode descriptionNode = node.SelectSingleNode("description");
                if (descriptionNode != null)
                    bbc.Description = descriptionNode.InnerText;

                XmlNode emailNode = node.SelectSingleNode("email");
                if (emailNode != null)
                    bbc.Email = emailNode.InnerText;

                XmlNode telNode = node.SelectSingleNode("tel");
                if (telNode != null)
                    bbc.Tel = telNode.InnerText;




                // parse nodes that might occur more often such as image and info

                bbc.Details = "";

                // run through info nodes
                XmlNodeList infoNodeList = node.SelectNodes("info");

                foreach (XmlNode infoNode in infoNodeList)
                {
                    if (!bbc.Details.Equals(""))
                        bbc.Details += "\n";

                    bbc.Details += infoNode.InnerText;
                }

                // run through image nodes
                XmlNodeList imageNodeList = node.SelectNodes("image");

                foreach (XmlNode imageNode in imageNodeList)
                    bbc.addImage(imageNode.InnerText);



                // construct contact TextBlock content
                string contactString = "";
                if (!bbc.Email.Equals(""))
                    contactString = "E-Mail: " + bbc.Email;

                if (!bbc.Tel.Equals(""))
                {
                    if (!contactString.Equals(""))
                        contactString += "\n";


                    contactString += "Telefon: " + bbc.Tel;

                }

                bbc.Contact = contactString;






                //if (bbc.Type.Equals("gebot"))
                //    bbc.BgColor = Brushes.Blue;
                //else if (bbc.Type.Equals("gesuch"))
                //    bbc.BgColor = Brushes.Green;





                // finally ...
                bbItems.Add(bbc);
               
                //foreach (BlackboardItemControl currentItem in bbItems)
                //{
                //    currentItem.IsEnabled = false;
                //}

           
            }








        }

        internal void Clear()
        {
            blackboardScatterView.Items.Clear();
        }

        internal void Populate()
        {
            parseBlackboardXml("bbitems.xml");
        }

        private void ToggleCategory(object sender, RoutedEventArgs e)
        {

            if ((sender as SurfaceToggleButton).Opacity != .2)
            {
                foreach (BlackboardItemControl currentItem in bbItems)
                {
                    if (((sender as SurfaceToggleButton).Tag.ToString().Equals(currentItem.Category)) && (currentItem.Category != null))
                    {
                        displayedItems.Add(currentItem);
                    }


                }
                (sender as SurfaceToggleButton).Opacity = .2;
            }
            else {
                (sender as SurfaceToggleButton).Opacity = 1;
                foreach (BlackboardItemControl currentItem in bbItems)
                {
                    if (((sender as SurfaceToggleButton).Tag.ToString().Equals(currentItem.Category)) && (currentItem.Category != null))
                    {
                        displayedItems.Remove(currentItem);
                    }


                }
            
            }
        }

        //private void ToggleAll(object sender, RoutedEventArgs e)
        //{
        //    if ((sender as SurfaceToggleButton).Tag != "clicked")
        //    {
        //        displayedItems.Clear();
        //        foreach (BlackboardItemControl currentItem in bbItems)
        //        {
        //            displayedItems.Add(currentItem);
        //        }
                
        //        (sender as SurfaceToggleButton).Opacity = .2;
        //        button1.Opacity = .2;
        //        button2.Opacity = .2;
        //        button3.Opacity = .2;
        //        button4.Opacity = .2;
        //        button5.Opacity = .2;
        //    }
        //    else
        //    {
        //        (sender as SurfaceToggleButton).Opacity = 1;
        //        button1.Opacity = 1;
        //        button2.Opacity = 1;
        //        button3.Opacity = 1;
        //        button4.Opacity = 1;
        //        button5.Opacity = 1;
        //        displayedItems.Clear();
        //    }
        //}

        private void AllOn(object sender, RoutedEventArgs e)
        {
            displayedItems.Clear();
            button1.IsChecked = false;
            button2.IsChecked = false;
            button3.IsChecked = false;
            button4.IsChecked = false;
            button5.IsChecked = false;
            if (allesButton.IsChecked == false)
            {
                allesButton.IsChecked = true;
            }

            if (button1.IsChecked == false)
            {
                button1.IsChecked = true;
            }
            if (button2.IsChecked == false)
            {
                button2.IsChecked = true;
            }
            if (button3.IsChecked == false)
            {
                button3.IsChecked = true;
            }
            if (button4.IsChecked == false)
            {
                button4.IsChecked = true;
            }
            if (button5.IsChecked == false)
            {
                button5.IsChecked = true;
            }
            //if (allesButton.IsChecked == false)
            //{
            //    allesButton.IsChecked = true;
            //}
            //foreach (BlackboardItemControl currentItem in bbItems)
            //{
            //    displayedItems.Add(currentItem);
            //}
        }

        private void AllOff(object sender, RoutedEventArgs e)
        {
            displayedItems.Clear();
            if (button1.IsChecked == true)
            {
                button1.IsChecked = false;
            }
            if (button2.IsChecked == true)
            {
                button2.IsChecked = false;
            }
            if (button3.IsChecked == true)
            {
                button3.IsChecked = false;
            }
            if (button4.IsChecked == true)
            {
                button4.IsChecked = false;
            }
            if (button5.IsChecked == true)
            {
                button5.IsChecked = false;
            }
            //if (allesButton.IsChecked==true)
            //{
            //    allesButton.IsChecked = false;
            //}
        }

        private void CategoryOff(object sender, RoutedEventArgs e)
        {

            
            
            foreach (BlackboardItemControl currentItem in bbItems)
            {
                if (((sender as SurfaceToggleButton).Tag.ToString().Equals(currentItem.Category)) && (currentItem.Category != null))
                {
                    displayedItems.Remove(currentItem);
                }

            }
            
        }

        private void CategoryOn(object sender, RoutedEventArgs e)
        {


            
            foreach (BlackboardItemControl currentItem in bbItems)
            {
                
                if (((sender as SurfaceToggleButton).Tag.ToString().Equals(currentItem.Category)) && (currentItem.Category != null))
                {
                    
                    
                    DoubleAnimation opacAnim= new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.5)));

                    currentItem.BeginAnimation(ScatterViewItem.OpacityProperty, opacAnim);
                    displayedItems.Add(currentItem);
                }


            }
        }

        private void likeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Hauptformular.cardReader.CardKey.Equals(""))
            {
                try
                {
                    string myInsertQuery = "INSERT INTO blackboardSurvey (cardkey) Values(@cardkey)";
                    MySqlCommand myCommand = new MySqlCommand(myInsertQuery);
                    myCommand.Parameters.AddWithValue("@cardkey", this.Hauptformular.cardReader.CardKey);
                    myCommand.Connection = this.Hauptformular.myConnection;
                    this.Hauptformular.myConnection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();
                    likeButton.IsEnabled = false;
                }
                catch { likeButton.IsEnabled = false; MessageBox.Show("Du hast bereits abgestimmt!"); }
            }
        }
    }
}
