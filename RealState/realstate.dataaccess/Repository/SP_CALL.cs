﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using realstate.dataaccess.Data;
using realstate.dataaccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.dataaccess.Repository
{
   public class SP_CALL:ISP_Call
    {
        private readonly ApplicationDbContext _db;
        private static string ConnectionString;
        public SP_CALL(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = _db.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Execute(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn=new SqlConnection(ConnectionString) )
            {
                sqlConn.Open();
                sqlConn.Execute(procedureName, param,commandType: CommandType.StoredProcedure);


            }
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                return sqlConn.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);


            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                var result = SqlMapper.QueryMultiple(sqlConn, procedureName, param, commandType: CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();
                if (item1 != null && item2!=null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }


                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(),new List<T2>());

            }
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                var value = sqlConn.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
                return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));


            }
        }

        public T Single<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                var value = sqlConn.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure);
                return (T)Convert.ChangeType(value, typeof(T));




            }
        }
    }
}
