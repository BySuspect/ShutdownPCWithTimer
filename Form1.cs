using RemoteShutdownPC.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace RemoteShutdownPC
{
    public partial class Form1 : Form
    {
        FirebaseHelper firebase = new FirebaseHelper();
        bool threadRunning = true;
        public Form1()
        {
            InitializeComponent();


            //Starting background Thread
            Thread thread = new Thread(backgroundTask);
            thread.SetApartmentState(ApartmentState.STA);
            CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        async void backgroundTask()
        {
            while (threadRunning)
            {
                Thread.Sleep(49);//Minimum CPU usage
                var list = await firebase.GetData("Shutdown");//Requesting data from firebase

                if (list == null)//If database is empty
                {
                    await firebase.AddData("Shutdown");
                    continue;
                }

                if (list.Shutdown)
                {
                    if (list.Timer != "0")//If shutdowning with timer
                    {

                        await firebase.UpdateData(list.Name, false, "0", list.Cancel);
                        await sendToCMD(Convert.ToInt32(list.Timer));
                    }
                    else
                    {
                        //Without timer shutdown sending 1 milisecond
                        firebase.UpdateData(list.Name, false, list.Timer, list.Cancel);
                        await sendToCMD(1);
                    }
                }

                if (list.Cancel)//if cancel is true 
                {
                    await cancelShutdown();
                    firebase.UpdateData(list.Name, list.Shutdown, "0", false);
                }
            }
        }

        //Send shutdown timer to CMD
        async Task sendToCMD(int time)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown -s -t " + time;//Time is milisecond
            process.StartInfo = startInfo;
            process.Start();
        }

        //Send cancel command to CMD
        async Task cancelShutdown()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown -a";
            process.StartInfo = startInfo;
            process.Start();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
