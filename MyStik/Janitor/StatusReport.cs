using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MyStik;
using System.Threading;
using System.Diagnostics;
using System.Windows;

namespace myUserControls
{
    public class StatusReport
    {
        private SurfaceWindow1 _hauptformular;
        public SurfaceWindow1 Hauptformular
        {
            get { return _hauptformular; }
            set { _hauptformular = value; }
        }
        
        public void doReport()
        {
            Thread t = new Thread(new System.Threading.ThreadStart(delegate()
            {
                do
                {

                    try
                    {
                        string myInsertQuery = "TRUNCATE TABLE status; INSERT INTO status (date, processes, cardreader, touch, net) Values(@date, @processes, @cardreader, @touch, @net)";
                        string processString = "";
                        MySqlCommand myCommand = new MySqlCommand(myInsertQuery);
                        

                        Hauptformular.Dispatcher.Invoke(new Action(delegate
                                {
                                    foreach (Process clsProcess in Process.GetProcesses())
                                    {
                                        processString = processString + clsProcess.ProcessName + "; ";
                                    }
                                    myCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString());
                                    myCommand.Parameters.AddWithValue("@processes", processString);
                                    myCommand.Parameters.AddWithValue("@cardreader", Hauptformular.statusLabel.Content.ToString());
                                    myCommand.Parameters.AddWithValue("@touch", Hauptformular.TouchesOver.Count().ToString());
                                    myCommand.Parameters.AddWithValue("@net", "-");
                                    myCommand.Connection = Hauptformular.myConnection;
                                    try
                                    {
                                        Hauptformular.myConnection.Open();
                                        myCommand.ExecuteNonQuery();
                                        myCommand.Connection.Close();
                                    }
                                    catch { }
                                }));

                        Thread.Sleep(60000);
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                while (true);
            }));
            t.Start();
        
        }
    
    }
}
