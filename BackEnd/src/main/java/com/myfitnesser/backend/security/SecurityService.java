package com.myfitnesser.backend.security;

import com.myfitnesser.backend.db.DbException;
import com.myfitnesser.backend.db.Token;
import com.myfitnesser.backend.db.User;

import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.spec.InvalidKeySpecException;
import java.time.OffsetDateTime;
import java.util.Arrays;

/**
 * Сервис (helper) аутентификации
 */
public final class SecurityService {

    private SecurityService() {
    }

    /**
     * Регистрация нового пользователя
     * @param email
     * Почта в качестве логина
     * @param password
     * Пароль
     * @return
     * Токен
     */
    public static byte[] registerNewUser(String email, char[] password) throws LoginIsAlreadyUsed, DbException, InternalSecurityException {
        if(User.getByEmail(email) != null)
            throw new LoginIsAlreadyUsed();

        byte[] salt = generateSalt();

        User user = new User();
        user.setEmail(email);
        user.setSalt(salt);
        user.setPassHash(generateHash(password, salt));
        user.setRegistrationDateTime(OffsetDateTime.now());
        user.save();

        return getNewTokenDataForUser(user);
    }

    /**
     * Вход пользователя в систему
     * @param email
     * Почта в качестве логина
     * @param password
     * Пароль
     * @return
     * Токен
     */
    public static byte[] loginUser(String email, char[] password) throws InvalidUserOrPassword, DbException, InternalSecurityException {
        byte[] userSalt;
        byte[] userHash;

        User user = User.getByEmail(email);
        if(user == null) {
            // Обработки ситуаций отсутствия пользователя и неверного пароля не должны сильно отличаться по продолжительности
            userSalt = generateSalt();
            userHash = new byte[50];
        }
        else {
            userSalt = user.getSalt();
            userHash = user.getPassHash();
        }

        byte[] hash = generateHash(password, userSalt);
        if(!Arrays.equals(hash, userHash))
            throw new InvalidUserOrPassword();

        if(user != null)
            return getNewTokenDataForUser(user);

        return null; // Сюда мы не должны попадать
    }

    /**
     * Выход пользователя из системы
     * @param tokenData
     * Токен
     */
    public static void logoutUser(byte[] tokenData) throws DbException {
        Token token = getTokenFromTokenData(tokenData);
        if(token != null)
            token.delete();
    }

    /**
     * Удаление пользователя из системы (данные в базе сохраняются)
     * @param tokenData
     * Токен
     */
    public static void deleteUser(byte[] tokenData) throws DbException {
        User user = getUserFromTokenData(tokenData);
        if(user != null)
            user.delete();
    }

    /**
     * Возвращает пользователя по токену
     * @return
     * null, если токен невалиден или просрочился
     */
    public static User getUserFromTokenData(byte[] tokenData) throws DbException {
        Token token = getTokenFromTokenData(tokenData);
        if(token == null)
            return null;
        User user = token.getUser();
        if(user == null)
            return null;
        if(user.isDeleted())
            return null;
        return user;
    }

    private static byte[] generateSalt() {
        SecureRandom sr = new SecureRandom();
        byte[] salt = new byte[32];
        sr.nextBytes(salt);
        return salt;
    }

    private static byte[] generateHash(char[] password, byte[] salt) throws InternalSecurityException {
        PBEKeySpec keySpec = new PBEKeySpec(password, salt, 10000, 256);
        try {
            SecretKeyFactory factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
            return factory.generateSecret(keySpec).getEncoded();
        }
        catch (NoSuchAlgorithmException | InvalidKeySpecException e) {
            throw new InternalSecurityException(e);
        }
        finally {
            keySpec.clearPassword();
        }
    }

    private static byte[] getNewTokenDataForUser(User user) throws DbException {
        SecureRandom sr = new SecureRandom();
        byte[] data = new byte[32];
        sr.nextBytes(data);

        new Token()
                .setTokenData(data)
                .setUser(user)
                .setExpirationDateTime(OffsetDateTime.now().plusMonths(1))
                .save();

        return data;
    }

    private static Token getTokenFromTokenData(byte[] tokenData) throws DbException {
        return Token.getByTokenData(tokenData);
    }
}
