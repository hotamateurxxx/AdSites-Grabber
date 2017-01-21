using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Criterion;

using log4net;
using log4net.Config;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Менеджер для работы с БД.
    /// </summary>
    interface IDatabaseManager : IDisposable
    {

        /// <summary>
        /// Сохранение объекта (и всех его членов) в БД.
        /// </summary>
        /// <param name="item">Сохраняемый объект.</param>
        /// <typeparam name="T">Тип сохраняемого объекта.</typeparam>
        void Save<T>(T item);

    }

    /// <summary>
    /// Менеджер для работы с БД.
    /// </summary>
    class DatabaseManager : IDatabaseManager
    {

        /// <summary>
        /// Инстанция.
        /// </summary>
        private static DatabaseManager _instance;

        /// <summary>
        /// Фабрика сессий NHibernate.
        /// </summary>
        protected ISessionFactory sessionFactory;

        /// <summary>
        /// Создать если экземпляр не создан. Вернуть ссылку на экземпляр.
        /// </summary>
        /// <returns>Ссылка на экземпляр.</returns>
        public static DatabaseManager GetInstance()
        {
            _instance = _instance ?? new DatabaseManager();
            return _instance;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        private DatabaseManager()
        {
            _instance = this;
            try
            {
                Configuration configuration = new Configuration().Configure();
                sessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception e)
            {
                throw new Exception("Не удается инициализировать драйвер базы данных.", e);
            }
        }
        
        /// <summary>
        /// Закрытие менеджера.
        /// </summary>
        public void Close()
        {
            sessionFactory.Close();
        }

        /// <summary>
        /// Освобождение менеджера.
        /// </summary>
        public void Dispose()
        {
            sessionFactory.Dispose();
        }

        /// <summary>
        /// Удаление из БД объекта заданного типа.
        /// </summary>
        /// <param name="item">Удаляемый объект.</param>
        /// <typeparam name="T">Тип удаляемого объекта.</typeparam>
        public void Delete<T>(T item)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.Delete(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Удаление из БД списка объектов заданного типа.
        /// </summary>
        /// <param name="items">Список удаляемых объектов.</param>
        /// <typeparam name="T">Тип удаляемых объектов.</typeparam>
        public void Delete<T>(IList<T> items)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                foreach (T item in items)
                {
                    using (session.BeginTransaction())
                    {
                        session.Delete(item);
                        session.Transaction.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Извлечение из БД объектов заданного типа и удовлетворяющих условиям.
        /// </summary>
        /// <typeparam name="T">Тип извлекаемых объектов.</typeparam>
        /// <param name="propertyName">Имя свойства условия.</param>
        /// <param name="propertyValue">Значение свойства условия.</param>
        /// <returns>Список объектов.</returns>
        public IList<T> RetrieveEquals<T>(string propertyName, object propertyValue)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.Add(Expression.Eq(propertyName, propertyValue));
                IList<T> matchingObjects = criteria.List<T>();
                return matchingObjects;
            }
        }

        /// <summary>
        /// Сохранение объекта (и всех его членов) в БД.
        /// </summary>
        /// <param name="item">Сохраняемый объект.</param>
        /// <typeparam name="T">Тип сохраняемого объекта.</typeparam>
        public void Save<T>(T item)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(item);
                    session.Transaction.Commit();
                }
            }
        }

    }
}
