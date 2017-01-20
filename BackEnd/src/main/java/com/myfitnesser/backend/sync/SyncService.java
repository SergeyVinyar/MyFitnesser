package com.myfitnesser.backend.sync;

import com.google.gson.Gson;
import com.myfitnesser.backend.db.Client;
import com.myfitnesser.backend.db.LatestSync;
import com.myfitnesser.backend.db.Training;
import com.myfitnesser.backend.db.User;

import java.time.OffsetDateTime;
import java.util.*;
import java.util.stream.Collectors;

/**
 * Сервис (helper) синхронизации с пользовательскими устройствами
 */
public final class SyncService {

    private static Gson gson = new Gson();

    private SyncService() {
    }

    /**
     * Выполнить синхронизацию
     * @param user
     * Пользователь
     * @param jsonSyncPackage
     * Сериализованный SyncPackage от пользовательского устройства с данными, измененными на устройстве с предыдущей синхронизации
     * @return
     * Сериализованный SyncPackage с данными, измененными на сервере с предыдущей синхронизации
     */
    public static String doSync(User user, String jsonSyncPackage) throws SyncException {
        try {
            SyncPackage syncPackage = gson.fromJson(jsonSyncPackage, SyncPackage.class);
            if(syncPackage.clientsDeleted == null)
                syncPackage.clientsDeleted = new UUID[0];
            if(syncPackage.clientsUpdated == null)
                syncPackage.clientsUpdated = new SyncPackage.Client[0];
            if(syncPackage.trainingsDeleted == null)
                syncPackage.trainingsDeleted = new UUID[0];
            if(syncPackage.trainingsUpdated == null)
                syncPackage.trainingsUpdated = new SyncPackage.Training[0];

            syncPackageToDb(user, syncPackage);

            OffsetDateTime newLatestSync = OffsetDateTime.now();
            String result = gson.toJson(dbToSyncPackage(user, syncPackage.deviceId, newLatestSync, syncPackage));

            LatestSync.set(syncPackage.deviceId, newLatestSync);
            return result;
        } catch (Exception e) {
            throw new SyncException(e);
        }
    }

    /**
     * Отражает данные пакета синхронизации на базу данных
     */
    private static void syncPackageToDb(User user, SyncPackage syncPackage) throws SyncException {
        try {
            // Клиента
            for (SyncPackage.Client syncClient : syncPackage.clientsUpdated) {
                try {
                    Client dbClient = Client.get(syncClient.id, () -> new Client(syncClient.id));
                    dbClient
                            .setUser(user)
                            .setName(syncClient.name)
                            .setPhone(syncClient.phone)
                            .setEmail(syncClient.email)
                            .setBirthDate(syncClient.birthDate)
                            .setNotes(syncClient.notes)
                            .save();
                } catch (Exception e) {
                    throw new RuntimeException(e);
                }
            }
            Client.deleteAll(Arrays.asList(syncPackage.clientsDeleted));

            // Тренировки
            for (SyncPackage.Training syncTraining : syncPackage.trainingsUpdated) {
                try {
                    Training dbTraining = Training.get(syncTraining.id, () -> new Training(syncTraining.id));
                    dbTraining
                            .setUser(user)
                            .setClient(Client.get(syncTraining.clientId, null))
                            .setStartDateTime(syncTraining.start)
                            .setEndDateTime(syncTraining.end)
                            .setState(syncTraining.state)
                            .setNotes(syncTraining.notes)
                            .save();
                } catch (Exception e) {
                    throw new RuntimeException(e);
                }
            }
            Training.deleteAll(Arrays.asList(syncPackage.trainingsDeleted));
        } catch (Exception e) {
            throw new SyncException(e);
        }
    }

