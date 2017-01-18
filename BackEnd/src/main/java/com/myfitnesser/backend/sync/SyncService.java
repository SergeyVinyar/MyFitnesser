package com.myfitnesser.backend.sync;

import com.google.gson.Gson;
import com.myfitnesser.backend.db.Client;
import com.myfitnesser.backend.db.LatestSync;
import com.myfitnesser.backend.db.Training;

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
     * @param jsonSyncPackage
     * Сериализованный SyncPackage от пользовательского устройства с данными, измененными на устройстве с предыдущей синхронизации
     * @return
     * Сериализованный SyncPackage с данными, измененными на сервере с предыдущей синхронизации
     */
    public static String doSync(String jsonSyncPackage) throws SyncException {
        try {
            SyncPackage syncPackage = gson.fromJson(jsonSyncPackage, SyncPackage.class);
            if(syncPackage.cDeleted == null)
                syncPackage.cDeleted = new UUID[0];
            if(syncPackage.cUpdated == null)
                syncPackage.cUpdated = new SyncPackage.Client[0];
            if(syncPackage.tDeleted == null)
                syncPackage.tDeleted = new UUID[0];
            if(syncPackage.tUpdated == null)
                syncPackage.tUpdated = new SyncPackage.Training[0];

            syncPackageToDb(syncPackage);

            OffsetDateTime newLatestSync = OffsetDateTime.now();
            String result = gson.toJson(dbToSyncPackage(syncPackage.deviceId, newLatestSync, syncPackage));

            LatestSync.set(syncPackage.deviceId, newLatestSync);
            return result;
        } catch (Exception e) {
            throw new SyncException(e);
        }
    }

    /**
     * Отражает данные пакета синхронизации на базу данных
     */
    private static void syncPackageToDb(SyncPackage syncPackage) throws SyncException {
        try {
            // Клиента
            for (SyncPackage.Client syncClient : syncPackage.cUpdated) {
                try {
                    Client dbClient = Client.get(syncClient.id, () -> new Client(syncClient.id));
                    dbClient
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
            Client.deleteAll(Arrays.asList(syncPackage.cDeleted));

            // Тренировки
            for (SyncPackage.Training syncTraining : syncPackage.tUpdated) {
                try {
                    Training dbTraining = Training.get(syncTraining.id, () -> new Training(syncTraining.id));
                    dbTraining
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
            Training.deleteAll(Arrays.asList(syncPackage.tDeleted));
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
    private static SyncPackage dbToSyncPackage(UUID deviceId, OffsetDateTime newLatestSync, SyncPackage syncPackageFromDevice) throws SyncException {
        try {
            SyncPackage syncPackage = new SyncPackage();
            syncPackage.deviceId = deviceId;

            OffsetDateTime oldLatestSync = LatestSync.get(deviceId);

            HashSet<UUID> oldcDeleted = new HashSet<>(Arrays.asList(syncPackageFromDevice.cDeleted));
            HashSet<UUID> oldcUpdated = Arrays.stream(syncPackageFromDevice.cUpdated).map(v -> v.id).collect(Collectors.toCollection(HashSet::new));

            List<UUID> cDeleted = new LinkedList<>();
            List<SyncPackage.Client> cUpdated = new LinkedList<>();
            List<Client> dbClients = Client.select(c -> c.getLastModified().isAfter(oldLatestSync) && c.getLastModified().isBefore(newLatestSync));
            for (Client dbClient : dbClients) {
                if (dbClient.isDeleted()) {
                    if(!oldcDeleted.contains(dbClient.getId()))
                        cDeleted.add(dbClient.getId());
                } else {
                    if(!oldcUpdated.contains(dbClient.getId())) {
                        SyncPackage.Client syncClient = syncPackage.new Client();
                        syncClient.id = dbClient.getId();
                        syncClient.name = dbClient.getName();
                        syncClient.phone = dbClient.getPhone();
                        syncClient.email = dbClient.getEmail();
                        syncClient.birthDate = dbClient.getBirthDate();
                        syncClient.notes = dbClient.getNotes();
                        cUpdated.add(syncClient);
                    }
                }
            }
            syncPackage.cDeleted = cDeleted.toArray(new UUID[0]);
            syncPackage.cUpdated = cUpdated.toArray(new SyncPackage.Client[0]);

            HashSet<UUID> oldtDeleted = new HashSet<>(Arrays.asList(syncPackageFromDevice.tDeleted));
            HashSet<UUID> oldtUpdated = Arrays.stream(syncPackageFromDevice.tUpdated).map(v -> v.id).collect(Collectors.toCollection(HashSet::new));

            List<UUID> tDeleted = new LinkedList<>();
            List<SyncPackage.Training> tUpdated = new LinkedList<>();
            List<Training> dbTrainings = Training.select(t -> t.getLastModified().isAfter(oldLatestSync) && t.getLastModified().isBefore(newLatestSync));
            for (Training dbTraining : dbTrainings) {
                if (dbTraining.isDeleted()) {
                    if(!oldtDeleted.contains(dbTraining.getId()))
                        tDeleted.add(dbTraining.getId());
                } else {
                    if(!oldtUpdated.contains(dbTraining.getId())) {
                        SyncPackage.Training syncTraining = syncPackage.new Training();
                        syncTraining.id = dbTraining.getId();
                        syncTraining.clientId = dbTraining.getClient().getId();
                        syncTraining.start = dbTraining.getStartDateTime();
                        syncTraining.end = dbTraining.getEndDateTime();
                        syncTraining.state = dbTraining.getState();
                        syncTraining.notes = dbTraining.getNotes();
                        tUpdated.add(syncTraining);
                    }
                }
            }
            syncPackage.tDeleted = tDeleted.toArray(new UUID[0]);
            syncPackage.tUpdated = tUpdated.toArray(new SyncPackage.Training[0]);

            return syncPackage;
        } catch (Exception e) {
            throw new SyncException(e);
        }
    }
}
