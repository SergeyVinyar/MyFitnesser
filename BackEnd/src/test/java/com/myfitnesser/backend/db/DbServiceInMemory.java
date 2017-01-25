package com.myfitnesser.backend.db;

import org.hibernate.cfg.Configuration;

import java.lang.reflect.Field;
import java.lang.reflect.Method;

import static org.junit.jupiter.api.Assertions.fail;

public class DbServiceInMemory extends DbService {

    private static DbServiceInMemory dbServiceInMemory;

    /**
     * Подключить тестовую базу в памяти вместо основной
     */
    public static void switchToMemDb() {
        if(dbServiceInMemory != null)
            fail("Повторный вызов DbServiceInMemory.switchToMemDb без вызова DbServiceInMemory.closeConnection");

        try {
            Class<?> dbServiceClass = Class.forName("com.myfitnesser.backend.db.DbService");
            Field field = dbServiceClass.getDeclaredField("dbService");
            field.setAccessible(true);
            field.set(null, dbServiceInMemory = new DbServiceInMemory());
            field.setAccessible(false);
        } catch(Exception e) {
            fail("switchToMemDb: " + e.getMessage());
        }
    }

    /**
     * Закрыть соединение
     */
    public static void closeConnection() {
        dbServiceInMemory.close();
        dbServiceInMemory = null;
    }

    @Override
    protected void initConfiguration(Configuration cfg) {
        cfg.setProperty("hibernate.connection.driver_class", "org.h2.Driver");

        cfg.setProperty("hibernate.connection.url", "jdbc:h2:mem:test");
        cfg.setProperty("hibernate.connection.username", "test");
        cfg.setProperty("hibernate.connection.password", "");

        //cfg.setProperty("hibernate.show_sql", "true");
        cfg.setProperty("hibernate.hbm2ddl.auto", "create");
        cfg.setProperty("hibernate.dialect", "org.hibernate.dialect.H2Dialect");
    }
}
