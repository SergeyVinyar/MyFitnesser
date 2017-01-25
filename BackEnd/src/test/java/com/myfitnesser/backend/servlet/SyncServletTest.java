package com.myfitnesser.backend.servlet;

import com.google.gson.Gson;
import com.myfitnesser.backend.db.Client;
import com.myfitnesser.backend.db.DbServiceInMemory;
import com.myfitnesser.backend.db.Training;
import com.myfitnesser.backend.db.User;
import com.myfitnesser.backend.security.SecurityService;
import com.myfitnesser.backend.sync.SyncPackage;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentCaptor;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.PrintWriter;
import java.util.Arrays;
import java.util.Base64;
import java.util.Collections;
import java.util.UUID;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class SyncServletTest {

    @BeforeEach
    void setUp() {
        DbServiceInMemory.switchToMemDb();
    }

    @AfterEach
    void tearDown() {
        DbServiceInMemory.closeConnection();
    }

    @Test
    @DisplayName("Синхронизация клиентов")
    void syncClients() throws Exception {
        Context context = prepareContext();
        Gson gson = new Gson();

        // Готовим пакет синхронизации
        SyncPackage requestSyncPackage = new SyncPackage();
        requestSyncPackage.deviceId = UUID.randomUUID();

        SyncPackage.Client newClient = requestSyncPackage.new Client();
        newClient.id = UUID.randomUUID();
        newClient.name = "NewClient";

        SyncPackage.Client modifiedClient = requestSyncPackage.new Client();
        modifiedClient.id = context.u2c2.getId();
        modifiedClient.name = "UpdatedClient";

        requestSyncPackage.clientsUpdated = new SyncPackage.Client[] { newClient, modifiedClient };
        requestSyncPackage.clientsDeleted = new UUID[] { context.u2c1.getId() };

        // Поехали
        String tokenData = Base64.getEncoder().encodeToString(SecurityService.loginUser(context.u2.getEmail(), new char[0]));

        HttpServletRequest request = mock(HttpServletRequest.class);
        when(request.getParameter("token")).thenReturn(tokenData);
        when(request.getParameter("data")).thenReturn(gson.toJson(requestSyncPackage));

        HttpServletResponse response = mock(HttpServletResponse.class);
        when(response.getWriter()).thenReturn(mock(PrintWriter.class));

        new SyncServlet().doPost(request, response);

        verify(response).setStatus(HttpServletResponse.SC_OK);
        ArgumentCaptor<String> writeArgumentCaptor = ArgumentCaptor.forClass(String.class);
        verify(response.getWriter()).write(writeArgumentCaptor.capture());

        // Проверяем ответ
        SyncPackage responseSyncPackage = gson.fromJson(writeArgumentCaptor.getValue(), SyncPackage.class);
        assertNotNull(responseSyncPackage);

        // Были в базе u2c1, u2c2, u2c3. Одну запись вставили, u2c1 удалили, u2c2 изменили => возвращается u2c3
        assertTrue(responseSyncPackage.clientsUpdated.length == 1 && responseSyncPackage.clientsUpdated[0].id.equals(context.u2c3.getId()));

        // Готовим пакет от другого DeviceId
        requestSyncPackage = new SyncPackage();
        requestSyncPackage.deviceId = UUID.randomUUID();

        // Поехали
        request = mock(HttpServletRequest.class);
        when(request.getParameter("token")).thenReturn(tokenData);
        when(request.getParameter("data")).thenReturn(gson.toJson(requestSyncPackage));

        response = mock(HttpServletResponse.class);
        when(response.getWriter()).thenReturn(mock(PrintWriter.class));

        new SyncServlet().doPost(request, response);

        verify(response).setStatus(HttpServletResponse.SC_OK);
        writeArgumentCaptor = ArgumentCaptor.forClass(String.class);
        verify(response.getWriter()).write(writeArgumentCaptor.capture());

        // Проверяем ответ (мы должны получить полный набор данных с учетом предыдущей синхронизации)
        responseSyncPackage = gson.fromJson(writeArgumentCaptor.getValue(), SyncPackage.class);
        assertNotNull(responseSyncPackage);

        // u2c1 должна быть удаленная
        assertTrue(responseSyncPackage.clientsDeleted.length == 1 && responseSyncPackage.clientsDeleted[0].equals(context.u2c1.getId()));

        // должны быть записи: u2c2 (name == "UpdatedClient"), u2c3 и newClient
        assertEquals(3, responseSyncPackage.clientsUpdated.length);

        long correctClientsCount = Arrays.stream(responseSyncPackage.clientsUpdated).filter(c -> {
            return
                       (c.id.equals(context.u2c2.getId()) && c.name.equals("UpdatedClient"))
                    || (c.id.equals(context.u2c3.getId()))
                    || (c.id.equals(newClient.id));
        }).count();
        assertEquals(3, correctClientsCount);
    }

    @Test
    @DisplayName("Синхронизация тренирововок")
    void syncTrainings() throws Exception {
        Context context = prepareContext();
        Gson gson = new Gson();

        // Готовим пакет синхронизации
        SyncPackage requestSyncPackage = new SyncPackage();
        requestSyncPackage.deviceId = UUID.randomUUID();

        SyncPackage.Training newTraining = requestSyncPackage.new Training();
        newTraining.id = UUID.randomUUID();
        newTraining.clientId = context.u2c3.getId();
        newTraining.notes = "NewTraining";

        SyncPackage.Training modifiedTraining = requestSyncPackage.new Training();
        modifiedTraining.id = context.u2c2t2.getId();
        modifiedTraining.clientId = context.u2c2.getId();
        modifiedTraining.notes = "UpdatedTraining";

        requestSyncPackage.trainingsUpdated = new SyncPackage.Training[] { newTraining, modifiedTraining };
        requestSyncPackage.trainingsDeleted = new UUID[] { context.u2c1t1.getId() };

        // Поехали
        String tokenData = Base64.getEncoder().encodeToString(SecurityService.loginUser(context.u2.getEmail(), new char[0]));

        HttpServletRequest request = mock(HttpServletRequest.class);
        when(request.getParameter("token")).thenReturn(tokenData);
        when(request.getParameter("data")).thenReturn(gson.toJson(requestSyncPackage));

        HttpServletResponse response = mock(HttpServletResponse.class);
        when(response.getWriter()).thenReturn(mock(PrintWriter.class));

        new SyncServlet().doPost(request, response);

        verify(response).setStatus(HttpServletResponse.SC_OK);
        ArgumentCaptor<String> writeArgumentCaptor = ArgumentCaptor.forClass(String.class);
        verify(response.getWriter()).write(writeArgumentCaptor.capture());

        // Проверяем ответ
        SyncPackage responseSyncPackage = gson.fromJson(writeArgumentCaptor.getValue(), SyncPackage.class);
        assertNotNull(responseSyncPackage);

        // Были в базе:
        // u2c1t1, u2c1t2
        // u2c2t1, u2c2t2
        // Одну запись вставили, u2c1t1 удалили, u2c2t2 изменили => возвращается 2 записи (еще 2 есть под u1, но мы здесь работает под u2)
        assertTrue(responseSyncPackage.trainingsUpdated.length == 2);

        long correctTrainingsCount = Arrays.stream(responseSyncPackage.trainingsUpdated).filter(t -> {
            return
                   (t.id.equals(context.u2c1t2.getId()))
                || (t.id.equals(context.u2c2t1.getId()));
        }).count();
        assertEquals(2, correctTrainingsCount);

        // Готовим пакет от другого DeviceId
        requestSyncPackage = new SyncPackage();
        requestSyncPackage.deviceId = UUID.randomUUID();

        // Поехали
        request = mock(HttpServletRequest.class);
        when(request.getParameter("token")).thenReturn(tokenData);
        when(request.getParameter("data")).thenReturn(gson.toJson(requestSyncPackage));

        response = mock(HttpServletResponse.class);
        when(response.getWriter()).thenReturn(mock(PrintWriter.class));

        new SyncServlet().doPost(request, response);

        verify(response).setStatus(HttpServletResponse.SC_OK);
        writeArgumentCaptor = ArgumentCaptor.forClass(String.class);
        verify(response.getWriter()).write(writeArgumentCaptor.capture());

        // Проверяем ответ (мы должны получить полный набор данных с учетом предыдущей синхронизации)
        responseSyncPackage = gson.fromJson(writeArgumentCaptor.getValue(), SyncPackage.class);
        assertNotNull(responseSyncPackage);

        // u2c1t1 должна быть удаленная
        assertTrue(responseSyncPackage.trainingsDeleted.length == 1 && responseSyncPackage.trainingsDeleted[0].equals(context.u2c1t1.getId()));

        // должны быть 4 исходных записей - удаленная + новая
        assertEquals(4, responseSyncPackage.trainingsUpdated.length);

        correctTrainingsCount = Arrays.stream(responseSyncPackage.trainingsUpdated).filter(t -> {
            return
                   (t.id.equals(context.u2c1t2.getId()))
                || (t.id.equals(context.u2c2t1.getId()))
                || (t.id.equals(context.u2c2t2.getId()) && t.notes.equals("UpdatedTraining"))
                || (t.id.equals(newTraining.id));
        }).count();
        assertEquals(4, correctTrainingsCount);
    }

    private Context prepareContext() throws Exception {
        Context context = new Context();

        // Формирование данных первого пользователя
        SecurityService.registerNewUser("user1@test.com", new char[0]);
        context.u1 = User.getByEmail("user1@test.com");
        context.u1c1.setUser(context.u1).save();
        context.u1c1t1.setUser(context.u1).setClient(context.u1c1).save();
        context.u1c1t2.setUser(context.u1).setClient(context.u1c1).save();

        // Формирование данных второго пользователя
        SecurityService.registerNewUser("user2@test.com", new char[0]);
        context.u2 = User.getByEmail("user2@test.com");
        context.u2c1.setUser(context.u2).save();
        context.u2c1t1.setUser(context.u2).setClient(context.u2c1).save();
        context.u2c1t2.setUser(context.u2).setClient(context.u2c1).save();
        context.u2c2.setUser(context.u2).save();
        context.u2c2t1.setUser(context.u2).setClient(context.u2c2).save();
        context.u2c2t2.setUser(context.u2).setClient(context.u2c2).save();
        context.u2c3.setUser(context.u2).save();

        return context;
    }

    /**
     * Данные предварительно сформированных в базе сущностей
     */
    private static class Context {
        User u1;

        Client u1c1 = new Client();

        Training u1c1t1 = new Training();
        Training u1c1t2 = new Training();

        User u2;

        Client u2c1 = new Client();

        Training u2c1t1 = new Training();
        Training u2c1t2 = new Training();

        Client u2c2 = new Client();

        Training u2c2t1 = new Training();
        Training u2c2t2 = new Training();

        Client u2c3 = new Client();
    }
}