using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestLEdBoard
{
    public partial class Base : Form, MainInterface
    {
        int count=0;
        //singleton 
        Listener listenobject = Listener.Instance();

        char[] var = new char[1024];
        int totpos = 0;

        string datas;



        public void On_msg_recieved(string data)
        {
            int d = data.Length;
            datas = data;
            foreach (char c in data) 
            { 
               var[count++]=c;      
            }
            PICBOX.Invalidate();
            count = 0;
        }



        public Base()
        {
            listenobject.Register_Receiver(this);
            InitializeComponent();
        }

        private void Base_Load(object sender, EventArgs e)
        {
           
           
        }



        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }


        private void PICBOX_Paint(object sender, PaintEventArgs e)
        {
                for(int ypos=0;ypos<800;ypos=ypos+25)
                    for(int xpos=0;xpos<800;xpos=xpos+25)
                        DrawIconRectangle(e,var[totpos++%1024],xpos,ypos);
        }
       

       
       

       private void DrawIconRectangle(PaintEventArgs e,char d,int Xpos,int Ypos )
       {
           Icon newIcon;

           // Create icon.
           if(d=='1')

            newIcon = new Icon("b.ico");
           else

            newIcon = new Icon("r.ico");

           // Create rectangle for icon.

           Rectangle rect = new Rectangle(Xpos, Ypos, 25, 25);


           // Draw icon to screen.
           e.Graphics.DrawIcon(newIcon, rect);

          // Application.DoEvents();

       }

       private void PICBOX_Click(object sender, EventArgs e)   //impulsul
       {
          PICBOX.Invalidate();


            
       }

       private void Base_FormClosing(object sender, FormClosingEventArgs e)
       {
           listenobject.ClientThread.Abort();
       }
      
  
   



        /*
        private void DrawIconRectangle()
        {
            // Create icon.

            Icon newIcon = new Icon("on.ico");

            // Create rectangle for icon.
            Rectangle rect = new Rectangle(0, 0, 53, 50);

            // Draw icon to screen.
            //.Graphics.DrawIcon(newIcon, rect);
          

        }
         * */



    }
}
