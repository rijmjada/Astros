using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


/*-
                Crear clase AccesoDatos que haga la conexion con la base de datos. 
                Agregar los objetos y using correspondientes.

                Métodos: ObtenerListaPlaneta():List<Planeta>
	                 AgregarPlaneta(planeta:Planeta):bool
	                 ModificarPlaneta(planeta:Planeta):bool	
	                 EliminarPlaneta(id:Int):bool	 
*/
namespace Entidades
{
    public class AccesoDatos
    {

        static SqlCommand comando;
        static SqlConnection conexion;

        static AccesoDatos()
        {

            SqlConnectionStringBuilder strConexion = new SqlConnectionStringBuilder();
            strConexion.DataSource = @"DIEGO-PC\SQLEXPRESS";
            strConexion.InitialCatalog = @"Astros";
            strConexion.IntegratedSecurity = true;


            comando = new SqlCommand();
            conexion = new SqlConnection(strConexion.ToString());
            comando.Connection = conexion;
            comando.CommandType = System.Data.CommandType.Text;

        }

        public static List<Planeta> ObtenerListaPlaneta()
        {
            List<Planeta> lista = new List<Planeta>();

            try
            {
                comando.Parameters.Clear();
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                comando.CommandText = $"SELECT * FROM planetas ";
               
                SqlDataReader dataReader = comando.ExecuteReader();

                while (dataReader.Read())
                {
                    // Obtengo los datos.
                    int id = Convert.ToInt32(dataReader["id"]);
                    string nombre = dataReader["nombre"].ToString();
                    int satelites = Convert.ToInt32(dataReader["satelites"]);
                    double gravedad = Convert.ToDouble(dataReader["gravedad"]);

                    // Creo el objecto con los datos.
                    Planeta p1 = new Planeta(id, nombre, satelites, gravedad);

                    // Agrego a la lista.
                    lista.Add(p1);
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            finally
            {
                conexion.Close();
            }

            return lista;
        }

        public static bool AgregarPlaneta(Planeta planeta)
        {
            bool ret = true;

            try
            {
                comando.Parameters.Clear();
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
                comando.CommandText = $"INSERT INTO planetas (nombre, satelites, gravedad)   " +
                    $"                  VALUES (@nombre, @satelites, @gravedad) ";

             
                comando.Parameters.AddWithValue("@nombre", planeta.nombre);
                comando.Parameters.AddWithValue("@satelites", planeta.satelites);
                comando.Parameters.AddWithValue("@gravedad", planeta.gravedad);

                comando.ExecuteNonQuery();

            }

            catch (Exception)
            {
                ret = false;
            }

            finally
            {
                conexion.Close();
            }

            return ret;
        }


        public static bool ModificarPlaneta(Planeta planeta)
        {
            bool ret = true;
           
            try
            {
                comando.Parameters.Clear();
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
                comando.CommandText = $"UPDATE planetas SET nombre = @nombre , satelites = @satelites , gravedad = @gravedad WHERE nombre = {planeta.nombre}, satelites = {planeta.satelites}, gravedad{planeta.gravedad} ";


                comando.Parameters.AddWithValue("@nombre", planeta.nombre);
                comando.Parameters.AddWithValue("@satelites", planeta.satelites);
                comando.Parameters.AddWithValue("@gravedad", planeta.gravedad);
          
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                ret = false;
            }

            finally
            {
                conexion.Close();
            }

            return ret;
        }

        public static bool Eliminar(int id)
        {
            bool ret = true;

            try
            {
                comando.Parameters.Clear();
                conexion.Open();
                comando.CommandText = $"DELETE FROM planetas WHERE id = {id}";
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ret = false;
            }
            finally
            {
                conexion.Close();
            }

            return ret;
        }

        public static int DevolverIdPlaneta(Planeta planeta)
        {
            int id = 0;
            try
            {
                comando.Parameters.Clear();
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                comando.CommandText = $"SELECT * FROM planetas WHERE nombre = @nombre AND satelites = @satelites AND gravedad = @gravedad";

                comando.Parameters.AddWithValue("@nombre", planeta.nombre);
                comando.Parameters.AddWithValue("@satelites", planeta.satelites);
                comando.Parameters.AddWithValue("@gravedad", planeta.gravedad);

                SqlDataReader dataReader = comando.ExecuteReader();

                while (dataReader.Read())
                {
                    // Obtengo los datos.
                     id = Convert.ToInt32(dataReader["id"]);
                  
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            finally
            {
                conexion.Close();
            }

            return id;
        }

    }
}
