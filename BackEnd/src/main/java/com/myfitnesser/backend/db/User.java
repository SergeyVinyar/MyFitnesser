package com.myfitnesser.backend.db;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Table;
import java.time.OffsetDateTime;
import java.util.List;

/**
 * Пользователь сервиса (данные для аутентификации на сервере)
 */
@Entity
@Table(name = "system_user")
public final class User extends BaseEntity {

    @Column(length = 255)
    private String email;

    @Column(length = 50)
    private byte[] passHash;

    @Column(length = 50)
    private byte[] salt;

    @Column
    private OffsetDateTime registrationDateTime;

    public User() {
        super();
    }

    /**
     * Возвращает пользователя с указанным email
     * @return
     * null, если не найден
     */
    public static User getByEmail(String email) throws DbException {
        List<User> result = select(User.class, v -> !v.isDeleted() && v.getEmail().equals(email));
        return result.size() != 0 ? result.get(0) : null;
    }

    public String getEmail() {
        return email;
    }

    public User setEmail(String email) {
        this.email = email;
        return this;
    }

    public byte[] getPassHash() {
        return passHash;
    }

    public User setPassHash(byte[] passHash) {
        this.passHash = passHash;
        return this;
    }

    public byte[] getSalt() {
        return salt;
    }

    public User setSalt(byte[] salt) {
        this.salt = salt;
        return this;
    }

    public OffsetDateTime getRegistrationDateTime() {
        return registrationDateTime;
    }

    public User setRegistrationDateTime(OffsetDateTime dateOfRegistration) {
        this.registrationDateTime = dateOfRegistration;
        return this;
    }
}
