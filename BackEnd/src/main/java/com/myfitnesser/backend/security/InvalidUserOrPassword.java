package com.myfitnesser.backend.security;

/**
 * Неверный пароль или пользователь
 */
public class InvalidUserOrPassword extends Exception {

    public InvalidUserOrPassword() {
        super("Invalid user or password");
    }
}
