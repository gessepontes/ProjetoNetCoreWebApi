using mp.ce.fdid.Domain.Interfaces.Repositories.Common;
using Dommel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace mp.ce.fdid.Data.Repositories.Common
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly SqlConnection conn;

        public RepositoryBase()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            conn = new SqlConnection(config.GetSection(key: "ConnectionStrings")["DefaultConnection"]);

        }

        public virtual void Add(TEntity obj) => conn.Insert(obj);

        public virtual IEnumerable<TEntity> GetAll() => conn.GetAll<TEntity>();

        public virtual TEntity GetById(int? id) => conn.Get<TEntity>(id);

        public virtual void Remove(TEntity obj) => conn.Delete(obj);

        public virtual void Update(TEntity obj) => conn.Update(obj);

        public void Dispose()
        {
            conn.Close();
            conn.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
