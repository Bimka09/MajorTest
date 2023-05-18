using Dapper;
using MajorTest.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text;

namespace MajorTest.Repositories
{
    internal class OrderDataRepository : IDisposable
    {
        private IDbConnection _dbConnection;
        private bool disposed = false;

        public OrderDataRepository()
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            _dbConnection = new NpgsqlConnection(connectionString);
            _dbConnection.Open();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                _dbConnection.Close();
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        ~OrderDataRepository()
        {
            Dispose(true);
        }
        public IEnumerable<OrderData> GetFilteredCreateOrder(FilterData filterData)
        {
            bool filterExist = true;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("select * from \"OrderData\"");
            Type myType = typeof(FilterData);
            foreach (PropertyInfo prop in myType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propName = myType.GetProperty(prop.Name);
                var propValue = propName?.GetValue(filterData);
                if (propValue != null)
                {
                    if (filterExist)
                    {
                        sbQuery.Append(@" where """ + prop.Name + @"""=@" + prop.Name);
                        filterExist = false;
                    }
                    else
                    {
                        sbQuery.Append(@" and """ + prop.Name + @"""=@" + prop.Name);
                    }
                }
            }
            return _dbConnection.Query<OrderData>(sbQuery.ToString(), new
            {
                AdressSender = filterData.AdressSender,
                AdressReceiver = filterData.AdressReceiver,
                Weight = filterData.Weight,
                Length = filterData.Length,
                Width = filterData.Width,
                Height = filterData.Height,
            });
        }
        public void CreateOrder(OrderData orderData)
        {
            var query = @"insert into ""OrderData"" (""AdressSender"", ""AdressReceiver"", ""Weight"", ""Length"", ""Width"", ""Height"", ""Volume"", ""Status"")
                            values ( @adressSender, @adressReceiver, @weight, @length, @width, @height, @volume, @status)";

            var list = _dbConnection.Execute(query, new
            {
                adressSender = orderData.AdressSender,
                adressReceiver = orderData.AdressReceiver,
                weight = orderData.Weight,
                length = orderData.Length,
                width = orderData.Width,
                height = orderData.Height,
                volume = orderData.Volume,
                status = orderData.Status,
            });
        }
        public void DeleteOrder(int[] id)
        {
            var query = @"delete from ""OrderData"" where ""Id"" = ANY(@array)";
            _dbConnection.Query(query, new { array = id });
        }
        public void EditOrderData(OrderData[] data)
        {
            for (int i = data.Length - 1; i >= 0; i--)
            {
                var item = data[i];

                var query = @"update ""OrderData"" set ""AdressSender"" = @AdressSender, ""AdressReceiver"" = @AdressReceiver, ""Weight"" = @Weight,
                            ""Length"" = @Length, ""Width"" = @Width, ""Height"" = @Height, ""Volume"" = @Volume,
                            ""Status"" = @Status, ""CancelationReason"" = @CancelationReason where ""Id"" = @Id";


                _dbConnection.Execute(query, new
                {
                    Id = item.Id,
                    AdressSender = item.AdressSender,
                    AdressReceiver = item.AdressReceiver,
                    Weight = item.Weight,
                    Length = item.Length,
                    Width = item.Width,
                    Height = item.Height,
                    Volume = item.Height * item.Width * item.Height,
                    Status = item.Status,
                    CancelationReason = item.CancelationReason
                });
            }
        }
        public void ChangeStatus(int[] id, string status)
        {
            var query = @"update ""OrderData"" set ""Status"" = @status where ""Id"" = ANY(@array)";

            _dbConnection.Execute(query, new
            {
                status = status,
                array = id
            });
        }

        public void CanselOrder(OrderData[] data)
        {
            for (int i = data.Length - 1; i >= 0; i--)
            {
                var item = data[i];

                var query = @"update ""OrderData"" set ""Status"" = @Status, ""CancelationReason"" = @CancelationReason where ""Id"" = @Id";


                _dbConnection.Execute(query, new
                {
                    Id = item.Id,
                    Status = item.Status,
                    CancelationReason = item.CancelationReason
                });
            }
        }
    }
}