    /**
     * Заносит в пакет синхронизации изменения в базе данных с момента последней синхронизации
     * @param syncPackageFromDevice
     * Пакет синхронизации, полученный только что с клиентского устройства. Необходим для того, чтобы отсечь из пакета
     * те изменения, которые только что приехали с устройства. Просто для уменьшения трафика.
     */
    private static SyncPackage dbToSyncPackage(User user, UUID deviceId, OffsetDateTime newLatestSync, SyncPackage syncPackageFromDevice) throws SyncException {
        try {
            SyncPackage syncPackage = new SyncPackage();
            syncPackage.deviceId = deviceId;

            OffsetDateTime oldLatestSync = LatestSync.get(deviceId);

            HashSet<UUID> oldClientsDeleted = new HashSet<>(Arrays.asList(syncPackageFromDevice.clientsDeleted));
            HashSet<UUID> oldClientsUpdated = Arrays.stream(syncPackageFromDevice.clientsUpdated).map(v -> v.id).collect(Collectors.toCollection(HashSet::new));

            List<UUID> clientsDeleted = new LinkedList<>();
            List<SyncPackage.Client> clientsUpdated = new LinkedList<>();
            List<Client> dbClients = Client.select(c -> c.getUser().getId().equals(user.getId()) && c.getLastModified().isAfter(oldLatestSync) && c.getLastModified().isBefore(newLatestSync));
            for (Client dbClient : dbClients) {
                if (dbClient.isDeleted()) {
                    if(!oldClientsDeleted.contains(dbClient.getId()))
                        clientsDeleted.add(dbClient.getId());
                } else {
                    if(!oldClientsUpdated.contains(dbClient.getId())) {
                        SyncPackage.Client syncClient = syncPackage.new Client();
                        syncClient.id = dbClient.getId();
                        syncClient.name = dbClient.getName();
                        syncClient.phone = dbClient.getPhone();
                        syncClient.email = dbClient.getEmail();
                        syncClient.birthDate = dbClient.getBirthDate();
                        syncClient.notes = dbClient.getNotes();
                        clientsUpdated.add(syncClient);
                    }
                }
            }
            syncPackage.clientsDeleted = clientsDeleted.toArray(new UUID[0]);
            syncPackage.clientsUpdated = clientsUpdated.toArray(new SyncPackage.Client[0]);

            HashSet<UUID> oldTrainingsDeleted = new HashSet<>(Arrays.asList(syncPackageFromDevice.trainingsDeleted));
            HashSet<UUID> oldTrainingsUpdated = Arrays.stream(syncPackageFromDevice.trainingsUpdated).map(v -> v.id).collect(Collectors.toCollection(HashSet::new));

            List<UUID> trainingsDeleted = new LinkedList<>();
            List<SyncPackage.Training> trainingsUpdated = new LinkedList<>();
            List<Training> dbTrainings = Training.select(t -> t.getUser().getId().equals(user.getId()) && t.getLastModified().isAfter(oldLatestSync) && t.getLastModified().isBefore(newLatestSync));
            for (Training dbTraining : dbTrainings) {
                if (dbTraining.isDeleted()) {
                    if(!oldTrainingsDeleted.contains(dbTraining.getId()))
                        trainingsDeleted.add(dbTraining.getId());
                } else {
                    if(!oldTrainingsUpdated.contains(dbTraining.getId())) {
                        SyncPackage.Training syncTraining = syncPackage.new Training();
                        syncTraining.id = dbTraining.getId();
                        syncTraining.clientId = dbTraining.getClient().getId();
                        syncTraining.start = dbTraining.getStartDateTime();
                        syncTraining.end = dbTraining.getEndDateTime();
                        syncTraining.state = dbTraining.getState();
                        syncTraining.notes = dbTraining.getNotes();
                        trainingsUpdated.add(syncTraining);
                    }
                }
            }
            syncPackage.trainingsDeleted = trainingsDeleted.toArray(new UUID[0]);
            syncPackage.trainingsUpdated = trainingsUpdated.toArray(new SyncPackage.Training[0]);

            return syncPackage;
        } catch (Exception e) {
            throw new SyncException(e);
        }
    }
}
