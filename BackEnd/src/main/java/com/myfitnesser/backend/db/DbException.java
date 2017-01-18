package com.myfitnesser.backend.db;

/**
 * Любые ошибки работы с БД
 */
public final class DbException extends Exception {

    DbException(Throwable cause) {
        super("Database access error", cause);
    }
}
