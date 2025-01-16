
using Microsoft.Extensions.Configuration;
using BitacoraData.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Context
{
    public class DBContext<T>
    {
        static conexion SQL = new conexion();
        public static string ConexionSQL()
        {
            return SQL.ConexionSQL();
        }

        public static SqlParameter AddParams(string parameterName, SqlDbType parameterType, object parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter parameters = new SqlParameter();
            parameters.ParameterName = parameterName;
            parameters.SqlDbType = parameterType;
            parameters.Value = parameterValue;
            parameters.Direction = parameterDirection;

            return parameters;
        }

        public static SqlParameter AddParamsType(string parameterName, SqlDbType parameterType, string parameterTypeName, object parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter parameters = new SqlParameter();
            parameters.ParameterName = parameterName;
            parameters.SqlDbType = parameterType;
            parameters.Value = parameterValue;
            parameters.Direction = parameterDirection;
            parameters.TypeName = parameterTypeName;

            var d = parameterValue.ToString();
            var g = parameterValue.GetHashCode();

            return parameters;
        }

        public static IEnumerable<T> CallStoreProcedure(string storedProcedure, List<SqlParameter> parameters, Func<IDataRecord, T> copyRow)
        {
            using (SqlConnection Conexion = new SqlConnection(ConexionSQL()))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, Conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    parameters.ForEach(x => cmd.Parameters.Add(x));

                    Conexion.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            yield return copyRow(rdr);
                        }
                        rdr.Close();
                    }
                }
            }
        }

        public static IEnumerable<T> CallSelectStatement(string query, Func<IDataRecord, T> copyRow)
        {
            using (SqlConnection Conexion = new SqlConnection(ConexionSQL()))
            {
                using (SqlCommand cmd = new SqlCommand(query, Conexion))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    Conexion.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            yield return copyRow(rdr);
                        }
                        rdr.Close();
                    }
                }
            }
        }

        public static DataTable CallStoreProcedureDt(string storedProcedure, List<SqlParameter> parameters)
        {
            var dataTable = new DataTable();

            using (SqlConnection Conexion = new SqlConnection(ConexionSQL()))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, Conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    parameters.ForEach(x => cmd.Parameters.Add(x));

                    Conexion.Open();

                    var dataReader = cmd.ExecuteReader();
                    dataTable.Load(dataReader);
                }
            }

            return dataTable;
        }
    }
}
