package com.myfitnesser.backend.db;

import javax.persistence.*;
import java.time.OffsetDateTime;
import java.time.ZoneOffset;
import java.util.Optional;
import java.util.UUID;
import java.util.stream.*;

/**
 * Дата/время последней синхронизации с клиентскими устройствами
 */
@Entity
@Table(name = "latest_sync")
public final class LatestSync extends BaseEntity {

    private LatestSync() {
    }

    @Column(nullable = false)
    private UUID deviceId;

    @Column(nullable = false)
    private OffsetDateTime latestDateTime;

    /**
     * Возвращает дату/время последней синхронизации устройства deviceId
     */
    public static OffsetDateTime get(UUID deviceId) throws DbException {
        Optional<LatestSync> optLatestSync = getByDeviceId(deviceId);
        LatestSync latestSync;
        if(optLatestSync.isPresent()) {
            latestSync = optLatestSync.get();
        } else {
            latestSync = new LatestSync();
            latestSync.setDeviceId(deviceId);
            latestSync.setLatestDateTime(OffsetDateTime.of(2017, 1, 1, 0, 0, 0, 0, ZoneOffset.UTC));
            latestSync.save();
        }
        return latestSync.getLatestDateTime();
    }

    /**
     * Устанавливает дату/время последней синхронизации устройства deviceId
     */
    public static void set(UUID deviceId, OffsetDateTime latestDateTime) throws DbException {
        Optional<LatestSync> optLatestSync = getByDeviceId(deviceId);
        LatestSync latestSync;
        if(optLatestSync.isPresent()) {
            latestSync = optLatestSync.get();
        } else {
            latestSync = new LatestSync();
            latestSync.setDeviceId(deviceId);
        }
        latestSync.setLatestDateTime(latestDateTime);
        latestSync.save();
    }

    private static Optional<LatestSync> getByDeviceId(UUID deviceId) throws DbException {
        return DbService.getInstance().executeWithStream(LatestSync.class, s -> s.filter(v -> v.getDeviceId().equals(deviceId)).findFirst());
    }

    private UUID getDeviceId() {
        return deviceId;
    }

    private void setDeviceId(UUID deviceId) {
        this.deviceId = deviceId;
    }

    private OffsetDateTime getLatestDateTime() {
        return latestDateTime;
    }

    private void setLatestDateTime(OffsetDateTime latestDateTime) {
        this.latestDateTime = latestDateTime;
    }
}
