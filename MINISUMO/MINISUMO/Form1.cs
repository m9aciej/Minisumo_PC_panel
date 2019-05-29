using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace MINISUMO
{
    public partial class MiniSumo : Form
    {
        private DateTime data;
        int a = 1;
        int flaga = 0;
        private string txt;
        String[] tablica = new String[20];
        String stan, pwml, pwmp, ADC4, ADC5, czujnik0, czujnik2;
        delegate void Delegat1();

        

        Delegat1 moj_del1;

        private void Wykresy_Click(object sender, EventArgs e)
        {
            if (flaga == 0)
            {
                flaga = 1;
            }
            else
            {
                flaga = 0;
            }


        }

        private void WłączRobota_Click(object sender, EventArgs e)
        {
            mySerialPort.WriteLine("x");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kp = textBox9.Text;
            mySerialPort.WriteLine(kp);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string kd = textBox10.Text;
            mySerialPort.WriteLine(kd);
        }

        SerialPort mySerialPort = new SerialPort("COM5");
        public MiniSumo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //mySerialPort.PortName = comboBox1.Text;
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            //mySerialPort.ReadBufferSize = 4096;
           // mySerialPort.Encoding = Encoding.GetEncoding(28591);
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);
            moj_del1 = new Delegat1(WpiszOdebrane);
            mySerialPort.Open();
            
        }

        private void wykres(object sender, EventArgs e)
        {
            data = DateTime.Now;
            string time = data.Hour + ":" + data.Minute;// + ":" + data.Second;
            this.chart1.Series["ADC-L"].Points.AddXY(time, ADC4);
            this.chart1.Series["ADC-P"].Points.AddXY(time, ADC5);
            this.chart2.Series["PWML"].Points.AddXY(time, pwml);
            this.chart2.Series["PWMP"].Points.AddXY(time, pwmp);

        }
        private void DataRecievedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            txt = mySerialPort.ReadTo(".");
            Invoke(moj_del1);
        }
        private void WpiszOdebrane()
        {
            textBox1.Text = txt;
            
            //tablica[0] = tablica[1] = tablica[2] = tablica[3] = tablica[4] = tablica[5] = "";
            stan = pwml = pwmp = ADC4 = ADC5 = czujnik0 = czujnik2 = "";
            foreach (Char element in txt)
            {
                if (element != ','&& a==1) { stan += element;  }
                else if (element != ',' && a == 2) { pwml += element; }
                else if (element != ',' && a == 3) { pwmp += element; }
                else if (element != ',' && a == 4) { ADC4 += element; }
                else if (element != ',' && a == 5) { ADC5 += element; }
                else if (element != ',' && a == 6) { czujnik0 += element; }
                else if (element == ',') { a = a + 1; }
                else
                {
                    czujnik2 += element;
                }
            }

            a = 1;
            // textBox2.Text = txt[1].ToString();
            textBox2.Text = stan ;
            textBox3.Text = pwml;
            textBox4.Text = pwmp;
            textBox5.Text = ADC4;
            textBox6.Text = ADC5;
            textBox7.Text = czujnik0;
            textBox8.Text = czujnik2;


            try { 

            if (Int32.Parse(ADC4) > 300) { pictureBox2.BackColor = System.Drawing.Color.Lime; }
            else { pictureBox2.BackColor = System.Drawing.Color.Red; }

            if (Int32.Parse(ADC5) > 300) { pictureBox3.BackColor = System.Drawing.Color.Lime; }
            else { pictureBox3.BackColor = System.Drawing.Color.Red; }

            if (Int32.Parse(czujnik0) == 1) { pictureBox4.BackColor = System.Drawing.Color.Lime; }
            else { pictureBox4.BackColor = System.Drawing.Color.Red; }

            if (Int32.Parse(czujnik2) == 1) { pictureBox5.BackColor = System.Drawing.Color.Lime; }
            else { pictureBox5.BackColor = System.Drawing.Color.Red; }

            
            if (Int32.Parse(pwml) > 0  )
            {
                
                pictureBox7.BackColor = System.Drawing.Color.Lime;
                pictureBox8.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                
                pictureBox7.BackColor = System.Drawing.Color.Red;
                pictureBox8.BackColor = System.Drawing.Color.Lime;
            }
            if (Int32.Parse(pwmp) > 0)
            {
                
                pictureBox10.BackColor = System.Drawing.Color.Lime;
                pictureBox11.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                
                pictureBox10.BackColor = System.Drawing.Color.Red;
                pictureBox11.BackColor = System.Drawing.Color.Lime;
            }


            }
            catch (Exception) { }

            if (flaga == 1)
            { 
            this.Invoke(new EventHandler(wykres));
            }

        }


    }

}
    






        
