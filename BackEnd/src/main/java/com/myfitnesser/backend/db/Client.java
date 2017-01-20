package com.myfitnesser.backend.db;

import java.util.*;
import java.time.*;
import java.util.function.Predicate;
import java.util.function.Supplier;
import javax.persistence.*;

/**
 * Клиент
 */
@Entity
@Table(name = "client")
public final class Client extends BaseEntity {

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    @Column(length = 255)
    private String name;

    @Column(length = 50)
    private String phone;

    @Column(length = 255)
    private String email;

    @Column
    private LocalDate birthDate;

    @Column(length = 2000)
    private String notes;

    public Client() {
        super();
    }

    public Client(UUID id) {
        super(id);
    }

    public static Client get(UUID id, Supplier<Client> defaultValue) throws DbException {
        return BaseEntity.get(Client.class, id, defaultValue);
    }

    public static List<Client> select() throws DbException {
        return BaseEntity.select(Client.class);
    }

    public static List<Client> select(Predicate<Client> filter) throws DbException {
        return BaseEntity.select(Client.class, filter);
    }

    public static void deleteAll(List<UUID> ids) throws DbException {
        BaseEntity.deleteAll(Client.class, ids);
    }

    public User getUser() {
        return user;
    }

    public Client setUser(User user) {
        this.user = user;
        return this;
    }

    public String getName() {
        return name;
    }

    public Client setName(String name) {
        this.name = name;
        return this;
    }

    public String getPhone() {
        return phone;
    }

    public Client setPhone(String phone) {
        this.phone = phone;
        return this;
    }

    public String getEmail() {
        return email;
    }

    public Client setEmail(String email) {
        this.email = email;
        return this;
    }

    public LocalDate getBirthDate() {
        return birthDate;
    }

    public Client setBirthDate(LocalDate birthDate) {
        this.birthDate = birthDate;
        return this;
    }

    public String getNotes() {
        return notes;
    }

    public Client setNotes(String notes) {
        this.notes = notes;
        return this;
    }
}
