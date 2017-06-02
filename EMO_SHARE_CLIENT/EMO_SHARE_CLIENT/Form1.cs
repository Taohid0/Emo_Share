using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace EMO_SHARE_CLIENT
{
    public partial class Form1 : Form
    {
        int difference;
        bool groupChat = false;
        bool avtivateNothingIsHere = true;
        public static string[] arr = new string[100];
        public static string[] messa = new string[100];
        public TextBox[] tb = new TextBox[100];
        public string []toMe = new string[100];

        public static int index = 0;

        string selectedTabTest = "";

        MyTab forWrite = new MyTab();

        MyTab[] mt = new MyTab[100];

     //   MyTab mt = new MyTab();







        private string UserName = "Unknown";

        private StreamWriter swSender;
        private StreamReader srReceiver;
        private TcpClient tcpServer;

        private delegate void UpdateLogCallback(string strMessage);

        private delegate void CloseConnectionCallback(string strReason);
        private Thread thrMessaging;
        private IPAddress ipAddr;
        private bool Connected;


        public Form1()
        {
            


            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent(); ;
        }
        public void OnApplicationExit(object sender, EventArgs e)
        {
            if (Connected == true)
            {

                Connected = false;
                swSender.Close();
                srReceiver.Close();
                tcpServer.Close();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            if (Connected == false)
            {

                InitializeConnection();
            }
            else
            {
                CloseConnection("Disconnected at user's request.");
            }
        }


        private void InitializeConnection()
        {
        //    MyTab[] mt = new MyTab[100];

            ipAddr = IPAddress.Parse(txtIp.Text);

            tcpServer = new TcpClient();
            tcpServer.Connect(ipAddr, 1986);


            Connected = true;

            UserName = txtUser.Text;



            txtIp.Enabled = false;
            txtUser.Enabled = false;
            txtMessage.Enabled = true;
            btnSend.Enabled = true;
            btnConnect.Text = "Disconnect";


            swSender = new StreamWriter(tcpServer.GetStream());
            swSender.WriteLine(txtUser.Text);
            swSender.Flush();

            thrMessaging = new Thread(new ThreadStart(ReceiveMessages));
            thrMessaging.Start();
        }


        private void ReceiveMessages()
        {

            srReceiver = new StreamReader(tcpServer.GetStream());

            string ConResponse = srReceiver.ReadLine();



            if (ConResponse[0] == '1')
            {


                this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { "successfully connected" });


            }

            else
            {
                string Reason = "Not Connected: ";

                Reason += ConResponse.Substring(2, ConResponse.Length - 2);

                this.Invoke(new CloseConnectionCallback(this.CloseConnection), new object[] { Reason });

                return;
            }

            while (Connected)
            {
                try
                {
                    this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { srReceiver.ReadLine() });
                }
                catch
                {

                }

            }
        }


        private void UpdateLog(string strMessage)
        {
            
            if(avtivateNothingIsHere)
            {
                for (int i = 0; i < 100; i++)
                {
                    arr[i] = "nothing is here";

                }

                for (int i = 0; i < 100; i++)
                {
                    toMe[i] = "nothing is here";

                }
                avtivateNothingIsHere = false;
            }

            string testToSend = "unknown";
            string messageToShow = "";
            int toStopMessage = 0;
            int toStopSender = 0;
            string UsernameTest = "nothing";
            string userName = "nothing";


            if (strMessage.Length >= 9 && strMessage[0]=='n' && strMessage[1]=='e')
            {
                
                UsernameTest = strMessage.Substring(0, 9);
                try
                {
                    userName = strMessage.Substring(10, strMessage.Length - 10);
                }
                catch
                {
                    MessageBox.Show("error found");
                }
                
               /* for(int k = 0;k<strMessage.Length;k++)
                {
                    if(strMessage[k]=='|')
                    {
                        userName = strMessage.Substring(k+1,strMessage.Length-k);
                        break;
                    }
                }*/


             
            }
            if (UsernameTest.Equals("NewClient"))
            {



                listBox1.Items.Clear();
            }

            if (UsernameTest.Equals("newClient") && txtUser.Text != userName)
            {
                bool listTestBool = true;

                for (int k = 0; k < listBox1.Items.Count; k++)
                 {
                    if(listBox1.Items[k].ToString().Equals(userName))
                    {
                        listTestBool = false;
                    }
                }
                if (listTestBool)
                {
                    listBox1.Items.Add(userName);
                    listTestBool = false;
                }




            }
            else
            {
                
                for (int i = 0; i < strMessage.Length; i++)
                {
                    if (strMessage[i] == '|')
                    {
                        toStopSender = i;
                        testToSend = strMessage.Substring(i + 1);
                    
                        break;
                    }
                }

                string tabReceiverName = "";



                if (testToSend.Equals(txtUser.Text))
                {
                    string sender = "";
                    sender = strMessage.Substring(0, toStopSender) + "\n";
                   
                //    txtLog.AppendText(sender);

                    
                    string sendableMessage = " ";

                    for (int i = 0; i < strMessage.Length; i++)
                    {
                        if (strMessage[i] == ':')
                        {
                            tabReceiverName= strMessage.Substring(0, i);
                            difference = tabReceiverName.Length;
                           // difference =  strMessage.Length -toStopSender;
                            sendableMessage = strMessage.Substring(i + 1,strMessage.Length-i-difference-1);
                              
                           //   messageToShow = strMessage.Substring(i + 1, strMessage.Length - i - (strMessage.Length - toStopMessage)-1);
                            break;
                        }
                        
                    }
                    





                    bool test = true;

                    string userNameTab = tabReceiverName;
                  //  userNameTab = listBox1.SelectedItem.ToString();



                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Equals(tabReceiverName))
                        {
                            test = false;
                            break;
                        }
                    }


                    if(test)
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Equals("nothing is here"))
                        {
                            arr[i] = tabReceiverName;
                            break;
                        }
                    }

                    

                     if (test == true )
                    {
                      

                        mt[index] = new MyTab();

                        mt[index].getTab();

                        mt[index].myTabPage.Text = tabReceiverName;

             //           selectedTabTest = userName;

                        tabControl1.Controls.Add(mt[index].myTabPage);

                       
                            txtLog.AppendText(tabReceiverName + " " + messageToShow );
                      
                            mt[index].writeOnTab("<-" + sendableMessage+"\n");

                          

                        arr[index] = tabReceiverName;
                        index++;
                      


                    }
                    else
                    {

                       // tabReceiverName = tabReceiverName.Substring(1, tabReceiverName.Length - 1);
                       
                        for (int I = 0; I< tabControl1.TabPages.Count; I++)
                        {
                           // MessageBox.Show(":"+tabControl1.TabPages[I].Text + ": :" + tabReceiverName+":");
                            if (tabControl1.TabPages[I].Text==tabReceiverName)
                            {

                                
                                mt[I].writeOnTab("<-" +sendableMessage+"\n");


                                break;
                            }


                        }
                    
                        
                    }
 
                }
 

            }



        }
        private void CloseConnection(string Reason)
        {

            txtLog.AppendText(Reason + "\r\n");

            txtIp.Enabled = true;
            txtUser.Enabled = true;
            txtMessage.Enabled = false;
            btnSend.Enabled = false;
            btnConnect.Text = "Connect";


            Connected = false;
            swSender.Close();
            srReceiver.Close();
            tcpServer.Close();
        }

        private void SendMessage()
        {


            try
            {
                selectedTabTest = listBox1.SelectedItem.ToString();
            }
            catch
            {
                
            }
             if(groupChat)
             { 
                 txtLog.AppendText("me : " + txtMessage.Text + "\n");

             }

             else
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                if (tabControl1.TabPages[i].Text==selectedTabTest)
                {
                 
                        mt[i].writeOnTab("-> " + txtMessage.Text + "\n");


                       
                    break;
                }
               
              
            }

             string messageSender = "null";
            
       
                 messageSender = listBox1.GetItemText(listBox1.SelectedItem);
            
                 
            if (txtMessage.Lines.Length >= 1 )
            {



                swSender.WriteLine(txtMessage.Text + "|" + messageSender);
               
                swSender.Flush();
                txtMessage.Lines = null;
            }
          /*  else
            {
                for(int t = 0;t<tabControl1.TabPages.Count;t++)
                {
                    swSender.WriteLine(txtMessage.Text + "|" + tabControl1.TabPages[t].Text);

                }
            }*/
            txtMessage.Text = "";

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();

        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the key is Enter
            if (e.KeyChar == (char)13)
            {
                ActiveForm.AcceptButton = btnSend;
            }
        }







        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtLog_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMessage_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendMessage();
            }
        }



        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            bool test = true;
            groupChat = false;
            string userNameTab = "";
       
                userNameTab = listBox1.SelectedItem.ToString();


              
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals(userNameTab) )
                {
                    test = false;
                    break;
                }
            }



            if (test)
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals("nothing is here"))
                {
                    arr[i] = userNameTab;
                    break;
                }
            }


            if (test == true)
            {
                userNameTab = listBox1.SelectedItem.ToString();

                mt[index] = new MyTab();

                mt[index].getTab();

                mt[index].myTabPage.Text = userNameTab;

                selectedTabTest = userNameTab;

                tabControl1.Controls.Add(mt[index].myTabPage);
               // mt[index].writeOnTab("Me : " + txtMessage.Text);
               
                index++;
           
               

            }
            
        

           
         



        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {


            

        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupChat = true;
            listBox1.ClearSelected();
         
        }
    }
}
