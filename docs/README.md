# How to setup your machine

## Docker

you can install docker with the following command 
```winget install -e --id Docker.DockerDesktop```

### Docker compose

You can run docker compose if you open terminal and go to the folder and use following command
```docker-compose up -d```

### Manual

#### Postgresql

You can run postgresql in a docker container with the following command
Make sure to not set your secretpassword as the one in the command line, use a unique one
```docker run restart -p 5432:5432 --name discordclone-postgress -e POSTGRES_USER=postgres -e POSTGRES_DB=discordclone -e POSTGRES_PASSWORD=mysecretpassword -d postgres```

#### Seq

Seq is the interface for reading logs
```docker run -d -p 80:80 -p 5341:5341 -e ACCEPT_EULA=Y --restart unless-stopped --name seq datalust/seq```

#### RabbitMq

```docker run -d -p 15672:15672 -p 5672:5672 --restart unless-stopped --name vdbconnect_rmq masstransit/rabbitmq```

#### Solr
```docker run -d -p 8983:8983 --restart unless-stopped --name vdbconnect_solr solr:9```