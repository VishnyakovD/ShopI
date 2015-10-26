using System;
using NHibernate;
using Shop.db.Entities;
using Shop.db.Repository;

namespace Shop.db
{
    public interface IDb
    {
        IRepository<T> GetRepository<T>() where T : class;
        void Run(Action<DB> code);
        void RunTransaction(Action<DB> code);
    }

    public class DB : IDb
    {
        public ISession session = null;

        public ITransaction transaction = null;

        public int sessionCount = 0;

        public DB(string connstring)
        {
            NHibernateHelper.ConnectionString = connstring;
        }

        public static void GeterateDBScript()
        {
            NHibernateHelper.GenerateDB(true);
        }

        public static void GeterateDB()
        {
            NHibernateHelper.GenerateDB(false);
        }

        public static void SetConnectionString(string connstring)
        {
            NHibernateHelper.ConnectionString = connstring;
        }

        public static string GetConnectionString()
        {
            return NHibernateHelper.ConnectionString;
        }

        public static bool IsConfigured
        {
            get
            {
                return NHibernateHelper.IsConfigured;
            }
        }

        public void Open()
        {
            if (session == null)
                session = NHibernateHelper.OpenSession();
            sessionCount++;
        }

        public void OpenWithTransaction()
        {
            Open();
            if (transaction == null)
                transaction = session.BeginTransaction();
        }

        public void Close()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
            if (--sessionCount == 0)
            {
                session.Close();
                session = null;
            }
        }

        public void CloseRollBack()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
            if (session != null)
            {
                sessionCount = 0;
                session.Close();
                session = null;
            }
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            IRepository<T> retvar = null;
            if (typeof(T) == typeof(Brand))
                retvar = (IRepository<T>)(new BrandRepository(session));
            else if (typeof(T) == typeof(Category))
                retvar = (IRepository<T>)(new CategoryRepository(session));
            else if (typeof(T) == typeof(Comment))
                retvar = (IRepository<T>)(new CommentRepository(session));
            else if (typeof(T) == typeof(Photo))
                retvar = (IRepository<T>)(new PhotoRepository(session));
            else if (typeof(T) == typeof(Sku))
                retvar = (IRepository<T>)(new SkuRepository(session));
            else if (typeof(T) == typeof(Specification))
                retvar = (IRepository<T>)(new SpecificationRepository(session));
            else if (typeof(T) == typeof(StaticSpecification))
                retvar = (IRepository<T>)(new StaticSpecificationRepository(session));
            else if (typeof(T) == typeof(StaticCategory))
                retvar = (IRepository<T>)(new StaticCategoryRepository(session));
            else if (typeof(T) == typeof(PhotoBig))
                retvar = (IRepository<T>)(new PhotoBigRepository(session));
            else if (typeof(T) == typeof(CartItem))
                retvar = (IRepository<T>)(new CartItemRepository(session));
            else if (typeof(T) == typeof(Cart))
                retvar = (IRepository<T>)(new CartRepository(session));
            else if (typeof(T) == typeof(CartState))
                retvar = (IRepository<T>)(new CartStateRepository(session));
            else
                retvar = new Repository<T>(session);
            return retvar;
        }

        public IRepository GetRepository(Type elementType)
        {
            var repositoryType = typeof(Repository<>).MakeGenericType(elementType);
            return (IRepository)Activator.CreateInstance(repositoryType, session );            
        }

        public void Run(Action<DB> code)
        {
            Open();
            try
            {
                code(this);
            }
            finally
            {
                Close();
            }
        }

        public void RunTransaction(Action<DB> code)
        {
            OpenWithTransaction();
            try
            {
                code(this);
                Close();
            }
            catch (Exception err)
            {
                CloseRollBack();
                throw err;
            }
        }
    }
}
