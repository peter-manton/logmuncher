using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data.Common;


namespace SquidLogAnalyzer
{
    class MysqlFactory
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public MysqlFactory()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "logmuncher";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(LogRecord log)
        {
            string query = "INSERT `log_collection` SET `DateStamp`=@dateStamp, `Duration`=@duration, `ClientAddress`=@clientAddress, `SquidReturnCode`=@squidReturnCode, `HTTPReturnCode`=@httpReturnCode, `Bytes`=@bytes, `RequestMethod`=@requestMethod, `URL`=@url, `RFC931`=@rfc931, `HierarchyCode`=@hierarchyCode, `Type`=@type";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                // Parametized queries 
                cmd.Parameters.AddWithValue("@dateStamp", log.dateStamp);
                cmd.Parameters.AddWithValue("@duration", log.duration);
                cmd.Parameters.AddWithValue("@clientAddress", log.clientAddress);
                cmd.Parameters.AddWithValue("@squidReturnCode", log.squidReturnCode);
                cmd.Parameters.AddWithValue("@httpReturnCode", log.httpReturnCode);
                cmd.Parameters.AddWithValue("@bytes", log.bytes);
                cmd.Parameters.AddWithValue("@requestMethod", log.requestMethod);
                cmd.Parameters.AddWithValue("@url", log.url);
                cmd.Parameters.AddWithValue("@rfc931", log.rfc931);
                cmd.Parameters.AddWithValue("@hierarchyCode", log.hierarchyCode);
                cmd.Parameters.AddWithValue("@type", log.type);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

 

        //Delete statement
        public void Delete()
        {

        }

        //Select statement
        public List<string>[] SelectTopUrls(DateTime datetime)
        {
            // SELECT *, COUNT(URL) as URL_Count FROM log_collection WHERE DateStamp BETWEEN "2013-08-29 00:00:01" AND "2013-08-29 23:59:59" GROUP BY URL ORDER BY url_count DESC LIMIT 25
            string query = "SELECT *, COUNT(URL) as URL_Count FROM log_collection" + @" WHERE DateStamp BETWEEN """ + datetime.ToString("yyyy-M-d") + @" 00:00:01"" AND """ + datetime.ToString("yyyy-M-d") + @" 23:59:59"" GROUP BY URL ORDER BY url_count DESC LIMIT 25";

            //Create a list to store the result
            List<string>[] list = new List<string>[11];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            list[8] = new List<string>();
            list[9] = new List<string>();
            list[10] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["ID"] + "");
                    list[1].Add(dataReader["DateStamp"] + "");
                    list[2].Add(dataReader["ClientAddress"] + "");
                    list[3].Add(dataReader["SquidReturnCode"] + "");
                    list[4].Add(dataReader["HTTPReturnCode"] + "");
                    list[5].Add(dataReader["Bytes"] + "");
                    list[6].Add(dataReader["RequestMethod"] + "");
                    list[7].Add(dataReader["URL"] + "");
                    list[8].Add(dataReader["RFC931"] + "");
                    list[9].Add(dataReader["HierarchyCode"] + "");
                    list[9].Add(dataReader["Type"] + "");
                    list[9].Add(dataReader["ID"] + "");
                    list[10].Add(dataReader["URL_Count"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Select statement
        public List<string>[] SelectTopBytes(DateTime datetime)
        {
            string query = "SELECT * FROM log_collection WHERE " + @"DateStamp BETWEEN """ + datetime.ToString("yyyy-M-d") + @" 00:00:01"" AND """ + datetime.ToString("yyyy-M-d") + @" 23:59:59""" + " ORDER BY CAST(`Bytes` AS decimal) DESC LIMIT 25";
            Console.WriteLine(query);
            Console.ReadKey();

            //Create a list to store the result
            List<string>[] list = new List<string>[10];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            list[8] = new List<string>();
            list[9] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["ID"] + "");
                    list[1].Add(dataReader["DateStamp"] + "");
                    list[2].Add(dataReader["ClientAddress"] + "");
                    list[3].Add(dataReader["SquidReturnCode"] + "");
                    list[4].Add(dataReader["HTTPReturnCode"] + "");
                    list[5].Add(dataReader["Bytes"] + "");
                    list[6].Add(dataReader["RequestMethod"] + "");
                    list[7].Add(dataReader["URL"] + "");
                    list[8].Add(dataReader["RFC931"] + "");
                    list[9].Add(dataReader["HierarchyCode"] + "");
                    list[9].Add(dataReader["Type"] + "");
                    list[9].Add(dataReader["ID"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            return 1;
        }

        //Backup
        public void Backup()
        {

        }

        //Restore
        public void Restore()
        {

        }

    }
}
