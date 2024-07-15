# Automated Door API - A system which allows users to open doors and show historical events to extend user experience beyond classical tags

An ASP.NET Web API which allows users to open doors and show historical events to extend users experience beyond classical tags.
This repository contains the backend API implementation for the Automated Doors API.

## Table of Contents

  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Architecture](#architecture)
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
* Application

## Architecture 

This project is built with CQRS Clean Architectural pattern. A pattern that separates read and update operations for a data store. Implementing CQRS in this application maximizes its performance, scalability, and security. This makes the code easier to manage and with better performance. It improves separates read and write operations, making the system more maintainable and scalable. Promotes a clean architecture by decoupling business logic from controllers.

The project consists of 4 Layers
- ### Api Layer ->
- The API contains endpoints that clients can call to perform operations like creating, reading, updating, and deleting resources.
- ### Application Layer ->
- The Application Layer contains the business logic and orchestrates the interaction between the API layer and the Domain layer.
- ### Domain Layer ->
- The Domain Layer encapsulates the core business logic and rules.
- ### Infrasturcture Layer ->
- The Infrastructure Layer provides implementations for interactions with external systems and technologies, such as databases, etc.


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

  Clone the app via this url <https://github.com/sholanejo/ClaySolutionsAutomatedDoor.git> into your local machine, open the cloned app and run the app using visual studio to see the swagger documentation.
  At Application startup, a local database is created named claysolutionsautomateddoor if the database does not currently exist.

## Docs

- Swagger Docs Image
- ![image](https://github.com/user-attachments/assets/960beabc-0079-4855-bb91-0eb76e0ca18b)
- ![image](https://github.com/user-attachments/assets/5e979f1e-3509-4b79-9d99-d14c08281681)
