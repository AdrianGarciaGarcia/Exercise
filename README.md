Exercice Project
=================

An exercise project demonstrating the use of the minimalapi approach combined with the vertical slice architecture.

### Key Libraries
MediatR: Implements the mediator pattern in .NET, aiding in decoupling in-process messaging.
Scrutor: Scans assemblies and registers classes, facilitating integration with minimalapi and vertical slice.

### Architecture
MinimalAPI: This approach is used to create HTTP APIs with minimal configuration. It's a lightweight, high-performance way to build web APIs in .NET.
Vertical Slice: Instead of organizing the codebase around technical concerns (controllers, services, repositories), the vertical slice architecture organizes it around features or use-cases.

## Prerequisites
Visual Studio.
A valid key for the external api communication.

## Setup and Run
1. Clone the Repository.
2. Open in Visual Studio.
3. Insert the Key: Place the provided key into the Api appsettings.json file and uncomment it.
4. Set Startup Projects: Ensure both projects (Api and UI) are selected as startup projects.
5. Run the project.
