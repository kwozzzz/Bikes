using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace TREK
{
    public partial class Trek : Form
    {
        //string WriteFile = "";
        string ReadFile = "";
        string FileRecord = "";

        string sChkthis = "]";
        string sChkthat = "{";
        string sChkthose = ",";
        string sChkthese = "bikes";
        string[] sChk;

        string TrackFilepath = "C:\\Log\\Log2\\"; // TREKbikes.txt";//Proof file were read
        string new_sPaths = "";

        public Trek()
        {
            InitializeComponent();

            if (!System.IO.Directory.Exists(@TrackFilepath))
                System.IO.Directory.CreateDirectory(@TrackFilepath);//create a directory

            MessageBox.Show("PLACE ALL TREK FILES IN C:\\Log\\ FOLDER!\n TO BE READ... ", "Copy Files", MessageBoxButtons.OK);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(new_sPaths))
                System.IO.File.Delete(new_sPaths);  //deletes the NR (newly read) file or files

            this.Close();
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            string sPath = "";
            string New_sPath = "";
            int len = 0;

            ofdSelectFile.InitialDirectory = @"C:\\Log\\";
            ofdSelectFile.Title = "Select a File";
            ofdSelectFile.FileName = "";
            ofdSelectFile.Filter = "All files(*.*)|*.*|Text files(*.txt)|*.txt|Count Files (*.csv)|*.csv|Data files(*.dat)|*.dat"; //Count Files (*.csv)|*.csv|All files (*.*)|*.*";
            ofdSelectFile.FilterIndex = 2; //here you can add 2 or 3 extensions txt.xls etc
            int i = 0;

            //Show the open file dialog box
            if (ofdSelectFile.ShowDialog() == DialogResult.OK)
            {

                foreach (String file in ofdSelectFile.FileNames)
                {
                    lstBxFiles.Items.Add(ofdSelectFile.FileNames[i]);//count of number of occurance lines 
                    ReadFile = ofdSelectFile.FileNames[i];
                    len = ReadFile.Length;
                    sPath = ReadFile;
                    sPath = sPath.Insert(len - 4, "NR"); //add Newly Read "NR"
                    if (!System.IO.File.Exists(sPath))
                        System.IO.File.Copy(ReadFile, sPath);
                    len = sPath.Length;

                    New_sPath = sPath.Substring(7, len - 7);
                    try
                    {
                        New_sPath = New_sPath.Insert(0, "C:\\Log\\Log2\\");
                        System.IO.File.Move(sPath, New_sPath);//NR file copied for proof
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Clear out old files in C:\\Log\\Log2 folder!\n");
                    }
                    FileRecord = New_sPath;/////////Newly read/copied NRfile in C:\Log\Log2
                    i++;
                }
            }
            new_sPaths = New_sPath;
            New_sPath = "";
        }

        private void btnFileSearch_Click(object sender, EventArgs e)
        {
            int iNumRec = 0;
            int i = 0;
            int j = 0;
            string PrevRecord = "";
            //sChk[0] = "Trek Fuel EX 9.8";

            try
            {//Read each line of the file in the C:\Log\CLASS_Cntrl\DATALogFiles folder
                StreamReader FileLine = new StreamReader(@FileRecord);

                FileRecord = FileLine.ReadLine();
                FileRecord = FileLine.ReadLine();// get rid of first entry

                while (FileRecord != null)//Do work of finding reopen search criteria
                {
                    if (FileRecord.Contains(sChkthese))
                        iNumRec++;
                    if (!FileRecord.Contains(sChkthis) && !FileRecord.Contains(sChkthat) && !FileRecord.Contains(sChkthese) && !FileRecord.Contains(sChkthose))
                    {
                        lstBxRead.Items.Add(FileRecord);//list out all occurences of search criteria
                        PrevRecord = FileRecord;
                    }
                    FileRecord = FileLine.ReadLine();
                    //if (FileRecord.Contains(PrevRecord))
                    //{
                    //    //sChk[i] = PrevRecord;//perhaps a case statement to hold positive compares
                    //    i++;
                    //}
                }

                FileLine.Close();
                FileLine.Dispose();

            }
            catch (Exception objException)
            {
                MessageBox.Show("Error in File: " + objException);
                //MessageBox.Show("The file cannot be read!");
            }

            textBox1.Text = iNumRec.ToString();//how many bikes or families found
            textBox2.Text = i.ToString();//how many lines read
        }
    }
}
