using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAndSavePrice.DBAdapter
{
    internal class MySQLAdapter
    {
        MySqlConnection _connection;

        private bool _connected = false;
        public MySQLAdapter(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }
        private void Connect()
        {
            try
            {
                _connection.Open();
                _connected = true;
                Console.WriteLine("Подключен к БД");
                Console.WriteLine("\tБаза данных: {0}", _connection.Database);
                Console.WriteLine("\tСервер: {0}", _connection.DataSource);
                Console.WriteLine("\tВерсия сервера: {0}", _connection.ServerVersion);
                Console.WriteLine("\tСостояние: {0}", _connection.State);
            }
            catch (MySqlException ex)
            {
                _connection.Close();
                _connected = false;
                Console.WriteLine(ex.Message);
                Console.WriteLine("Подключение закрыто");
            }
        }

        private void Disconnect() 
        {
            if (_connection != null && _connected == true) _connection.Close();
        }

        public void SaveDataToDB(List<PriceItem> priceItems)
        {
            Connect();

            if (_connected == false)
            {
                Console.WriteLine("Нет подключения к БД");
                return;
            }

            foreach (var item in priceItems)
            {
                string query = "INSERT INTO priceitems (Vendor, Number, SearchVendor, SearchNumber, Description, Price, Count) " +
                               "VALUES (@Vendor, @Number, @SearchVendor, @SearchNumber, @Description, @Price, @Count)";

                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Vendor", item.Vendor);
                    command.Parameters.AddWithValue("@Number", item.Number);
                    command.Parameters.AddWithValue("@SearchVendor", item.SearchVendor);
                    command.Parameters.AddWithValue("@SearchNumber", item.SearchNumber);
                    command.Parameters.AddWithValue("@Description", item.Description);
                    command.Parameters.AddWithValue("@Price", item.Price);
                    command.Parameters.AddWithValue("@Count", item.Count);

                    command.ExecuteNonQuery();
                }
            }
            Disconnect();
            Console.WriteLine($"{priceItems.Count()} записей сохранено в БД");
        }
    }
}
