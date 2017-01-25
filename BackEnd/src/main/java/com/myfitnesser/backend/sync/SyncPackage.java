package com.myfitnesser.backend.sync;

import java.time.LocalDate;
import java.time.OffsetDateTime;
import java.util.UUID;

/**
 * Пакет данных синхронизации с пользовательскими устройствами.
 * Сериализуется в Json.
 */
public final class SyncPackage {

    public UUID deviceId;

    /**
     * Версия формата пакета
     */
    public int ver = 1;

    /**
     * Идентификаторы удаленных клиентов
     */
    public UUID[] clientsDeleted;

    /**
     * Данные добавленных/измененных клиентов
     */
    public Client[] clientsUpdated;

    /**
     * Идентификаторы удаленных тренировок
     */
    public UUID[] trainingsDeleted;

    /**
     * Данные добавленных/измененных тренировок
     */
    public Training[] trainingsUpdated;

    /**
     * Клиент
     */
    public class Client {

        /**
         * Идентификатор
         */
        public UUID id;

        /**
         * ФИО
         */
        public String name;

        /**
         * Телефон
         */
        public String phone;

        /**
         * Email
         */
        public String email;

        /**
         * Дата рождения
         */
        public LocalDate birthDate;

        /**
         * Примечание
         */
        public String notes;
    }

    /**
     * Тренировка
     */
    public class Training {

        /**
         * Идентификатор
         */
        public UUID id;

        /**
         * Идентификатор клиента
         */
        public UUID clientId;

        /**
         * Дата/время начала
         */
        public OffsetDateTime start;

        /**
         * Дата/время окончания
         */
        public OffsetDateTime end;

        /**
         * Состояние
         */
        public com.myfitnesser.backend.db.Training.State state;

        /**
         * Примечание
         */
        public String notes;
    }
}
