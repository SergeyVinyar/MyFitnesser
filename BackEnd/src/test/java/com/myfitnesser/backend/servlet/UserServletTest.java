package com.myfitnesser.backend.servlet;

import com.myfitnesser.backend.db.DbServiceInMemory;
import com.myfitnesser.backend.db.Token;
import com.myfitnesser.backend.db.User;
import com.myfitnesser.backend.security.SecurityService;
import org.junit.jupiter.api.*;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import java.io.PrintWriter;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class UserServletTest {

    @Test
    @DisplayName("Регистрация нового пользователя")
    void registerUser() throws Exception {
        DbServiceInMemory.switchToMemDb();
        try {

            HttpServletRequest request = mock(HttpServletRequest.class);
            when(request.getParameter("action")).thenReturn("register");
            when(request.getParameter("email")).thenReturn("test@test.com");
            when(request.getParameter("password")).thenReturn("1122334455ABCабв");

            HttpServletResponse response = mock(HttpServletResponse.class);
            when(response.getWriter()).thenReturn(mock(PrintWriter.class));

            new UserServlet().doPost(request, response);

            verify(response).setStatus(HttpServletResponse.SC_OK);

            // Token в base64 == 44 символа (т.к. исходно токен 32 байта или 11 8-битных троек, которые превратились в 11 6-битных четверок)
            verify(response.getWriter()).write(matches("^[0-9a-zA-Z+/=]{44}$"));

            User user = User.getByEmail("test@test.com");
            assertNotNull(user);
            assertEquals(Token.select(t -> t.getUser().getId().equals(user.getId())).size(), 1);

        } finally {
            DbServiceInMemory.closeConnection();
        }
    }

}