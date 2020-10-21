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
using System.IO;

namespace USB_ESP32
{
    public partial class Form1 : Form
    {
        SerialPort USBPort = new SerialPort();
        string payload = "";

        public Form1()
        {
            InitializeComponent();
            tick.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cb_baud.SelectedIndex = 2;
            USBPort.DataReceived += new SerialDataReceivedEventHandler(receivedHandler);
            changeEnabled(0);
        }

        private void refreshCOM()
        {
            int i = 0;
            bool changed = false;
            if (cb_port.Items.Count == SerialPort.GetPortNames().Length)
            {
                foreach (string s in SerialPort.GetPortNames())
                {
                    if (cb_port.Items[i++].Equals(s) == false)
                    {
                        changed = true;
                    }
                }
            }
            else
            {
                changed = true;
            }
            if (changed == false)
            {
                return;
            }
            if(cb_port.Items.Count > 0)
            {
                cb_port.Items.Clear();
            }
            foreach (string s in SerialPort.GetPortNames())
            {
                cb_port.Items.Add(s);
            }
            if (cb_port.Items.Count > 0)
            {
                cb_port.SelectedIndex = 0;
            }
            else
            {
                cb_port.Text = "";
            }
        }

        private void timerCOM_Tick(object sender, EventArgs e)
        {
            refreshCOM();
        }

        private void btn_conectar_Click(object sender, EventArgs e)
        {
            if (USBPort.IsOpen == false)
            {
                try
                {
                    USBPort.PortName = cb_port.Items[cb_port.SelectedIndex].ToString();
                    USBPort.BaudRate = Int32.Parse(cb_baud.Text);
                    USBPort.Open();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return;
                }
                if (USBPort.IsOpen)
                {
                    changeEnabled(1);
                }
            }
            else
            {
                try
                {
                    USBPort.Close();
                    changeEnabled(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (USBPort.IsOpen == true)
            {
                USBPort.Close();
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (USBPort.IsOpen == true)
            {
                USBPort.Write(tb_send.Text);
                tb_chat.AppendText(System.Environment.NewLine + "SEND: " +tb_send.Text);

            }
            tb_send.Text = "";
        }

        private void receivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            payload = USBPort.ReadExisting();
            Invoke(new EventHandler(appendChat));
        }

        private void appendChat(object sender, EventArgs e)
        {
            tb_chat.AppendText(System.Environment.NewLine + payload);
        }

        private void tb_send_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btn_send_Click(sender,e);
            }
        }

        private void btn_on_Click(object sender, EventArgs e)
        {
            if (USBPort.IsOpen == true)
            {
                USBPort.Write(nud_node.Value.ToString() + "0");
            }
        }

        private void btn_off_Click(object sender, EventArgs e)
        {
            if (USBPort.IsOpen == true)
            {
                USBPort.Write(nud_node.Value.ToString() + "1");
            }
        }

        private void btn_state_Click(object sender, EventArgs e)
        {
            if (USBPort.IsOpen == true)
            {
                USBPort.Write(nud_node.Value.ToString() + "2");
            }
        }

        private void changeEnabled(int state)
        {
            if(state == 1)
            {
                cb_port.Enabled = false;
                btn_connect.Text = "Disconnect";
                btn_on.Enabled = true;
                btn_off.Enabled = true;
                btn_state.Enabled = true;
                btn_send.Enabled = true;
                btn_clear.Enabled = true;
                tb_send.Enabled = true;
                nud_node.Enabled = true;
            }
            else
            {
                cb_port.Enabled = true;
                btn_connect.Text = "Connect";
                btn_on.Enabled = false;
                btn_off.Enabled = false;
                btn_state.Enabled = false;
                btn_send.Enabled = false;
                btn_clear.Enabled = false;
                tb_send.Enabled = false;
                nud_node.Enabled = false;
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            tb_chat.Text = "";
            tb_send.Text = "";
        }
    }
}
