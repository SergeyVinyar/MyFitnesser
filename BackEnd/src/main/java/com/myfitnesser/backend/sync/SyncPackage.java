package com.myfitnesser.backend.sync;

import java.time.LocalDate;
import java.time.OffsetDateTime;
import java.util.UUID;

/**
 * Пакет данных синхронизации с пользовательскими устройствами.
 * Сериализуется в Json.
 */
final class SyncPackage {

    UUID deviceId;

    /**
     * Версия формата пакета
     */
    int ver = 1;

    /**
     * Идентификаторы удаленных клиентов
     */
    UUID[] cDeleted;

    /**
     * Данные добавленных/измененных клиентов
     */
    Client[] cUpdated;

    /**
     * Идентификаторы удаленных тренировок
     */
    UUID[] tDeleted;

    /**
     * Данные добавленных/измененных тренировок
     */
    Training[] tUpdated;

    /**
     * Клиент
     */
    class Client {

        /**
         * Идентификатор
         */
        UUID id;

        /**
         * ФИО
         */
        String name;

        /**
         * Телефон
         */
        String phone;

        /**
         * Email
         */
        String email;

        /**
         * Дата рождения
         */
        LocalDate birthDate;

        /**
         * Примечание
         */
        String notes;
    }

    /**
     * Тренировка
     */
    class Training {

        /**
         * Идентификатор
         */
        UUID id;

        /**
         * Идентификатор клиента
         */
        UUID clientId;

        /**
         * Дата/время начала
         */
        OffsetDateTime start;

        /**
         * Дата/время окончания
         */
        OffsetDateTime end;

        /**
         * Состояние
         */
        com.myfitnesser.backend.db.Training.State state;

        /**
         * Примечание
         */
        String notes;
    }
}
