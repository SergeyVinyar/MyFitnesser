package com.myfitnesser.backend;

import com.myfitnesser.backend.db.Client;
import com.myfitnesser.backend.db.DbException;
import com.myfitnesser.backend.db.Training;
import com.myfitnesser.backend.sync.SyncException;
import com.myfitnesser.backend.sync.SyncService;

import java.time.LocalDate;
import java.time.OffsetDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class Main {

    public static void main(String[] args) throws DbException, SyncException {
        String result = SyncService.doSync("{ \"deviceId\": \"5c244197-d776-4afd-8b7d-99d76ea7aeb5\", \"ver\": \"1\", \"cDeleted\":[\"53e2b332-b3bf-4ba4-96e1-c780ea4b6e8f\"] }");
        System.out.println(result);

//        {"deviceId":"5c244197-d776-4afd-8b7d-99d76ea7aeb5","ver":1,"cDeleted":["b868f79a-e51d-4b07-9723-d345a4cfe238"],"cUpdated":[{"id":"a087da00-feea-4521-94ca-7513a78e621f","name":"Иванов","birthDate":{"year":2017,"month":1,"day":16},"notes":"Это такой длинный комментарий, что и конца оного не видно"},{"id":"53e2b332-b3bf-4ba4-96e1-c780ea4b6e8f","name":"Петров","birthDate":{"year":2017,"month":1,"day":16},"notes":"Это такой длинный комментарий, что и конца оного не видно"}],"tDeleted":[],"tUpdated":[]}


//        Client c = Client.load(UUID.fromString("53e2b332-b3bf-4ba4-96e1-c780ea4b6e8f"));
//        System.out.println(c.getName());

        //List<Client> cc = Client.select(c -> !c.getName().equals("Петров") && !c.getName().equals("Иванов"));
        //cc.get(0).delete();



//        Client c = new Client();
//        c.setName("Залипукин");
//        c.setNotes("Это такой длинный комментарий, что и конца оного не видно");
//        c.setBirthDate(LocalDate.now());
//
//        List<Training> tt = new ArrayList<Training>(2);
//        Training t = new Training();
//        t.setStartDateTime(OffsetDateTime.now());
//        t.setEndDateTime(OffsetDateTime.now().plusHours(2));
//        t.setNotes("Первая тренировка");
//        t.setClient(c);
//        tt.add(t);
//
//        t = new Training();
//        t.setStartDateTime(OffsetDateTime.now());
//        t.setEndDateTime(OffsetDateTime.now().plusHours(2));
//        t.setNotes("Вторая тренировка");
//        t.setClient(c);
//        tt.add(t);

//        c.setTrainings(tt);

//        c.save();
    }

}
