using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using UUWebstore.Models;

namespace UUWebstore.Models.Repositories
{
    public class Repositories<TEntity>  where TEntity : class
    {
        protected readonly sbv_uuwebstoreEntities Context;
        internal DbSet<TEntity> DbSet;
        public Repositories(sbv_uuwebstoreEntities context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }
      
        public TEntity GetByID(Int64 id)
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }
        public IQueryable<TEntity> GetWithInclude(
           System.Linq.Expressions.Expression<Func<TEntity,
           bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.Database.Log = msg => Debug.Write(msg);
            Context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
            Context.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            Context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            Context.SaveChanges();
        }
        public void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            try
            {
                Context.Entry(entityToUpdate).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                var a = e.Message;
            }
        }
        public bool Exists(object primaryKey)
        {
            return DbSet.Find(primaryKey) != null;
        }

        public DbRawSqlQuery<T> SQLQuery<T>(string sql, params object[] parameters)
        { 
            return Context.Database.SqlQuery<T>(sql, parameters);
        }
    }
}