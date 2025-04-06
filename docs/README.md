# How to setup your machine

## Docker

you can install docker with the following command 
```winget install -e --id Docker.DockerDesktop```

### Postgresql

You can run postgresql in a docker container with the following command
Make sure to not set your secretpassword as the one in the command line, use a unique one
```docker run restart -p 5432:5432 --name discordclone-postgress -e POSTGRES_USER=postgres -e POSTGRES_DB=discordclone -e POSTGRES_PASSWORD=mysecretpassword -d postgres```

### Seq

Seq is the interface for reading logs
```docker run -d -p 80:80 -p 5341:5341 -e ACCEPT_EULA=Y --restart unless-stopped --name seq datalust/seq```