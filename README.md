# Software Engineering II - Dev Kit #

### Introduction ###

This solution allows you to easily install the development environment and its dependencies.
This structure can be used for the project assignment in Software Engineering II course from Informatics Engineering at IPVC/ESTG.

### How to I setup my development environment? ###

* Install [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* There are 2 alternatives:
  * Compile and run the Infrastructure project 
```
cd Infrastructure
dotnet build
dotnet run
```
  * Run docker compose commands manually using the following set of commands in the root folder of the infrastructure project:
```
docker-compose build
docker-compose up
```
  * ***Note:*** you can use the **-d** flag to run the docker containers in background
```
docker-compose up -d
```

### Docker Images ###

#### PostgreSQL Database ####

* Available at localhost:PORT_NUMBER or db:5432 within docker virtual network. The port is defined by the variable EXP_PORT_PG in the .env file.
* This database also installs PostGIS to allow for dealing with the geographical data, if needed
* The credentials for the database are:
    * **username**: es2
    * **password**: es2
    * **database**: es2
* Please note that this image will recreate the database when it runs, if no previous volume is available. The database will be created based on the scripts under the migrations folder - feel free to modify those and adapt to your specific case.

#### Volumes ####

##### *pg_data* #####
This is a volume to store and keep the database data intact, even if you stop the containers.

___
#### _Informatics Engineering @ipvc/estg, 2022-2023_ ####
###### _Professors: Luís Teófilo_ ######