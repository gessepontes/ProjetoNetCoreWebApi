using mp.ce.fdid.Data.Mappings;
using mp.ce.fdid.Domain.Interfaces.Repositories.Common;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
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
            if (FluentMapper.EntityMaps.Count == 0)
            {
                FluentMapper.Initialize(c =>
                {
                    c.AddMap(mapper: new EstadoMap());
                    c.AddMap(mapper: new CidadeMap());
                    c.AddMap(mapper: new InstituicaoMap());
                    c.AddMap(mapper: new ProjetoMap());
                    c.AddMap(mapper: new ArquivoMap());
                    c.AddMap(mapper: new PrazoMap());

                    c.ForDommel();
                });
            }

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
