package com.myfitnesser.backend.servlet;

import com.myfitnesser.backend.db.DbServiceInMemory;
import com.myfitnesser.backend.db.Token;
import com.myfitnesser.backend.db.User;
import com.myfitnesser.backend.security.InvalidUserOrPassword;
import com.myfitnesser.backend.security.SecurityService;
import org.junit.jupiter.api.*;
import org.mockito.ArgumentCaptor;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import java.io.PrintWriter;
import java.util.Base64;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class UserServletTest {

    @BeforeEach
    void beforeEach() {
        DbServiceInMemory.switchToMemDb();
    }

    @AfterEach
    void afterEach() {
        DbServiceInMemory.closeConnection();
    }

    @Test
    @DisplayName("Регистрация нового пользователя")
    void registerUser() throws Exception {
        HttpServletRequest request = mock(HttpServletRequest.class);
        when(request.getParameter("action")).thenReturn("register");
        when(request.getParameter("email")).thenReturn("test@test.com");
        when(request.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse response = mock(HttpServletResponse.class);
        when(response.getWriter()).thenReturn(mock(PrintWriter.class));

        new UserServlet().doPost(request, response);

        verify(response).setStatus(HttpServletResponse.SC_OK);
        checkToken("test@test.com", response);
    }

    @Test
    @DisplayName("Повторная регистрация одного и того же пользователя")
    void registerSameUserSecondTime() throws Exception {
        HttpServletRequest request = mock(HttpServletRequest.class);
        when(request.getParameter("action")).thenReturn("register");
        when(request.getParameter("email")).thenReturn("test@test.com");
        when(request.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse response1 = mock(HttpServletResponse.class);
        when(response1.getWriter()).thenReturn(mock(PrintWriter.class));

        new UserServlet().doPost(request, response1);

        verify(response1).setStatus(HttpServletResponse.SC_OK);

        HttpServletResponse response2 = mock(HttpServletResponse.class);
        new UserServlet().doPost(request, response2);

        verify(response2).sendError(HttpServletResponse.SC_BAD_REQUEST, "Login is already used");
    }

    @Test
    @DisplayName("Вход существующего пользователя с неверным паролем")
    void loginExistedUserWrongPass() throws Exception {
        // Регистрируем
        HttpServletRequest requestRegister = mock(HttpServletRequest.class);
        when(requestRegister.getParameter("action")).thenReturn("register");
        when(requestRegister.getParameter("email")).thenReturn("test@test.com");
        when(requestRegister.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse responseRegister = mock(HttpServletResponse.class);
        when(responseRegister.getWriter()).thenReturn(mock(PrintWriter.class));

        new UserServlet().doPost(requestRegister, responseRegister);

        verify(responseRegister).setStatus(HttpServletResponse.SC_OK);

        // Входим
        HttpServletRequest requestLogin = mock(HttpServletRequest.class);
        when(requestLogin.getParameter("action")).thenReturn("login");
        when(requestLogin.getParameter("email")).thenReturn("test@test.com");
        when(requestLogin.getParameter("password")).thenReturn("FakePass");

        HttpServletResponse responseLogin = mock(HttpServletResponse.class);

        new UserServlet().doPost(requestLogin, responseLogin);

        verify(responseLogin).sendError(HttpServletResponse.SC_UNAUTHORIZED, "Invalid user or password");
    }

    @Test
    @DisplayName("Вход существующего пользователя с верным паролем")
    void loginExistedUser() throws Exception {
        // Регистрируем
        HttpServletRequest requestRegister = mock(HttpServletRequest.class);
        when(requestRegister.getParameter("action")).thenReturn("register");
        when(requestRegister.getParameter("email")).thenReturn("test@test.com");
        when(requestRegister.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse responseRegister = mock(HttpServletResponse.class);
        when(responseRegister.getWriter()).thenReturn(mock(PrintWriter.class));

        new UserServlet().doPost(requestRegister, responseRegister);

        verify(responseRegister).setStatus(HttpServletResponse.SC_OK);
        ArgumentCaptor<String> writeArgumentCaptor = ArgumentCaptor.forClass(String.class);
        verify(responseRegister.getWriter()).write(writeArgumentCaptor.capture());

        // Выходим
        HttpServletRequest requestLogout = mock(HttpServletRequest.class);
        when(requestLogout.getParameter("action")).thenReturn("logout");
        when(requestLogout.getParameter("token")).thenReturn(writeArgumentCaptor.getValue());

        HttpServletResponse responseLogout = mock(HttpServletResponse.class);

        new UserServlet().doPost(requestLogout, responseLogout);

        verify(responseLogout).setStatus(HttpServletResponse.SC_OK);

        // Входим
        HttpServletRequest requestLogin = mock(HttpServletRequest.class);
        when(requestLogin.getParameter("action")).thenReturn("login");
        when(requestLogin.getParameter("email")).thenReturn("test@test.com");
        when(requestLogin.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse responseLogin = mock(HttpServletResponse.class);
        when(responseLogin.getWriter()).thenReturn(mock(PrintWriter.class));

        new UserServlet().doPost(requestLogin, responseLogin);

        verify(responseLogin).setStatus(HttpServletResponse.SC_OK);
        checkToken("test@test.com", responseLogin);
    }

    @Test
    @DisplayName("Вход несуществующего пользователя")
    void loginNonExistedUser() throws Exception {
        HttpServletRequest requestLogin = mock(HttpServletRequest.class);
        when(requestLogin.getParameter("action")).thenReturn("login");
        when(requestLogin.getParameter("email")).thenReturn("test@test.com");
        when(requestLogin.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse responseLogin = mock(HttpServletResponse.class);

        new UserServlet().doPost(requestLogin, responseLogin);

        verify(responseLogin).sendError(HttpServletResponse.SC_UNAUTHORIZED, "Invalid user or password");
    }

    @Test
    @DisplayName("Выход пользователя")
    void logoutUser() throws Exception {
        // Регистрируем
        HttpServletRequest requestRegister = mock(HttpServletRequest.class);
        when(requestRegister.getParameter("action")).thenReturn("register");
        when(requestRegister.getParameter("email")).thenReturn("test@test.com");
        when(requestRegister.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse responseRegister = mock(HttpServletResponse.class);
        when(responseRegister.getWriter()).thenReturn(mock(PrintWriter.class));

        new UserServlet().doPost(requestRegister, responseRegister);

        verify(responseRegister).setStatus(HttpServletResponse.SC_OK);
        ArgumentCaptor<String> writeArgumentCaptor = ArgumentCaptor.forClass(String.class);
        verify(responseRegister.getWriter()).write(writeArgumentCaptor.capture());

        // Выходим
        HttpServletRequest requestLogout = mock(HttpServletRequest.class);
        when(requestLogout.getParameter("action")).thenReturn("logout");
        when(requestLogout.getParameter("token")).thenReturn(writeArgumentCaptor.getValue());

        HttpServletResponse responseLogout = mock(HttpServletResponse.class);

        new UserServlet().doPost(requestLogout, responseLogout);

        verify(responseLogout).setStatus(HttpServletResponse.SC_OK);
        assertTrue(Token.select(t -> !t.isDeleted()).isEmpty());
    }

    @Test
    @DisplayName("Удаление пользователя")
    void deleteUser() throws Exception {
        // Регистрируем
        HttpServletRequest requestRegister = mock(HttpServletRequest.class);
        when(requestRegister.getParameter("action")).thenReturn("register");
        when(requestRegister.getParameter("email")).thenReturn("test@test.com");
        when(requestRegister.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse responseRegister = mock(HttpServletResponse.class);
        when(responseRegister.getWriter()).thenReturn(mock(PrintWriter.class));

        new UserServlet().doPost(requestRegister, responseRegister);

        verify(responseRegister).setStatus(HttpServletResponse.SC_OK);
        ArgumentCaptor<String> writeArgumentCaptor = ArgumentCaptor.forClass(String.class);
        verify(responseRegister.getWriter()).write(writeArgumentCaptor.capture());

        // Удаляем
        HttpServletRequest requestDelete = mock(HttpServletRequest.class);
        when(requestDelete.getParameter("action")).thenReturn("delete");
        when(requestDelete.getParameter("token")).thenReturn(writeArgumentCaptor.getValue());

        HttpServletResponse responseDelete = mock(HttpServletResponse.class);

        new UserServlet().doPost(requestDelete, responseDelete);

        verify(responseDelete).setStatus(HttpServletResponse.SC_OK);
        assertNull(User.getByEmail("test@test.com"));

        // Вход больше не должен проходить
        HttpServletRequest requestLogin = mock(HttpServletRequest.class);
        when(requestLogin.getParameter("action")).thenReturn("login");
        when(requestLogin.getParameter("email")).thenReturn("test@test.com");
        when(requestLogin.getParameter("password")).thenReturn("1122334455ABCабв");

        HttpServletResponse responseLogin = mock(HttpServletResponse.class);

        new UserServlet().doPost(requestLogin, responseLogin);

        verify(responseLogin).sendError(HttpServletResponse.SC_UNAUTHORIZED, "Invalid user or password");
    }

    /**
     * Проверка корректности возвращаемого серветом токена
     */
    private static void checkToken(String email, HttpServletResponse response) throws Exception {
        // Token в base64 == 44 символа (т.к. исходно токен 32 байта или 11 8-битных троек, которые превратились в 11 6-битных четверок)
        verify(response.getWriter()).write(matches("^[0-9a-zA-Z+/=]{44}$"));

        User user = User.getByEmail(email);
        assertNotNull(user);

        List<Token> tokens = Token.select(t -> !t.isDeleted() && t.getUser().getId().equals(user.getId()));
        assertEquals(1, tokens.size());

        Token token = tokens.get(0);
        ArgumentCaptor<String> writeArgumentCaptor = ArgumentCaptor.forClass(String.class);
        verify(response.getWriter()).write(writeArgumentCaptor.capture());
        byte[] actualTokenData = Base64.getDecoder().decode(writeArgumentCaptor.getValue());
        assertArrayEquals(token.getTokenData(), actualTokenData);
    }
}