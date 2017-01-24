package com.myfitnesser.backend.db;

import javax.persistence.*;
import java.time.OffsetDateTime;
import java.util.List;
import java.util.function.Predicate;

/**
 * Токен
 */
@Entity
@Table(name = "token")
public final class Token extends BaseEntity {

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    @Column(length = 32, nullable = false)
    private byte[] tokenData;

    @Column(nullable = false)
    private OffsetDateTime expirationDateTime;

    public Token() {
    }

    public static List<Token> select(Predicate<Token> filter) throws DbException {
        return BaseEntity.select(Token.class, filter);
    }

    /**
     * @param tokenData
     * null, если не найден
     */
    public static Token getByTokenData(byte[] tokenData) throws DbException {
        return DbService.getInstance().execute(session -> {
            List result = session.createQuery(
                    "from " + Token.class.getSimpleName() + " where tokendata = :tokendata and expirationdatetime > :expirationdatetime and deleted = false")
                    .setParameter("tokendata", tokenData)
                    .setParameter("expirationdatetime", OffsetDateTime.now())
                    .list();
            return result.size() == 1 ? (Token) result.get(0) : null; // Коллизия токенов (size > 1)? Пусть пользователь логинится заново
        });
    }

    public User getUser() {
        return user;
    }

    public Token setUser(User user) {
        this.user = user;
        return this;
    }

    public byte[] getTokenData() {
        return tokenData;
    }

    public Token setTokenData(byte[] tokenData) {
        this.tokenData = tokenData;
        return this;
    }

    public OffsetDateTime getExpirationDateTime() {
        return expirationDateTime;
    }

    public Token setExpirationDateTime(OffsetDateTime expirationDateTime) {
        this.expirationDateTime = expirationDateTime;
        return this;
    }
}
