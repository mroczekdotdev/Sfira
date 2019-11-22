![Sfira-logo](.readme/sfira-logo-medium.png?raw=true "logo")

## Description

**Sfira** is a social media web application created using ASP.NET Core MVC.

## Technologies and dependencies

- **C#** is an elegant and type-safe object-oriented language that enables developers to build a variety of secure and robust applications that run on the .NET Framework.

- **.NET Core** is an open-source, general-purpose development platform. It's cross-platform (supporting Windows, macOS, and Linux) and can be used to build device, cloud, and IoT applications.

- **ASP.NET Core** is an open-source and cross-platform framework for building modern cloud based internet connected applications, such as web apps, IoT apps and mobile backends.

- **Entity Framework Core** is a lightweight, extensible, open-source and cross-platform data access technology. It can serve as an object-relational mapper, enabling developers to work with a database using .NET objects.

- **Npgsql** is an open-source ADO.NET Data Provider for PostgreSQL.

- **Sass** is an extension of CSS, adding nested rules, variables, mixins, selector inheritance, and more. It's translated to well-formatted, standard CSS.

- **JavaScript** is a prototype-based, multi-paradigm scripting language that is dynamic, and supports object-oriented, imperative, and functional programming styles.

- **jQuery** is a fast, small, and feature-rich JavaScript library.

- **Font Awesome** is the iconic SVG, font, and CSS toolkit.

- **npm** is a package manager for JavaScript.

- **webpack** is a bundler for modules. It is also capable of transforming, bundling, or packaging just about any resource or asset.

- **NGINX Open Source** is an HTTP and reverse proxy server, a mail proxy server, and a generic TCP/UDP proxy server.

- **Docker** is a platform for developers and sysadmins to develop, deploy, and run applications with containers.

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
