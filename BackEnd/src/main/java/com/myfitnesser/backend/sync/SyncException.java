package com.myfitnesser.backend.sync;

/**
 * Любые ошибки синхронизации баз данных с пользовательскими устройствами
 */
public final class SyncException extends Exception {

    SyncException(Throwable cause) {
        super("Database sync error", cause);
    }
}

