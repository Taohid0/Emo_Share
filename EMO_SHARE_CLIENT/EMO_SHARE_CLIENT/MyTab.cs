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
   public  class MyTab
    {
       
           public TabPage myTabPage = new TabPage("new textTab");
            public TextBox textBox  = new TextBox();

            public void getTab()
            {
                textBox.Multiline = true;
                textBox.Size = new System.Drawing.Size(238, 258);

                myTabPage.Controls.Add(textBox);
            }
       public void writeOnTab(string toWrite)
            {
                textBox.AppendText(toWrite);
            }
    }


   

}
