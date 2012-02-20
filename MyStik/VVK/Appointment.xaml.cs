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
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for Appointment.xaml
    /// </summary>
    public partial class Appointment : UserControl
    {
        int day;
        int fromHour;
        int fromMinute;
        int toHour;
        int toMinute;
        bool wasClicked;
        





        public Appointment(int day, int fromHour, int fromMinute, int toHour, int toMinute, string vname, string room, string prof, string info)
        {
            this.day = day;
            this.fromHour = fromHour;
            this.fromMinute = fromMinute;
            this.toHour = toHour;
            this.toMinute = toMinute;

            VName = vname;
            Room = room;
            Prof = prof;
            VInfo = info;




            string fromMinuteString = fromMinute.ToString();
            if (fromMinuteString.Length == 1)
                fromMinuteString = "0" + fromMinuteString;


            string toMinuteString = toMinute.ToString();
            if (toMinuteString.Length == 1)
                toMinuteString = "0" + toMinuteString;

            
            //FormatedTime = fromHour + ":" + fromMinute + " Uhr - " + toHour + ":" + toMinute + " Uhr";
            FormatedTime = fromHour + ":" + fromMinuteString + " - " + toHour + ":" + toMinuteString + " Uhr";




            //this.room = room;
            //this.prof = prof;

            InitializeComponent();



            //OnPropertyChanged("FormatedTime");
        }



        //public string FormatedTime
        //{
        //    get
        //    {
        //        string formatedTime = fromHour + ":" + fromMinute + " Uhr - " + toHour + ":" + (toMinute * 5) + " Uhr";
        //        return formatedTime;

        //    }

        //}



        //FormatedTimeProperty
        public static DependencyProperty FormatedTimeProperty = DependencyProperty.Register("FormatedTime", typeof(string), typeof(Appointment));

        public string FormatedTime
        {
            get { return (string)GetValue(FormatedTimeProperty); }
            set { SetValue(FormatedTimeProperty, value); }
        }

        public bool WasClicked
        {
            get { return this.wasClicked; }
            set { this.wasClicked = value; }
        }




        //VNameProperty
        public static DependencyProperty VNameProperty = DependencyProperty.Register("VName", typeof(string), typeof(Appointment));

        public string VName
        {
            get { return (string)GetValue(VNameProperty); }
            set { SetValue(VNameProperty, value); }
        }




        //RoomProperty
        public static DependencyProperty RoomProperty = DependencyProperty.Register("Room", typeof(string), typeof(Appointment));
        
        public string Room
        {
            get { return (string)GetValue(RoomProperty); }
            set { SetValue(RoomProperty, value);  }        
        }

        // ProfProperty
        public static DependencyProperty ProfProperty = DependencyProperty.Register("Prof", typeof(string), typeof(Appointment));

        public string Prof
        {
            get { return (string)GetValue(ProfProperty); }
            set { SetValue(ProfProperty, value); }
        }






        //InfoProperty
        public static DependencyProperty VInfoProperty = DependencyProperty.Register("VInfo", typeof(string), typeof(Appointment));

        public string VInfo
        {
            get { return (string)GetValue(VInfoProperty); }
            set { SetValue(VInfoProperty, value); }
        }

        private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void VvkAppointment_PreviewMouseDown(object sender, TouchEventArgs e)
        {
            if ((sender as Appointment).WasClicked)
            {

                TranslateTransform transform2 = new TranslateTransform();
                DoubleAnimation da2 = new DoubleAnimation();
                (sender as Appointment).RenderTransform = transform2;
                da2.From = 120;
                da2.To = 0;
                da2.Duration = TimeSpan.FromSeconds(.5);
                transform2.BeginAnimation(TranslateTransform.XProperty, da2);
                WasClicked = false;
                DoubleAnimation da3 = new DoubleAnimation();
                da3.From = .5;
                da3.To = 1;
                da3.Duration = TimeSpan.FromSeconds(.5);
                (sender as Appointment).BeginAnimation(Appointment.OpacityProperty, da3);
            }
            else
            { 
            TranslateTransform transform = new TranslateTransform();
            DoubleAnimation da = new DoubleAnimation();
            (sender as Appointment).RenderTransform=transform;
            da.From=0;
            da.To=120;
            da.Duration = TimeSpan.FromSeconds(.5);
            transform.BeginAnimation(TranslateTransform.XProperty, da);
            DoubleAnimation da4 = new DoubleAnimation();
            da4.From = 1;
            da4.To = .5;
            da4.Duration = TimeSpan.FromSeconds(.5);
            (sender as Appointment).BeginAnimation(Appointment.OpacityProperty, da4);
                (sender as Appointment).WasClicked = true;
            }
        }

       









    }
}
