package com.myfitnesser.backend.servlet;

import com.myfitnesser.backend.db.DbException;
import com.myfitnesser.backend.db.User;
import com.myfitnesser.backend.security.SecurityService;
import com.myfitnesser.backend.sync.SyncException;
import com.myfitnesser.backend.sync.SyncService;

import java.util.Base64;
import java.util.Optional;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

/**
 * Синхронизация баз данных с клиентскими устройствами
 */
final public class SyncServlet extends HttpServlet {

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        try {
            String tokenData = req.getParameter("token");
            if(tokenData == null) {
                resp.sendError(HttpServletResponse.SC_UNAUTHORIZED);
                return;
            }
            User user = SecurityService.getUserFromTokenData(Base64.getDecoder().decode(tokenData));
            if(user == null) {
                resp.sendError(HttpServletResponse.SC_UNAUTHORIZED);
                return;
            }
            String syncData = req.getParameter("data");
            if (syncData != null) {
                String responseSyncData = SyncService.doSync(user, syncData);
                resp.getWriter().write(responseSyncData);
            }
            resp.setStatus(HttpServletResponse.SC_OK);
        }
        catch(DbException e) {
            throw new IOException(e);
        }
        catch (SyncException e) {
            throw new ServletException(e);
        }
    }
}
