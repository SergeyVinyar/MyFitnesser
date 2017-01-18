package com.myfitnesser.backend.db;

import org.hibernate.annotations.ColumnDefault;

import java.time.OffsetDateTime;
import java.util.Collections;
import java.util.List;
import java.util.Set;
import java.util.UUID;
import java.util.function.Predicate;
import java.util.function.Supplier;
import java.util.stream.Collectors;
import javax.persistence.*;

/**
 * Базовый класс записи в базе данных
 */
@MappedSuperclass
abstract class BaseEntity {

    @Id
    private UUID id;

    @Column(nullable = false)
    private OffsetDateTime lastModified;

    @Column(nullable = false)
    @ColumnDefault("false")
    private boolean deleted;

    protected BaseEntity() {
    }

    protected BaseEntity(UUID id) {
        this.id = id;
    }

    public UUID save() throws DbException {
        if(this.id == null)
            this.id = UUID.randomUUID(); // Идентификаторы могут приходить с пользовательского устройства, поэтому нельзя
                                         // использовать @GeneratedValue
        this.lastModified = OffsetDateTime.now();
        return DbService.getInstance().save(this);
    }

    public void delete() throws DbException {
        deleted = true;
        save();
    }

    protected static <T extends BaseEntity> void deleteAll(Class<T> entityClass, List<UUID> ids) throws DbException {
        if(ids == null || ids.isEmpty())
            return;
        DbService.getInstance().execute(session -> {
            session.createQuery(
                    "update " + entityClass.getSimpleName() + " set deleted = true, lastModified = :lastModified" +
                    " where id in (:ids)")
                    .setParameter("lastModified", OffsetDateTime.now())
                    .setParameter("ids", ids)
                    .executeUpdate();
            return null;
        });
    }

    protected static <T extends BaseEntity> T get(Class<T> entityClass, UUID id, Supplier<T> defaultValue) throws DbException {
        T result = DbService.getInstance().get(entityClass, id);
        if(result == null && defaultValue != null)
            result = defaultValue.get();
        return result;
    }

    protected static <T extends BaseEntity> List<T> select(Class<T> entityClass, Predicate<T> filter) throws DbException {
        return DbService.getInstance().executeWithStream(entityClass, s -> {
            if(filter != null)
                s = s.filter(filter);
            return s.collect(Collectors.toList());
        });
    }

    protected static <T extends BaseEntity> List<T> select(Class<T> entityClass) throws DbException {
        return select(entityClass, null);
    }

    public UUID getId() {
        return id;
    }

    public OffsetDateTime getLastModified() {
        return lastModified;
    }

    public boolean isDeleted() {
        return deleted;
    }
}
