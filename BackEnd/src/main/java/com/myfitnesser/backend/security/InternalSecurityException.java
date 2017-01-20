package com.myfitnesser.backend.security;

/**
 * Технический сбой в безопасности
 */
public class InternalSecurityException extends Exception {

    InternalSecurityException(Throwable cause) {
        super(cause);
    }
}
