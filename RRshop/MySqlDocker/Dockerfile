FROM mysql:8.0.32
MAINTAINER razil_khayka@mail.com
ENV MYSQL_ROOT_PASSWORD dbuserpassword
ENV DBHOST=db
ADD SqlDump/setup.sql /docker-entrypoint-initdb.d
EXPOSE 3306