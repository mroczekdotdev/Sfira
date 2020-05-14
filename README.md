![Sfira-logo](.readme/sfira-logo-medium.png?raw=true "logo")

## Description

**Sfira** is a social media web application created using ASP.NET Core MVC.

## Technologies and dependencies

**C#, .NET Core, ASP.NET Core, Entity Framework Core, xUnit.net, Moq, PostgreSQL, Sass (SCSS), jQuery, Font Awesome, webpack, NGINX Open Source, Docker.**

## Showcase

### Demonstration

Go to Sfira online demo:

| https://mroczek.dev/sfira/explore |
| --------------------------------- |


Register as a new user or log in with these credentials:

`Username: user1`\
`Password: user1`

### Screenshots

![Sfira-screenshot_01](.readme/sfira-screenshot-01.jpg?raw=true "screenshot")
![Sfira-screenshot_02](.readme/sfira-screenshot-02.jpg?raw=true "screenshot")
![Sfira-screenshot_03](.readme/sfira-screenshot-03.jpg?raw=true "screenshot")
![Sfira-screenshot_04](.readme/sfira-screenshot-04.jpg?raw=true "screenshot")

## Installation

### Setup

Make sure that **Docker** and **Docker Compose** are installed:

```console
docker --version
docker-compose --version

```

Clone the repository into a new directory:

```console
git clone https://github.com/mroczekdotdev/Sfira/
```

or download the repository, then extract the files:

| https://github.com/mroczekdotdev/Sfira/archive/master.zip |
| --------------------------------------------------------- |


### Configure

From the root of the repository open `.env` file in a text editor.

Edit values after equals signs.

Save the file.

⚠ **IMPORTANT NOTICE!** Database seeding with dummy data is enabled by default.

### Build and run

From the root of the repository open `run.sh` or execute following command:

```console
docker-compose up --detach
```

The application should be reachable at:

| http://localhost:80/ |
| -------------------- |


⚠ **IMPORTANT NOTICE!** The port will differ if `Application_Port` was changed in the [Configuration](#configuration) step.
