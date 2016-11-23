using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace DES
{
    class DataAcess_layer
    {
        SqlConnection cn;


        public DataAcess_layer()
        {
            cn = new SqlConnection(@"Data Source=USERFILES\SQLEXPRESS;Initial Catalog=securty_assingment;Integrated Security=True");
           
        }

        public void open()   // to Ensure to open connection
        {
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }

        }
        public void close()  // to close connection
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        public DataTable select(string stored_procedure, SqlParameter[] parame)  // Read data from database 
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = stored_procedure;
            cmd.Connection = cn;
            if (parame != null)      // to check the paramates have value to add it to command
            {
                for (int i = 0; i < parame.Length; i++)  //add paramates to command
                {
                    cmd.Parameters.Add(parame[i]);

                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void ExecuteCommand(string stored_procedure, SqlParameter[] parame)  //to Execute command like(insert,delete,update....
        {
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = stored_procedure;
            sqlcmd.Connection = cn;
            if (parame != null)
            {
                for (int i = 0; i < parame.Length; i++)  //add paramates to command
                {
                    sqlcmd.Parameters.Add(parame[i]);

                }
                // sqlcmd.Parameters.AddRange(parame);
            }

            sqlcmd.ExecuteNonQuery();
        }


    }
}
