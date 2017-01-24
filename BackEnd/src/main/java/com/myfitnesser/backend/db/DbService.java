package com.myfitnesser.backend.db;

import java.util.*;
import java.util.stream.Stream;

import org.hibernate.*;
import org.hibernate.boot.registry.StandardServiceRegistryBuilder;
import org.hibernate.cfg.Configuration;
import org.hibernate.service.ServiceRegistry;

/**
 * Сервис для работы с БД
 */
final class DbService {

    private static final DbService dbService = new DbService();
    private final SessionFactory sessionFactory;

    static DbService getInstance() {
        return dbService;
    }

    private DbService() {
        Configuration cfg = new Configuration();
        cfg.setProperty("hibernate.connection.driver_class", "org.postgresql.Driver");

        String host = System.getenv("DB_HOST");
        if(host != null) {
            // Production
            String port = System.getenv("DB_PORT");
            String user = System.getenv("DB_USER");
            String password = System.getenv("DB_PASSWORD");
            String database = System.getenv("DB_DATABASE");
            cfg.setProperty("hibernate.connection.url", String.format("jdbc:postgresql://%s:%s/%s", host, port, database));
            cfg.setProperty("hibernate.connection.username", user);
            cfg.setProperty("hibernate.connection.password", password);
        }
        else {
            // Development
            cfg.setProperty("hibernate.connection.url", "jdbc:postgresql://localhost:5432/sergeyv");
            cfg.setProperty("hibernate.connection.username", "sergeyv");
            cfg.setProperty("hibernate.connection.password", "");
        }

        cfg.setProperty("hibernate.show_sql", "true");
        cfg.setProperty("hibernate.hbm2ddl.auto", "update");
        cfg.setProperty("hibernate.dialect", "org.hibernate.dialect.PostgreSQL95Dialect");

        // TODO Как сделать саморегистрирующиеся классы?
        cfg.addAnnotatedClass(Client.class);
        cfg.addAnnotatedClass(Training.class);
        cfg.addAnnotatedClass(LatestSync.class);
        cfg.addAnnotatedClass(User.class);
        cfg.addAnnotatedClass(Token.class);


        StandardServiceRegistryBuilder builder = new StandardServiceRegistryBuilder();
        builder.applySettings(cfg.getProperties());
        ServiceRegistry serviceRegistry = builder.build();
        sessionFactory = cfg.buildSessionFactory(serviceRegistry);
    }

    /**
     *  Возвращает объект T по его идентификатору
     */
    <T extends BaseEntity> T get(Class<T> entityClass, UUID id) throws DbException {
        try {
            try(Session session = sessionFactory.openSession()) {
                return (T) session.get(entityClass, id);
            }
        }
        catch(HibernateException e) {
            throw new DbException(e);
        }
    }

    /**
     *  Сохраняет/создает объект T
     */
    <T extends BaseEntity> UUID save(T entity) throws DbException {
        try {
            try(Session session = sessionFactory.openSession()) {
                Transaction transaction = session.beginTransaction();
                try {
                    session.saveOrUpdate(entity);
                    transaction.commit();
                } catch(Exception e) {
                    transaction.rollback();
                    throw e;
                }
                return entity.getId();
            }
        }
        catch(HibernateException e) {
            throw new DbException(e);
        }
    }

    /**
     *  Выполняет в транзакции callback
     */
    <R> R execute(Executable<R> executable) throws DbException {
        try {
            R result = null;
            try(Session session = sessionFactory.openSession()) {
                Transaction transaction = session.beginTransaction();
                try {
                    result = executable.Execute(session);
                    transaction.commit();
                }
                catch (Exception e) {
                    transaction.rollback();
                    throw e;
                }
            }
            return result;
        }
        catch(HibernateException e) {
            throw new DbException(e);
        }
    }

    <T extends BaseEntity, R> R executeWithStream(Class<T> entityClass, ExecutableWithStream<T, R> executable) throws DbException {
        try {
            R result;
            try(Session session = sessionFactory.openSession()) {
                return executable.Execute(session.createQuery("from " + entityClass.getSimpleName(), entityClass).stream());
            }
        }
        catch(HibernateException e) {
            throw new DbException(e);
        }
    }

    @FunctionalInterface
    interface Executable<R> {
        R Execute(Session session);
    }

    @FunctionalInterface
    interface ExecutableWithStream<T, R> {
        R Execute(Stream<T> stream);
    }

}
