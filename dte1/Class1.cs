using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bd
{
    class DBConnect
    {
        public void insertar()
        {
            string connectionString = "SERVER=" + "localhost" + ";" + "DATABASE=" + "bowa" + ";" + "UID=" +"root" + ";" + "PASSWORD=" + "12345678"+ ";";

            string Command = "INSERT INTO RespuestaSii (Cliente, dte, trackid, Estado, Glosa, NumAtencion, Fecha, hora, tipoDoc, Informados, aceptados, rechazados, reparos ) VALUES " +
               "(@Cliente, @dte, @trackid, @Estado, @Glosa, @NumAtencion, @Fecha, @hora, @tipoDoc, @Informados, @aceptados, @rechazados, @reparos);";
            using (var mConnection = new MySqlConnection(connectionString))
            {
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.Parameters.AddWithValue("@Cliente", dte1.consultaestado.Cliente);
                    myCmd.Parameters.AddWithValue("@dte", dte1.consultaestado.dte);
                    myCmd.Parameters.AddWithValue("@trackid", dte1.consultaestado.trackid);
                    myCmd.Parameters.AddWithValue("@Glosa", dte1.consultaestado.Glosa);
                    myCmd.Parameters.AddWithValue("@Numatencion", dte1.consultaestado.Numatencion);
                    myCmd.Parameters.AddWithValue("@Fecha", dte1.consultaestado.Fecha);
                    myCmd.Parameters.AddWithValue("@hora", dte1.consultaestado.hora);
                    myCmd.Parameters.AddWithValue("@tipoDoc", dte1.consultaestado.tipoDoc);
                    myCmd.Parameters.AddWithValue("@Informados", dte1.consultaestado.Informados);
                    myCmd.Parameters.AddWithValue("@aceptados", dte1.consultaestado.aceptados);
                    myCmd.Parameters.AddWithValue("@rechazados", dte1.consultaestado.rechazados);
                    myCmd.Parameters.AddWithValue("@reparos", dte1.consultaestado.reparos);
                }
                mConnection.Close();
            }
        }
    }
}
