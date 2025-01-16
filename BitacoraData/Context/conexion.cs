using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BitacoraData.Context
{
    public class conexion
    {
        public String ConexionSQL()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            return configuration["ConnectionStrings:BitacoraDatabase"];
        }
    }
}
