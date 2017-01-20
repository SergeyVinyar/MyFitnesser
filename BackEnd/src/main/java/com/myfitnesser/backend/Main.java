package com.myfitnesser.backend;

import com.myfitnesser.backend.servlet.UserServlet;
import com.myfitnesser.backend.servlet.SyncServlet;
import org.eclipse.jetty.server.Handler;
import org.eclipse.jetty.server.Server;
import org.eclipse.jetty.server.handler.HandlerList;
import org.eclipse.jetty.server.handler.ResourceHandler;
import org.eclipse.jetty.servlet.ServletHandler;

public class Main {

    public static void main(String[] args) throws Exception {
        Server server = new Server(8080);

        ServletHandler servletHandler = new ServletHandler();
        servletHandler.addServletWithMapping(UserServlet.class, "/user");
        servletHandler.addServletWithMapping(SyncServlet.class, "/sync");

        ResourceHandler resourceHandler = new ResourceHandler();
        resourceHandler.setDirectoriesListed(true);
        resourceHandler.setResourceBase("public_html");
        resourceHandler.setWelcomeFiles(new String[] { "index.html" });
        resourceHandler.setRedirectWelcome(true);

        HandlerList handlerList = new HandlerList();
        handlerList.setHandlers(new Handler[] { resourceHandler, servletHandler });
        server.setHandler(handlerList);

        server.start();
        server.join();
    }
}
