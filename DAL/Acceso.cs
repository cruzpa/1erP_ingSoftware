using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    internal class Acceso
    {
        private SqlConnection conexion;
        private SqlTransaction tx;

        public void Abrir()
        {
            conexion = new SqlConnection();
            conexion.ConnectionString = "Initial catalog=1erP; Data Source=.;integrated security=SSPI";            
            conexion.Open();
        }

        public void Cerrar()
        {
            conexion.Close();
            conexion=null;
            GC.Collect();   
        }

        public SqlCommand CrearComando(string sql, List<SqlParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            if(parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            if(tx != null)
            {
                cmd.Transaction = tx;
            }

            return cmd;
        }

        public int Escribir(String sql, List<SqlParameter> parameters)
        {
            SqlCommand cmd = CrearComando(sql, parameters);
            int filas = 0;

            try
            {
                filas = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                filas = -1;
            }
            return filas;
        }
        public SqlDataReader Leer(String sql, List<SqlParameter> parameters = null)
        {
            SqlCommand cmd = CrearComando(sql, parameters);
            return cmd.ExecuteReader();
        }

        public int LeerEscalar(String sql, List<SqlParameter> parameters = null)
        {
            SqlCommand cmd = CrearComando(sql, parameters);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public SqlParameter CrearParametro(string name, string value)
        {
            SqlParameter p = new SqlParameter(name, value);
            p.DbType = System.Data.DbType.String;
            return p;
        }

        public SqlParameter CrearParametro(string name, int value)
        {
            SqlParameter p = new SqlParameter(name, value);
            p.DbType = System.Data.DbType.Int32;
            return p;
        }

        public SqlParameter CrearParametro(string name, float value)
        {
            SqlParameter p = new SqlParameter(name, value);
            p.DbType = System.Data.DbType.Single;
            return p;
        }

        public void IniciarTx()
        {
            if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
            {
                tx = conexion.BeginTransaction();
            }
        }
        public void ConfirmarTx()
        {
            if (tx != null)
            {
                tx.Commit();
                tx = null;
            }
        }

        public void DeshacerTx()
        {
            if (tx != null)
            {
                tx.Rollback();
                tx = null;
            }
        }
    }
}