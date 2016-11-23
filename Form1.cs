using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.Data;
namespace DES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static int getInt(string c)
        {
            int[] num = new int[100];
            int p = 1;
            int result = 0;

            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == '0')
                    num[i] = 0;
                else if (c[i] == '1')
                    num[i] = 1;
            }

            for (int i = c.Length - 1; i >= 0; i--)
            {
                result = result + num[i] * p;
                p = p * 2;
            }
            return result;
        }

        public static string generation(string key)
        {
            char[] bits = new char[64];
            bits = key.ToCharArray(); //bits
            int[] Decimal_arr = new int[8];
            string[] bits_string = new string[8];
            // string c;
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bits_string[i] += bits[count];
                    count++;
                }


            }
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                Decimal_arr[i] = Convert.ToInt32(bits_string[i], 2);
                // Console.WriteLine(Decimal_arr[i]);
                if (Decimal_arr[i] == 0)
                {
                    s += "A";
                }
                else
                {
                    s += (char)Decimal_arr[i];
                }
            }


            //for (int i = 0; i < bits_string.Length; i++)
            //{
            //    s+= bits_string[i];
            //}

            return s;
        }
        static byte[] bytes ;
        public static string Decrypt(string cryptedString, string key)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException
                   ("The string which needs to be decrypted can not be null.");
            }
            bytes = ASCIIEncoding.ASCII.GetBytes(key);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            //System.IO.File.WriteAllText(@"C:\WriteText.txt", reader.ReadToEnd());
            return reader.ReadToEnd();
        }
        public static string Encrypt(string originalString, string key)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                       ("The string which needs to be encrypted can not be null.");
            }

            bytes = ASCIIEncoding.ASCII.GetBytes(key);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }


      
           
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string key1 = Convert.ToString(Convert.ToInt64(textBox9.Text), 2).PadLeft(8, '0');
                string key2 = Convert.ToString(Convert.ToInt64(textBox8.Text), 2).PadLeft(8, '0');
                string s = Encrypt(textBox1.Text, key1);
                textBox2.Text = Encrypt(s, key2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       
        }

       
        public void ADD_key(string  key)
        {
            DataAcess_layer dal = new DataAcess_layer();
            dal.open();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@KEY", SqlDbType.VarChar, 100);
            param[0].Value =  key;


            dal.ExecuteCommand("add_key", param);
            dal.close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
           // bytes = ASCIIEncoding.ASCII.GetBytes();

            string s = Decrypt(textBox2.Text, "00000110");
           bytes = ASCIIEncoding.ASCII.GetBytes("00000100");
           textBox3.Text = Decrypt(s, "00001010");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Int64 keys = Convert.ToInt64(Math.Pow(2, 56));

            for (Int64 i = 0; i < keys; i++)
            {
                string temp = Convert.ToString(Convert.ToInt64(i), 2).PadLeft(64, '0');//convert from decimal to binary
                ADD_key(temp);
                //textBox3.Text = temp.ToString();
            }
            MessageBox.Show("done");
        }

        private void button4_Click(object sender, EventArgs e)
        {


            try
            {
                for (int i = 0; i < 150; i++)
                {
                    string temp = Convert.ToString(Convert.ToInt64(i), 2).PadLeft(8, '0');

                    string s = Encrypt(textBox4.Text, temp);
                    //char[] arr = s.ToArray();
                    //s = "";
                    //for (int j = 0; j < arr.Length; j++)
                    //{
                    //    s += arr[j].ToString();
                    //}
                    add_first_table(s, Convert.ToInt32(temp));



                    string s1 = Decrypt(textBox5.Text, temp);
                    //char[] arr1 = s1.ToArray();
                    //s1 = "";
                    //for (int j = 0; j < arr1.Length; j++)
                    //{
                    //    s1 += arr1[j].ToString();
                    //}
                    add_second_table(s1, Convert.ToInt32(temp));
                    //textBox6.Text = temp;
                }
                DataTable dt = new System.Data.DataTable();
                dt = select_keyes();
                dataGridView1.DataSource = dt;
                textBox6.Text = Convert.ToInt64(dataGridView1.Rows[0].Cells[1].Value.ToString(), 2).ToString();
                textBox7.Text = Convert.ToInt64(dataGridView1.Rows[0].Cells[3].Value.ToString(), 2).ToString();
                MessageBox.Show("done");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                delete_tables();
            }
        }

        public void add_first_table(string plain ,int key)
        {
            DataAcess_layer dal = new DataAcess_layer();
            dal.open();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@plain", SqlDbType.VarChar, 100);
            param[0].Value = plain;

            param[1] = new SqlParameter("@key", SqlDbType.Int);
            param[1].Value = key;
            dal.ExecuteCommand("add_first_table", param);
            dal.close();

        }
        public void add_second_table(string plain, int key)
        {
            DataAcess_layer dal = new DataAcess_layer();
            dal.open();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@plain", SqlDbType.VarChar, 100);
            param[0].Value = plain;

            param[1] = new SqlParameter("@key", SqlDbType.Int);
            param[1].Value = key;
            dal.ExecuteCommand("add_second_table", param);
            dal.close();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        public DataTable select_keyes()
        {
            DataAcess_layer dal = new DataAcess_layer();
            dal.open();
            DataTable dt = new DataTable();
            dt = dal.select("select_keyes", null);
            dal.close();
            return dt;
        }

        public void delete_tables( )
        {
            DataAcess_layer dal = new DataAcess_layer();
            dal.open();
            
            dal.ExecuteCommand("delete_tables", null);
            dal.close();

        }

        
    }
}
