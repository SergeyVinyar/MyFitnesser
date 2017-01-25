package com.myfitnesser.backend.servlet;

import com.myfitnesser.backend.db.DbException;
import com.myfitnesser.backend.security.InternalSecurityException;
import com.myfitnesser.backend.security.InvalidUserOrPassword;
import com.myfitnesser.backend.security.LoginIsAlreadyUsed;
import com.myfitnesser.backend.security.SecurityService;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.Arrays;
import java.util.Base64;

/**
 * Регистрация/разрегистрация пользователей
 */
final public class UserServlet extends HttpServlet {

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        try {
            String action = req.getParameter("action");

            // Регистрация нового пользователя
            if (action.equalsIgnoreCase("register")) {
                String email = req.getParameter("email") != null ? req.getParameter("email") : "";
                char[] password = req.getParameter("password") != null ? req.getParameter("password").toCharArray() : (new char[0]);
                try {
                    if (email.isEmpty() || password.length < 6) { // TODO Добавить проверку email
                        resp.sendError(HttpServletResponse.SC_BAD_REQUEST, "Email is empty or password length less then 6 symbols");
                        return;
                    }
                    byte[] tokenData = SecurityService.registerNewUser(email, password);
                    resp.getWriter().write(Base64.getEncoder().encodeToString(tokenData));
                    resp.setStatus(HttpServletResponse.SC_OK);
                } finally {
                    Arrays.fill(password, ' ');
                }

            // Логин в систему
            } else if (action.equalsIgnoreCase("login")) {
                String email = req.getParameter("email") != null ? req.getParameter("email") : "";
                char[] password = req.getParameter("password") != null ? req.getParameter("password").toCharArray() : (new char[0]);
                try {
                    byte[] tokenData = SecurityService.loginUser(email, password);
                    resp.getWriter().write(Base64.getEncoder().encodeToString(tokenData));
                    resp.setStatus(HttpServletResponse.SC_OK);
                } finally {
                    Arrays.fill(password, ' ');
                }

            // Выход из системы
            } else if (action.equalsIgnoreCase("logout")) {
                byte[] tokenData = req.getParameter("token") != null ? req.getParameter("token").getBytes() : (new byte[0]);
                tokenData = Base64.getDecoder().decode(tokenData);
                SecurityService.logoutUser(tokenData);
                resp.setStatus(HttpServletResponse.SC_OK);

            // Удаление пользователя
            } else if (action.equalsIgnoreCase("delete")) {
                byte[] tokenData = req.getParameter("token") != null ? req.getParameter("token").getBytes() : (new byte[0]);
                tokenData = Base64.getDecoder().decode(tokenData);
                SecurityService.deleteUser(tokenData);
                resp.setStatus(HttpServletResponse.SC_OK);

            } else {
                resp.sendError(HttpServletResponse.SC_BAD_REQUEST, "Action is not defined or recognized");
            }
        }
        catch(InvalidUserOrPassword e) {
            resp.sendError(HttpServletResponse.SC_UNAUTHORIZED, e.getMessage());
        }
        catch(DbException e) {
            throw new IOException(e);
        }
        catch(InternalSecurityException e) {
            throw new ServletException(e);
        }
        catch(LoginIsAlreadyUsed e) {
            resp.sendError(HttpServletResponse.SC_BAD_REQUEST, e.getMessage());
        }
    }
}