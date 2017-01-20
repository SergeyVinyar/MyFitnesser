package com.myfitnesser.backend.security;

/**
 * Данный логин (email) уже используется
 */
public class LoginIsAlreadyUsed extends Exception {

    public LoginIsAlreadyUsed() {
        super("Login is already used");
    }
}
