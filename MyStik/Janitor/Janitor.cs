using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyStik;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows;

namespace myUserControls
{
    public class Janitor
    {
        
        
        
        public bool ScreenSaverRuns = false;
        private SurfaceWindow1 _hauptformular;
        public SurfaceWindow1 Hauptformular
        {
            get { return _hauptformular; }
            set { _hauptformular = value; }
        }
        public DispatcherTimer timer = new DispatcherTimer();
        
        public void CleanUp()
        {
            
            timer.Interval = TimeSpan.FromMinutes(5);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            
           
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            ResetEverything();
        }
       
        public void ResetEverything()
        {
            


            //reset Info-Screen
            Hauptformular.infos.viewer.Items.Clear();
            
            Hauptformular.infos.ViewNewestDoc();
            
            Hauptformular.infos.GenerateItems();
            Hauptformular.infos.ConvertPDFs();
            try
            {
                Hauptformular.infos.MensaMenu.ScrollIntoView(Hauptformular.infos.MensaMenu.Items[Hauptformular.infos.todayID]);
            }
            catch { }
            //reset VVK-Screen
            foreach (var oldlecture in Hauptformular.calendarControl1.cGrid.Children)
            {
                if (oldlecture.GetType().ToString().Equals("myUserControls.Appointment"))
                {
                    (oldlecture as Appointment).Visibility = Visibility.Hidden;

                }
            }
            Hauptformular.calendarControl1.groupNameLabel.Content = "Keine Gruppe gewählt";
            Hauptformular.calendarControl1.nextWeek.IsEnabled = false;
            //reset Blackboard-Screen
            Hauptformular.blackboardControl.allesButton.IsChecked = false;
            //reset News-Screen
            Hauptformular.newsReader.button1.IsChecked = true;
            Hauptformular.newsReader.button5_Click(null, null);
            //reset MMV-Screen
            Hauptformular.fahrplan.RequestLothStr(null, null);
            Hauptformular.fahrplan.resetPlans();
            //reset Raumplan
            Hauptformular.raumplan.SurfaceButton_Click_1(null, null);
            //close all games
            foreach (Process clsProcess in Process.GetProcesses())
            {


                if (clsProcess.ProcessName.StartsWith("WindowsGame2"))
                {
                    clsProcess.Kill();


                }
                if (clsProcess.ProcessName.StartsWith("Airhockey"))
                {
                    clsProcess.Kill();


                }
                if (clsProcess.ProcessName.StartsWith("AnimatedSpriteWindows"))
                {
                    clsProcess.Kill();


                }
            }
           

            if (!ScreenSaverRuns)
            {
                
                Screensaver();
                ScreenSaverRuns = false;
            }
            
        }
        
        void Screensaver()
        {


            this.Hauptformular.Activate();
            this.Hauptformular.ScreensaverGrid.Children.Clear();
            
            Screensaver _screensaver = new Screensaver();
            _screensaver.ParentJanitor = this;
            this.Hauptformular.ScreensaverGrid.Children.Add(_screensaver);
            ScreenSaverRuns = true;
        }
    }
}
