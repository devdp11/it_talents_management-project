FROM postgres:15.3

RUN apt update
RUN apt install wget -y
RUN apt install postgresql-15-postgis-3 -y
RUN apt install postgis -y

ENV POSTGRES_USER=es2
ENV POSTGRES_PASSWORD=es2
ENV POSTGRES_DB=es2

COPY migrations/* /docker-entrypoint-initdb.d/

EXPOSE 5432