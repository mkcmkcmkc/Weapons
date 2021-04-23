using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DatabaseSeed
/// </summary>
public class DatabaseSeed
{
    public DatabaseSeed()
    {
        //
        // TODO: Add constructor logic here
        //
        

    }


    public static void Seed()
    {

        var connString = "Data Source=(local);Initial Catalog=Master;Integrated Security=true";


        var connection = new System.Data.SqlClient.SqlConnection(connString);
        string txtDatabase = "weapons";



        //Drop databasen

        
        using (var conn = new System.Data.SqlClient.SqlConnection(connString))
        {

            bool IsExits = CheckDatabaseExists(connection, "weapons"); //Check database exists in sql server.

            if (IsExits)
            {
                String sqlCommandText = @"ALTER DATABASE " + txtDatabase + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;DROP DATABASE [" + txtDatabase + "]";
                Debug.WriteLine("Dropping");
                var command = new System.Data.SqlClient.SqlCommand(sqlCommandText, conn);
                command.Connection.Open();
                command.ExecuteNonQuery();

            }


            Debug.WriteLine("Creating");
            String sqlcreate = "create database " + txtDatabase + ";";
            var cmd = new System.Data.SqlClient.SqlCommand(sqlcreate, conn);
             cmd.Connection.Open();
             cmd.ExecuteNonQuery();

        }
        



        /*  var sqlString = "SELECT * FROM ProductSubcategory ORDER BY Name";
          using (var conn = new System.Data.SqlClient.SqlConnection(connString))
          {

              Debug.WriteLine("In here-....");
              var command = new System.Data.SqlClient.SqlCommand(sqlString, conn);
              command.Connection.Open();
              command.ExecuteNonQuery();

          }*/


        /*String CreateDatabase;
         string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      //   GrantAccess(appPath); //Need to assign the permission for current application to allow create database on server (if you are in domain).
         bool IsExits = CheckDatabaseExists(connection, "weapons"); //Check database exists in sql server.

         Debug.WriteLine(IsExits);

         if (!IsExits)
         {
             CreateDatabase = "CREATE DATABASE " + txtDatabase + " ; ";
             SqlCommand command = new SqlCommand(CreateDatabase, connection);
             try
             {
                 connection.Open();
                 command.ExecuteNonQuery();
             }
             catch (System.Exception ex)
             {
                 Debug.WriteLine(ex.StackTrace);
             }
             finally
             {
                 if (connection.State == ConnectionState.Open)
                 {
                     connection.Close();
                 }
             }

         }
        */
    }


    public static bool CheckDatabaseExists(SqlConnection tmpConn, string databaseName)
    {
        string sqlCreateDBQuery;
        bool result = false;

        try
        {
            sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);
            using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
            {
                tmpConn.Open();
                object resultObj = sqlCmd.ExecuteScalar();
                int databaseID = 0;
                if (resultObj != null)
                {
                    int.TryParse(resultObj.ToString(), out databaseID);
                }
                tmpConn.Close();
                result = (databaseID > 0);
            }
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }

}