# How to setup your machine

## Docker

you can install docker with the following command 
```winget install -e --id Docker.DockerDesktop```

### Postgresql

You can run postgresql in a docker container with the following command
Make sure to not set your secretpassword as the one in the command line, use a unique one
```docker run -p 5432:5432 --name discordclone-postgress -e POSTGRES_USER=postgres -e POSTGRES_DB=discordclone -e POSTGRES_PASSWORD=mysecretpassword -d postgres```