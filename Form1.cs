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






        //singleton 
        Listener listenobject = Listener.Instance();


        string[] var=new string[3600];

        int totpos = 0;




        public void On_msg_recieved()
        {
        
        
        }



        public Base()
        {
       
            InitializeComponent();
        }

        private void Base_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <= 1; i++)
                var[i] = "0";
            for(int j=2;j<=3598;j++)
                var[j]="1";
           
        }

        private void PICBOX_Paint(object sender, PaintEventArgs e)
        {
            
                for(int ypos=0;ypos<720;ypos=ypos+16)
                    for(int xpos=0;xpos<1280;xpos=xpos+16)
                        DrawIconRectangle(e,var[totpos++%3600],xpos,ypos);

            
        }
       

       
       

       private void DrawIconRectangle(PaintEventArgs e,string d,int Xpos,int Ypos )
       {
           Icon newIcon;

           // Create icon.
           if(d=="1")
            newIcon = new Icon("on.ico");
           else
            newIcon = new Icon("off.ico");

           // Create rectangle for icon.


           Rectangle rect = new Rectangle(Xpos, Ypos, 16, 16);


           // Draw icon to screen.
           e.Graphics.DrawIcon(newIcon, rect);

           Application.DoEvents();

       }

       private void PICBOX_Click(object sender, EventArgs e)   //impulsul
       {
          PICBOX.Invalidate();


            
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
