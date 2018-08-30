using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serialApp
{
    public partial class Form1 : Form
    {
        #region 软件版本号
        public const string _softwareVersion = "V1.0.0";
        public const string _softwareName = "文件测试工具";
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           this.Text = _softwareName + _softwareVersion;

            //init port combox 
            string[] PortNames = SerialPort.GetPortNames();    //获取本机串口名称，存入PortNames数组中

            if (PortNames.Count() <= 0)
            {
                MessageBox.Show("no ports available");

            }
            else
            {
                for (int i = 0; i < PortNames.Count(); i++)
                {
                    comboBoxPort.Items.Add(PortNames[i]);   //将数组内容加载到comboBox控件中
                }

                comboBoxPort.SelectedIndex = 0;
            }

            //init badurate combox
            int[] baudrate_list = new int[5] { 115200, 19200, 38400, 9600, 57600 };
            for (int i = 0; i < baudrate_list.Length; i++)
            {
                comboBoxBaud.Items.Add(baudrate_list[i].ToString());
            }
            comboBoxBaud.SelectedIndex = 0;


            //init file protocl combox
            comboBoxFileProtocol.SelectedIndex = 0;
        }

        private void buttonOpenCloseSerial_Click(object sender, EventArgs e)
        {
            if (buttonOpenClosePort.Text == "打开串口")
            {
                buttonOpenClosePort.Text = "关闭串口";
                serialPort1.PortName = comboBoxPort.SelectedItem.ToString();
                serialPort1.BaudRate = int.Parse(comboBoxBaud.SelectedItem.ToString());
                try
                {
                    serialPort1.Open();
                }
                catch
                {
                    MessageBox.Show("串口打开失败");

                }
                MessageBox.Show("串口打开成功");

                Console.WriteLine("Baudrate:{0},Port:{1}", serialPort1.BaudRate, serialPort1.PortName);
            }
            else
            {
                buttonOpenClosePort.Text = "打开串口";
                try
                {
                    serialPort1.Close();
                }
                catch
                {
                    MessageBox.Show("串口关闭失败");

                }
                MessageBox.Show("串口关闭成功");

            }
        }

        private void buttonAddFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All files (*.*)|*.*";  //name|reg|name|reg
            openFileDialog1.Title = "Choose a File";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //遍历文件
                FileInfo info;
                ListViewItem item;
                foreach (string filename in openFileDialog1.FileNames)
                {
                    info = new FileInfo(filename);
                    string[] item_string = new string[3] { info.Name, info.Length.ToString() ,info.CreationTime.ToString("yyyy/MM/dd HH:mm:ss") };
                    item = new ListViewItem(item_string);
                    listViewDisplayFileList.Items.Add(item);
                }
            }
        }

        private void buttonAddDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                Console.WriteLine("choose path = {0}"+path);

                //遍历目录
                DirectoryInfo directory = new DirectoryInfo(path);
                ListViewItem item;
                foreach (FileInfo file in directory.GetFiles())
                {
                    string[] item_string = new string[3] { file.Name, file.Length.ToString(), file.CreationTime.ToString("yyyy/MM/dd HH: mm:ss")};
                    item = new ListViewItem(item_string);
                    listViewDisplayFileList.Items.Add(item);
                }
            }
        }

        private void buttonClearList_Click(object sender, EventArgs e)
        {
            listViewDisplayFileList.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //获取选定项目,调试代码
            foreach (ListViewItem item in listViewDisplayFileList.SelectedItems)
            {
                string info = string.Empty;
                for (int i=0;i< item.SubItems.Count;i++)
                {
                    info += " ";
                    info +=item.SubItems[i].Text;
                }
                Console.WriteLine(info);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            //获取选定项目
            foreach (ListViewItem item in listViewDisplayFileList.SelectedItems)
            {
                item.Remove();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //修改选定项目,调试代码
            foreach (ListViewItem item in listViewDisplayFileList.SelectedItems)
            {
                item.SubItems[0].Text = "modify";
            }
        }
    }
}
