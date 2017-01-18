package com.myfitnesser.backend.db;

import java.time.*;
import javax.persistence.*;
import java.util.*;
import java.util.function.Consumer;
import java.util.function.Predicate;
import java.util.function.Supplier;

/**
 * Тренировка
 */
@Entity
@Table(name = "training")
public final class Training extends BaseEntity {

    @ManyToOne
    @JoinColumn(name = "client_id", nullable = false)
    private Client client;

    @Column
    private OffsetDateTime startDateTime;

    @Column
    private OffsetDateTime endDateTime;

    @Column
    private State state;

    @Column(length = 2000)
    private String notes;

    public Training() {
        super();
    }

    public Training(UUID id) {
        super(id);
    }

    public static Training get(UUID id, Supplier<Training> defaultValue) throws DbException {
        return BaseEntity.get(Training.class, id, defaultValue);
    }

    public static List<Training> select() throws DbException {
        return BaseEntity.select(Training.class);
    }

    public static List<Training> select(Predicate<Training> filter) throws DbException {
        return BaseEntity.select(Training.class, filter);
    }

    public static void deleteAll(List<UUID> ids) throws DbException {
        BaseEntity.deleteAll(Training.class, ids);
    }

    public Client getClient() {
        return client;
    }

    public Training setClient(Client client) {
        this.client = client;
        return this;
    }

    public OffsetDateTime getStartDateTime() {
        return startDateTime;
    }

    public Training setStartDateTime(OffsetDateTime startDateTime) {
        this.startDateTime = startDateTime;
        return this;
    }

    public OffsetDateTime getEndDateTime() {
        return endDateTime;
    }

    public Training setEndDateTime(OffsetDateTime endDateTime) {
        this.endDateTime = endDateTime;
        return this;
    }

    public State getState() {
        return state;
    }

    public Training setState(State state) {
        this.state = state;
        return this;
    }

    public String getNotes() {
        return notes;
    }

    public Training setNotes(String notes) {
        this.notes = notes;
        return this;
    }

    /**
     * Состояние тренировки
     */
    public static enum State {
        /**
         * Запланирована
         */
        Planned,

        /**
         * Выполнена
         */
        Completed
    }
}
