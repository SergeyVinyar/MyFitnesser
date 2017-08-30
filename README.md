**MyFitnesser** had to be a commercial close-source piece of software but later the project was cancelled
due to some non-technical reasons at an early stage of development. I got a permission to publish it for everyone's sake.
If you find anything useful for you, feel free to use it. If you have some issues with MIT license, do not hesitate 
to write me and I might change the license for you.

MyFitnesser intended to be an Android (and later iOs) application for fitness trainers to manage their clients 
with their workouts. A fitness trainer could make a list of clients and plan workouts for each of them. Additionally 
there were some plans of implementing smart algorithms of choosing appropriate exercises and comparing 
the results of training months by months. Nothing of these was implemented. Just my dreams :(

All mobile devices had to be synchronized. So I wrote some kind of backend for it.
Backend and mobile app are located accordingly in */BackEnd* and */MobileApp* directories.

Sorry for comments in code in russian.

**Backend**

Implemented in Java and based on embedded [Jetty](http://www.eclipse.org/jetty/). 
So this is a kind of a standalone http server with a few servlets binded to some urls. The server is intentionally stateless 
so one can create several servers to improve performance.

Data persistance (package *com.myfitnesser.backend.db.\**) is based on [Hibernate ORM](http://hibernate.org/orm/) and [PosgreSQL](https://www.postgresql.org/). 
For testing [H2 memory database](http://www.h2database.com/html/main.html) is used.

At the moment MyFitnesser security system (package *com.myfitnesser.backend.security.\**) supports registering new users, deleting them, logging in and logging out. 
After logging in the server returns a token that must be used for other requests. All the tokens have some expiration time.

Syncronization code (package *com.myfitnesser.backend.sync.\**) is also implemented. Mobile devices and backend send 
each other *com.myfitnesser.backend.sync.SyncPackage*. SyncPackages are serialized into json and contain only records 
that were changed since the previous synchronization.

Deployment is automated (have a look at */deploy* directory). Servers are deployed as docker containers 
using [Docker Machine](https://docs.docker.com/machine/). Docker Machine is a part of docker distribution package and is used
for managing remote Docker daemons. I intended to use Digital Ocean droplets that's why the deployment process includes 
Digital Ocean droplet creation stage.

The full process of deployment consists of:
* Defining environment variables: DO_API_KEY (Digital Ocean private key), DB_USER, DB_PASSWORD
* Running *createVPS.sh* (just once)
* Running *build_and_deploy.sh*

build_and_deploy.sh: 
* Builds a project. We get Server.jar as a result
* Builds a docker container with Server.jar based on [Alpine](https://docs.docker.com/samples/library/alpine/) image
* Removes existing docker containers on Digital Ocean droplet and runs a new one there
* Sets database credentials using environment variables
* Runs db docker container based on *postgres:9.6-alpine* image (database data is located in a directory on a host machine 
so recreating db docker container won't drop any data)

**Mobile app**

We wanted to share code between Android and iOs platforms so we used [Xamarin](https://www.xamarin.com/).
Xamarin is an implementation of [Mono platform](http://www.mono-project.com/) with some native libraries for mobile devices. 
You have to use C# with Xamarin.
Xamarin has a few option for making UI: XAML and native for each platform way. I decided to use native methods. So I had 
to write UI twice but all the core code could be shared.
Obviously */MyFitnesser.Core* directory contains core code and */MyFitnesser.Droid* directory contains Android code. 
And no iOs code :(
The application is based on MVVM ideology. I used [MvvmCross framework](https://www.mvvmcross.com/) so you might see 
lots of f*cking magic due to its beta status (it was long time ago, everything could have changed since that ancient time).
Generally I liked MvvmCross. It worth time to look about.

Unfortunatelly development had stopped too early so I assume that mobile app source code is useless.
