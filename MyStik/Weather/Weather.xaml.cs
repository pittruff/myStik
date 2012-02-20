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
using System.Windows.Threading;

namespace myUserControls

{
    /// <summary>
    /// Interaction logic for Weather.xaml
    /// </summary>
    public partial class Weather : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        public Weather()
        {
            InitializeComponent();
            try
            {
                GetWeather();
            }
            catch { }
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromMinutes(5);
            timer.Start();
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
             try
                {
            GetWeather();
                }
             catch { }
        }
        public void GetWeather()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                
                XmlNode nodes;
                
                    doc.Load("http://www.google.com/ig/api?weather=munich&hl=en");
               
                nodes = doc.SelectSingleNode("//current_conditions");


                this.tempLabel.Content = nodes.SelectSingleNode("temp_c").Attributes[0].Value.ToString() + "°C";
                //this.conditionLabel.Content = nodes.SelectSingleNode("condition").Attributes[0].Value.ToString();
                //this.humidityLabel.Content = nodes.SelectSingleNode("humidity").Attributes[0].Value.ToString().Replace("Humidity","Luftfeuchtigkeit");
                //this.windLabel.Content = nodes.SelectSingleNode("wind_condition").Attributes[0].Value.ToString();

                sunny.Visibility = Visibility.Hidden;
                clouds.Visibility = Visibility.Hidden;
                storm.Visibility = Visibility.Hidden;
                rain.Visibility = Visibility.Hidden;
                snow.Visibility = Visibility.Hidden;

                switch ((nodes.SelectSingleNode("condition").Attributes[0].Value.ToString()).ToUpper())
                {
                    case ("CLEAR"):
                    case ("PARTLY SUNNY"):
                    case("MOSTLY SUNNY"):
                    case ("SUNNY"): sunny.Visibility = Visibility.Visible; break;
                    case("PARTLY CLOUDY"):
                    case("MOSTLY CLOUDY"):
                    case("CLOUDY"):
                    case("MIST"):
                    case("DUST"):
                    case("FOG"):
                    case("SMOKE"):
                    case("HAZE"):
                    case ("OVERCAST"): clouds.Visibility = Visibility.Visible; break;
                    case("STORM"):
                    case("FLURRIES"):
                    case("WINDY"):
                    case("THUNDERSTORM"):
                    case("CHANCE OF TSTORM"):
                    case("CHANCE OF THUNDERSTORM"):
                    case ("CHANCE OF STORM"):
                    case ("SCATTERED THUNDERSTORMS"): storm.Visibility = Visibility.Visible; break;
                    case("LIGHT RAIN"):
                    case ("RAIN"):
                    case ("CHANCE OF RAIN"):
                    case ("SCATTERED SHOWERS"):
                    case ("SHOWERS"): rain.Visibility = Visibility.Visible; break;
                    case("FREEZING DRIZZLE"):
                    case ("RAIN AND SNOW"):
                    case ("SNOW"):
                    case("ICY"):
                    case("SLEET"):
                    case ("LIGHT SNOW"): 
                    case("SNOW SHOWERS"):
                    case ("ICE/SNOW"):
                    case ("SCATTERED SNOW SHOWERS"):
                    case ("CHANCE OF SNOW"): snow.Visibility = Visibility.Visible; break;

                    default: sunny.Visibility = Visibility.Visible; break;

                }
            }

            catch { }
             
              
        
        
        }
    }
}
