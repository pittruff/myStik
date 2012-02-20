using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows;

namespace MyStik
{
    public class CardReader
    {
        public SerialPort serialPort;
        string cardKey;
        string status;
        
        public CardReader()
        {
            App.splashWindow.StatusUpdate("CardReader wird initialisiert.");
            try
            {
                //Thread t = new Thread(checkCard);
                //t.Start();
                serialPort=new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
                serialPort.Handshake = Handshake.None;
                serialPort.Encoding = Encoding.ASCII;
                serialPort.ReadTimeout = 500;
                serialPort.WriteTimeout = 500;

            }
            catch {}
            App.splashWindow.StatusUpdate("CardReader ist bereit");


        }
        
        private SurfaceWindow1 _hauptformular;
        public SurfaceWindow1 Hauptformular
        {
            get { return _hauptformular; }
            set { _hauptformular = value; }
        }
        public string CardKey
        {
            get { return cardKey; }
            
        }
        public string Status
        {
            get { return status; }
        }

        public void checkCard()
        {
            
                int exit = 0;
                bool loggedIn = new bool();
                string initialKey = null;
                bool runCheck = true;
                Thread.Sleep(500);
                do
                {
                    try
                    {

                    if (!(loggedIn))
                    {

                        do
                        {

                            if (!(serialPort.IsOpen))
                            {
                                do
                                {
                                    
                                  
                                        serialPort.Open();
                                    
                                    
                                }
                                while (!serialPort.IsOpen);
                            }

                            serialPort.WriteLine("s\r\n");
                            Thread.Sleep(300);
                            initialKey = serialPort.ReadExisting();
                            if (initialKey.Equals("E\r"))
                            {
                                status = "Warte auf Karte";
                                this.Hauptformular.statusLabel.Dispatcher.Invoke(new Action(delegate { this.Hauptformular.statusLabel.Content = status; }));
                            }
                            else
                            {
                                //MessageBox.Show(initialKey);
                                serialPort.WriteLine("b01\r\n");
                                Thread.Sleep(300);
                                serialPort.ReadExisting();
                            }
                            serialPort.WriteLine("e20\r\n");
                            Thread.Sleep(300);
                            serialPort.ReadExisting();
                            serialPort.WriteLine("e11\r\n");
                            Thread.Sleep(300);
                            serialPort.ReadExisting();
                        } while (initialKey.Equals("E\r"));
                    }
                    cardKey = initialKey;

                    if (!(serialPort.IsOpen)) {

                        do
                        {
                            serialPort.Open();
                            
                        }
                        while (!serialPort.IsOpen);
                    
                    
                    }


                    serialPort.WriteLine("s\r\n");
                    Thread.Sleep(300);
                    string currentKey = serialPort.ReadExisting();


                    loggedIn = true;
                    if (!(initialKey.Equals(currentKey)))
                    {

                        serialPort.WriteLine("e22\r\n");
                        Thread.Sleep(300);
                        serialPort.ReadExisting();
                        exit = exit + 1;
                        status = "Karte wurde entfernt! Logout in " + (3 - exit);
                        this.Hauptformular.statusLabel.Dispatcher.Invoke(new Action(delegate { this.Hauptformular.statusLabel.Content = status; }));

                    }
                    else
                    {
                        cardKey = currentKey;
                        this.Hauptformular.statusLabel.Dispatcher.Invoke(new Action(delegate { this.Hauptformular.statusLabel.Content = "Eingeloggt"; }));
                        exit = 0;
                        this.Hauptformular.userLogin();

                    }
                    if (exit > 2)
                    {
                        serialPort.WriteLine("b00\r\n");
                        Thread.Sleep(300);
                        serialPort.ReadExisting();
                        serialPort.WriteLine("e21\r\n");
                        Thread.Sleep(300);
                        serialPort.ReadExisting();
                        serialPort.WriteLine("e10\r\n");
                        Thread.Sleep(300);
                        serialPort.ReadExisting();
                        exit = 0;
                        loggedIn = false;
                        status = "User wurde ausgeloggt";
                        this.Hauptformular.statusLabel.Dispatcher.Invoke(new Action(delegate { this.Hauptformular.statusLabel.Content = status; }));
                        cardKey = "";
                        this.Hauptformular.userLogout();
                        //break;
                    }

                    if (initialKey.Equals(currentKey) == false && currentKey.Equals("E\r") == false)
                    {
                        //runCheck = false;
                        status = "Falsche Karte";
                        this.Hauptformular.statusLabel.Dispatcher.Invoke(new Action(delegate { this.Hauptformular.statusLabel.Content = status; }));
                        initialKey = "E\r";
                    }

                }
            catch(Exception ex)     
                    {
                        if(ex.Message.Contains("exist"))
                        {runCheck=false;}
                //this.Hauptformular.statusLabel.Dispatcher.Invoke(new Action(delegate { this.Hauptformular.statusLabel.Content = "Kartenleser-Fehler"; }));
                //try
                //{
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                Thread.Sleep(1000);
                //}
                //catch { }

            }
                } while (runCheck);
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }

            
        }
    }
}
