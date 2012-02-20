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
using System.Net;
using MyStik;
using Microsoft.Surface.Presentation.Controls;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for CalendarControl.xaml
    /// </summary>
    public partial class CalendarControl : UserControl
    {


        public static int MONDAY { get { return 0; } }
        public static int TUESDAY { get { return 1; } }
        public static int WEDNESDAY { get { return 2; } }
        public static int THURSDAY { get { return 3; } }
        public static int FRIDAY { get { return 4; } }
        public static int SATURDAY { get { return 5; } }
        public static int SUNDAY { get { return 6; } }

        private string activegroupname = "";
        private string activegroupID = "";
        private int weekShift = 0;
        private int firstHour = 8;
        private int lastHour = 20;
        private SurfaceWindow1 _hauptformular;
        public SurfaceWindow1 Hauptformular
        {
            get { return _hauptformular; }
            set { _hauptformular = value; }
        }



        


        public CalendarControl()
        {
            App.splashWindow.StatusUpdate("VVK wird initialisiert.");
            InitializeComponent();


            // create rows for quater hours
            for (int i = 0; i < ((lastHour - firstHour + 1) * 4); i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.MinHeight = 8;
                cGrid.RowDefinitions.Add(rowDef);




                if (i % 4 == 0)
                {
                    Label label = new Label();
                    label.HorizontalAlignment = HorizontalAlignment.Right;
                    label.Content = (i / 4)  + firstHour + " Uhr";
                    label.Foreground = Brushes.Black;
                    label.FontSize = 10;
                    label.FontWeight = FontWeights.Bold;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(label, (i + 1) );
                    Grid.SetRowSpan(label, 4);

                    cGrid.Children.Add(label);



                    if (i % 8 == 0)
                    {
                        Rectangle rect = new Rectangle();
                        rect.Fill = Brushes.Gray;
                        rect.Opacity = 0.05;



                        Grid.SetRow(rect, (i + 1));
                        Grid.SetRowSpan(rect, 4);
                        Grid.SetColumnSpan(rect, 999);

                        cGrid.Children.Add(rect);

                    }




                }
                

            }



            //Label label = new Label();
            //label.Content = i + firstHour + " Uhr";
            //Grid.SetRow(label, i);

            //cGrid.Children.Add(label);


















            App.splashWindow.StatusUpdate("VVK ist bereit.");

        }



        public void addAppointment(int day, int fromHour, int fromMinute, int toHour, int toMinute, string name, string room, string prof, string info)
        {
            Appointment appointment = new Appointment(day, fromHour, fromMinute, toHour, toMinute, name, room, prof, info);


            

            Grid.SetColumn(appointment, day + 1);



            int row = (1 + fromHour  * 4) - (firstHour * 4);
            int hours = toHour - fromHour;
            int rowspan = hours * 4;







            if (fromMinute > 0)
            {
                row += fromMinute / 15;

                rowspan -= fromMinute / 15;

            }

            Grid.SetRow(appointment, row);

            // calculate rowSpan




            if (toMinute > 0)
            {
                rowspan += toMinute / 15;

            }





            //MessageBox.Show(minutes + "");

            //rowspan += minutes / 15;










            Grid.SetRowSpan(appointment, rowspan);

            appointment.roomButton.Click += new RoutedEventHandler(roomButton_Click);
            cGrid.Children.Add(appointment);


            //Rectangle rect = new Rectangle();
            ////rect.Fill = Shape.f


            //TextBlock text = new TextBlock();
            //Border border = new Border();
            ////border.BorderThickness = BorderThicknessProperty.g
            


        }

        void roomButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hauptformular.raumplan.findRoom((sender as SurfaceButton).Tag.ToString());
            this.Hauptformular.RollOut(this.Hauptformular.VVKGrid);
            this.Hauptformular.RollIn(this.Hauptformular.RaumplanGrid);
            this.Hauptformular.button1.IsEnabled = true;
            this.Hauptformular.button6.IsEnabled = false;
            this.Hauptformular.tb1.Foreground = Brushes.WhiteSmoke;
            this.Hauptformular.tb2.Foreground = Brushes.WhiteSmoke;
            this.Hauptformular.tb3.Foreground = Brushes.WhiteSmoke;
            this.Hauptformular.tb4.Foreground = Brushes.WhiteSmoke;
            this.Hauptformular.tb7.Foreground = Brushes.WhiteSmoke;
            this.Hauptformular.tb6.Foreground = Brushes.Black;
            this.Hauptformular.homeTB.Foreground = Brushes.WhiteSmoke;
        }

        private void groupDirectory_Initialized(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("http://interface.vorlesungsplanung.de/interface.php?format=xml&aid=1&shared_secret=sadvSe21_9&function=getgroups");
                //doc.CreateXmlDeclaration("1.0", "ISO-8859-1", "yes");
                XmlNodeList groupNodes = doc.GetElementsByTagName("group");
                foreach (XmlNode node in groupNodes)
                {
                    Label groupLabel = new Label();
                    groupLabel.Background = Brushes.Gray;
                    groupLabel.Foreground = Brushes.WhiteSmoke;
                    groupLabel.FontWeight = FontWeights.Bold;
                    groupLabel.Width = 150;
                    groupLabel.Height = 50;
                    groupLabel.FontSize = 15;
                    groupLabel.Content = node.SelectSingleNode("group_name").InnerText;
                    groupLabel.Tag = node.SelectSingleNode("group_id").InnerText;
                    groupLabel.PreviewTouchDown += new EventHandler<TouchEventArgs>(groupLabel_MouseDoubleClick);
                    groupDirectory.Items.Add(groupLabel);
                    
                }
            }
            catch
            { }
        }

        void groupLabel_MouseDoubleClick(object sender, TouchEventArgs e)
        {
            activegroupID = (sender as Label).Tag.ToString();
            activegroupname=(sender as Label).Content.ToString();
            weekShift = 0;
            previousWeek.IsEnabled = false;
            nextWeek.IsEnabled = true;
            getGroupPlan((sender as Label).Tag.ToString(), (sender as Label).Content.ToString());
        }

        public void getGroupPlan(string groupID, string groupName)
        {
            loadAnim.Visibility = Visibility.Visible;
            foreach (var oldlecture in cGrid.Children)
            {
                if (oldlecture.GetType().ToString().Equals("myUserControls.Appointment"))
                {
                    (oldlecture as Appointment).Visibility = Visibility.Hidden;

                }
            }

            DateTime nearestMonday = DateTime.Now;
            nearestMonday = nearestMonday.AddDays(7*weekShift);
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                nearestMonday = DateTime.Now;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                nearestMonday = nearestMonday.AddDays(-1);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                nearestMonday = nearestMonday.AddDays(-2);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                nearestMonday = nearestMonday.AddDays(-3);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                nearestMonday = nearestMonday.AddDays(-4);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                nearestMonday = nearestMonday.AddDays(-5);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                nearestMonday = nearestMonday.AddDays(-6);
            }
            String dateString = nearestMonday.ToShortDateString();
            dateLabel.Content = "Woche vom " + nearestMonday.ToShortDateString();
            monLabel.Content = "Montag, " + nearestMonday.ToShortDateString();
            tueLabel.Content = "Dienstag, " + nearestMonday.AddDays(1).ToShortDateString();
            wedLabel.Content = "Mittwoch, " + nearestMonday.AddDays(2).ToShortDateString();
            thuLabel.Content = "Donnerstag, " + nearestMonday.AddDays(3).ToShortDateString();
            friLabel.Content = "Freitag, " + nearestMonday.AddDays(4).ToShortDateString();
            satLabel.Content = "Samstag, " + nearestMonday.AddDays(5).ToShortDateString();
            sunLabel.Content = "Sonntag, " + nearestMonday.AddDays(6).ToShortDateString();
            //MessageBox.Show("Plan ab " + nearestMonday.ToShortDateString());
            dateString.Replace(".", "-");
            string dateday = "";
            string datemonth = "";
            if (nearestMonday.Day.ToString().Length == 1)
            {
                dateday = "0" + nearestMonday.Day.ToString();
            }
            else
            {
                dateday = nearestMonday.Day.ToString();
            }

            if (nearestMonday.Month.ToString().Length == 1)
            {
                datemonth = "0" + nearestMonday.Month.ToString();
            }
            else
            {
                datemonth = nearestMonday.Month.ToString();
            }
            dateString = nearestMonday.Year.ToString() + "-" + datemonth + "-" + dateday;
            XmlDocument doc = new XmlDocument();
            //doc.Load("http://interface.vorlesungsplanung.de/interface.php?format=xml&aid=1&shared_secret=sadvSe21_9&function=getgroupplan&group_id=126&actual_date=" + dateString);
            doc.Load("http://interface.vorlesungsplanung.de/interface.php?format=xml&aid=1&shared_secret=sadvSe21_9&function=getgroupplan&group_id=" + groupID + "&actual_date=" + dateString);
            //doc.CreateXmlDeclaration("1.0", "ISO-8859-1", "yes");
            groupNameLabel.Content = groupName;


            XmlNodeList appointmentNodes = doc.GetElementsByTagName("appointment");
            foreach (XmlNode node in appointmentNodes)
            {

                string apptDate = node.SelectSingleNode("date").InnerText;
                string fromTime = node.SelectSingleNode("timeFrom").InnerText;
                string toTime = node.SelectSingleNode("timeTo").InnerText;
                string apptYear = apptDate.Remove(4);
                string apptDay = apptDate.Remove(0, 8);
                string apptMonth = apptDate.Remove(0, 5);
                apptMonth = apptMonth.Remove(2, 3);
                string fromTimeMin = fromTime.Remove(5);
                fromTimeMin = fromTimeMin.Remove(0, 3);
                string toTimeMin = toTime.Remove(5);
                toTimeMin = toTimeMin.Remove(0, 3);

                DateTime apptDateTime = new DateTime(Convert.ToInt32(apptYear), Convert.ToInt32(apptMonth), Convert.ToInt32(apptDay));

                int day = 0;
                if (apptDateTime.DayOfWeek == DayOfWeek.Monday) { day = 0; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Tuesday) { day = 1; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Wednesday) { day = 2; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Thursday) { day = 3; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Friday) { day = 4; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Saturday) { day = 5; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Sunday) { day = 6; }
                int fromHrs = Convert.ToInt32(fromTime.Remove(2, 6));
                int fromMin = Convert.ToInt32(fromTimeMin);
                int toHrs = Convert.ToInt32(toTime.Remove(2, 6)); ;
                int toMin = Convert.ToInt32(toTimeMin);
                string title = node.SelectSingleNode("appointment_title").InnerText;
                string trainer = node.SelectSingleNode("trainer_name").InnerText;
                string room = node.SelectSingleNode("name").InnerText;
                string desc;
                if (node.SelectSingleNode("content").InnerText == null)
                {
                    desc = "";
                }
                else
                {
                    desc = node.SelectSingleNode("content").InnerText;
                }
                if (DateTime.Compare(apptDateTime, nearestMonday.AddDays(6)) < 0)
                {

                    addAppointment(day, fromHrs, fromMin, toHrs, toMin, WebUtility.HtmlDecode(title), WebUtility.HtmlDecode(room), WebUtility.HtmlDecode(trainer), WebUtility.HtmlDecode(desc));
                }
                
            }
            loadAnim.Visibility = Visibility.Hidden;
        
        }
        public void getUserPlan(string usrname, string pwd)
        {
            loadAnim.Visibility = Visibility.Visible;
            groupNameLabel.Content = "Mein Plan";
            activegroupname = "userplan";
            Cryptor cryptor2 = new Cryptor();
            
            foreach (var oldlecture in cGrid.Children)
            {
                if (oldlecture.GetType().ToString().Equals("myUserControls.Appointment"))
                {
                    (oldlecture as Appointment).Visibility = Visibility.Hidden;

                }
            }

            DateTime nearestMonday = DateTime.Now.AddDays(7*weekShift);
            //nearestMonday = nearestMonday.AddDays(+14);
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                nearestMonday = DateTime.Now;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                nearestMonday = nearestMonday.AddDays(-1);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                nearestMonday = nearestMonday.AddDays(-2);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                nearestMonday = nearestMonday.AddDays(-3);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                nearestMonday = nearestMonday.AddDays(-4);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                nearestMonday = nearestMonday.AddDays(-5);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                nearestMonday = nearestMonday.AddDays(-6);
            }
            String dateString = nearestMonday.ToShortDateString();
            dateLabel.Content = "Woche vom "+nearestMonday.ToShortDateString();
            monLabel.Content = "Montag, " + nearestMonday.ToShortDateString();
            tueLabel.Content = "Dienstag, " + nearestMonday.AddDays(1).ToShortDateString();
            wedLabel.Content = "Mittwoch, " + nearestMonday.AddDays(2).ToShortDateString();
            thuLabel.Content = "Donnerstag, " + nearestMonday.AddDays(3).ToShortDateString();
            friLabel.Content = "Freitag, " + nearestMonday.AddDays(4).ToShortDateString();
            satLabel.Content = "Samstag, " + nearestMonday.AddDays(5).ToShortDateString();
            sunLabel.Content = "Sonntag, " + nearestMonday.AddDays(6).ToShortDateString();
            //MessageBox.Show("Plan ab " + nearestMonday.ToShortDateString());
            dateString.Replace(".", "-");
            string dateday="";
            string datemonth="";
            if (nearestMonday.Day.ToString().Length == 1)
            {
                 dateday = "0" + nearestMonday.Day.ToString();
            }
            else
            {
                dateday = nearestMonday.Day.ToString();
            }

            if (nearestMonday.Month.ToString().Length == 1)
            {
                 datemonth = "0" + nearestMonday.Month.ToString();
            }
            else
            {
                 datemonth = nearestMonday.Month.ToString();
            }
            dateString=nearestMonday.Year.ToString()+"-"+datemonth+"-"+dateday;
            XmlDocument doc = new XmlDocument();
            //doc.Load("http://interface.vorlesungsplanung.de/interface.php?format=xml&aid=1&shared_secret=sadvSe21_9&function=getgroupplan&group_id=126&actual_date=" + dateString);
            //doc.Load("http://interface.vorlesungsplanung.de/interface.php?format=xml&aid=1&shared_secret=sadvSe21_9&function=getgroupplan&group_id=" + (sender as Label).Tag.ToString() + "&actual_date=" + dateString);
            //doc.CreateXmlDeclaration("1.0", "ISO-8859-1", "yes");
            try
            {
                doc.Load("http://interface.vorlesungsplanung.de/interface.php?format=xml&aid=1&shared_secret=sadvSe21_9&function=getuserplan&user=" + usrname + "&pwd=" + cryptor2.Decrypt(pwd) + "&actual_date=" + dateString);
            }
            catch { }
                //MessageBox.Show(dateString.ToString());

            XmlNodeList appointmentNodes = doc.GetElementsByTagName("appointment");
            foreach (XmlNode node in appointmentNodes)
            {

                string apptDate = node.SelectSingleNode("date").InnerText;
                string fromTime = node.SelectSingleNode("timeFrom").InnerText;
                string toTime = node.SelectSingleNode("timeTo").InnerText;
                string apptYear = apptDate.Remove(4);
                string apptDay = apptDate.Remove(0, 8);
                string apptMonth = apptDate.Remove(0, 5);
                apptMonth = apptMonth.Remove(2, 3);
                string fromTimeMin = fromTime.Remove(5);
                fromTimeMin = fromTimeMin.Remove(0, 3);
                string toTimeMin = toTime.Remove(5);
                toTimeMin = toTimeMin.Remove(0, 3);

                DateTime apptDateTime = new DateTime(Convert.ToInt32(apptYear), Convert.ToInt32(apptMonth), Convert.ToInt32(apptDay));

                int day = 0;
                if (apptDateTime.DayOfWeek == DayOfWeek.Monday) { day = 0; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Tuesday) { day = 1; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Wednesday) { day = 2; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Thursday) { day = 3; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Friday) { day = 4; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Saturday) { day = 5; }
                if (apptDateTime.DayOfWeek == DayOfWeek.Sunday) { day = 6; }
                int fromHrs = Convert.ToInt32(fromTime.Remove(2, 6));
                int fromMin = Convert.ToInt32(fromTimeMin);
                int toHrs = Convert.ToInt32(toTime.Remove(2, 6)); ;
                int toMin = Convert.ToInt32(toTimeMin);
                string title = node.SelectSingleNode("appointment_title").InnerText;
                string trainer = node.SelectSingleNode("trainer_name").InnerText;
                string room = node.SelectSingleNode("name").InnerText;
                string desc;
                if (node.SelectSingleNode("content").InnerText == null)
                {
                    desc = "";
                }
                else
                {
                    desc = node.SelectSingleNode("content").InnerText;
                }
                if (DateTime.Compare(apptDateTime, nearestMonday.AddDays(6)) < 0)
                {
                    
                    addAppointment(day, fromHrs, fromMin, toHrs, toMin, WebUtility.HtmlDecode(title), room, WebUtility.HtmlDecode(trainer), WebUtility.HtmlDecode(desc));
                }
            }
            loadAnim.Visibility = Visibility.Hidden;
        }

        public void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.Hauptformular.username.Equals(""))
            { }
            else
            {
                weekShift = 0;
                previousWeek.IsEnabled = false;
                nextWeek.IsEnabled = true;
                getUserPlan(this.Hauptformular.username, this.Hauptformular.password); 
            }
        }

        private void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hauptformular.NavLabel.Content = "Registrierung";
            if (this.Hauptformular.button1.IsEnabled == false)
            {
                this.Hauptformular.RollOut(this.Hauptformular.VVKGrid);
            }


            this.Hauptformular.button1.IsEnabled = false;
            this.Hauptformular.button2.IsEnabled = false;
            this.Hauptformular.button3.IsEnabled = false;
            this.Hauptformular.button4.IsEnabled = false;
            this.Hauptformular.button6.IsEnabled = false;
            this.Hauptformular.button7.IsEnabled = false;
            this.Hauptformular.homeButton.IsEnabled = false;
            this.Hauptformular.tb1.Foreground = Brushes.Black;
            this.Hauptformular.tb2.Foreground = Brushes.Black;
            this.Hauptformular.tb3.Foreground = Brushes.Black;
            this.Hauptformular.tb4.Foreground = Brushes.Black;
            this.Hauptformular.tb7.Foreground = Brushes.Black;
            this.Hauptformular.tb6.Foreground = Brushes.Black;
            this.Hauptformular.homeTB.Foreground = Brushes.Black;


            this.Hauptformular.RollIn(this.Hauptformular.RegistrationGrid);
            //nameBox.Text = "";
            //lastnameBox.Text = "";
            this.Hauptformular.usernameBox.Text = "";
            this.Hauptformular.passwordBox.Password = "";
        }

        private void previousWeek_Click(object sender, RoutedEventArgs e)
        {
            if (weekShift == 1)
            {
                previousWeek.IsEnabled = false;
            }
            if (weekShift > 0)
            {
                weekShift = weekShift - 1;
                if (activegroupname.Equals("userplan"))
                { getUserPlan(this.Hauptformular.username, this.Hauptformular.password); }
                else { getGroupPlan(activegroupID, activegroupname); }
            }
            else
            {
                
            }
            
        }

        private void nextWeek_Click(object sender, RoutedEventArgs e)
        {
            
            weekShift = weekShift + 1;
            previousWeek.IsEnabled = true;
            if (activegroupname.Equals("userplan"))
            { getUserPlan(this.Hauptformular.username, this.Hauptformular.password); }
            else
            {
                getGroupPlan(activegroupID, activegroupname);
            }
        }




     
    }
}
