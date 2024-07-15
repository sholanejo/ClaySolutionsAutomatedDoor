# Automated Door API - A system which allows users to open doors and show historical events to extend our user experience beyond classical tags

An ASP.NET Web API which allows users to open doors and show historical events to extend users experience beyond classical tags
This repository contains the backend API implementation for the Automated Doors API.

## Table of Contents

  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Features](#features)
  - [Technology Stack](#technology-stack)
  - [Run The App](#run-the-app)
  - [Docs](#docs)
  - [License](#license)

## Overview

Automated Door API provides a comprehensive backend API for managing user accounts, authentication, door creation, door assignment to access control group, door access control group creation etc.

There are 5 core entities for this project
* AuditTrail
* Door
* DoorAccessControlGroup
* DoorPermission
* AuditTrail

## Features

- User Creation
- User Login
- User Activation and Deactivation
- Add and Remove Door from access control group
- Remove door from access control group
- Update User Access Control Group
- Get Audit trail
- Add new door
- Open and close door
- Get Doors

## Technology Stack

The technology stack used in this project is as follows:

- MEDIATR <https://github.com/jbogard/MediatR>

- EF Core <https://learn.microsoft.com/en-us/ef/core/>

- Fluent Validation <https://docs.fluentvalidation.net/en/latest/>

- MySQL: <https://www.mysql.com/>

- Serilog:  <https://serilog.net/>

- Swagger: <https://swagger.io/>

- JWT: <https://auth0.com/docs/secure/tokens/json-web-tokens>

- XUnit <https://xunit.net/>

## Run The App

  To run the App, Ensure you have;
  1) visual studio 2022 and above
  2) MySql
  3) .NET Core 8 and Above

  Clone the app via this url<https://github.com/sholanejo/ClaySolutionsAutomatedDoor.git> into your local machine, open the cloned app and run the app using visual studio to see the swagger documentation.
  At Application startup, a local database is created named claysolutionsautomateddoor if the database does not currently exist.

## Docs

- Post Man Collection: <http://localhost/>

## License

The SecureTrust Bank backend API is open-source and released under the [MIT License](LICENSE). Feel free to modify and use the code as per the terms of the license.
