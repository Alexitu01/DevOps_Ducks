# DevOpsDucks - Group 1

## Introduction

## System's Perspective

Description and illustration of:
1. Design and architecture of your ITU-MiniTwit systems.
2. All dependencies of your ITU-MiniTwit systems on all levels of abstraction and development stages. That is, list and briefly describe all technologies and tools you applied and depend on.
3. Describe the current state of your systems, for example using results of static analysis and quality assessments
\
This chapter outlines the system structure of our MiniTwit application and its current dependencies. The current system state will be illustrated and explained in detail. 


### System Structure
- Design and architecture
[Insert architecture Diagram] 

#### Application
MiniTwit is an ASP.NET Core web application based on the earlier 'Chirp' project: It extendeds the support for both browser-based interaction and API-based simulator traffic. The application is structured into presentation, service, and repository layers. Razor Pages handle the user-facing web interface, while controllers expose API endpoints used to handle simulator traffic.

For persistence, the application uses Entity Framework Core with a MySQL database hosted on Aiven instead of the earlier used SQLite setup from Chirp. All deployed application containers share the same database through a common connection string. This allows multiple running instances of the application to serve requests while persisting data in the same central database.

- Possibly Talk about LoginMetrics + Add other Metrics.




#### Infrastructure



#### Monitoring and Logging



### Dependencies
- List of dependencies

### Current System State
- Talk about the current "condition"
*-* Weaknesses
*-* Known bugs
*-* SonarCloud
*-* Tests?


## Process' Perspective
Clarify how code or other artifacts come from idea to running system. Include the following:
1. A complete description and illustration of stages and tools included in the CI/CD pipelines, including deployment and release of your systems.
2. How do you monitor your systems and what precisely do you monitor?
3. What do you log in your systems and how do you aggregate logs?
4. Brief description of how you security hardened your systems.
5.  How do you handle availability and scaling in your systems?

### System Stages
- Illustration of individual steps in CI/CD pipeline
*-* Github Actions
*-* Docker Compose
*-* Terraform


### Monitoring
- Minotor structure + what are we monitoring

### Logging


### Security
- Closing off exposed ports
- Firewall
- Github Secrets
- HTTPS
- Reverse proxy...?



### Availability
- Multiple droplets
- Load Balancer
- Green-blue architecture


## Reflection Perspective
The biggest issues and how they were solved:
1. evolution and refactoring
2. operation, and
3. maintenance
Link back to commit messages, issues, tickets etc.
Also reflect and describe what was the "DevOps" style of your work. For example, what did you do differently to previous development projects and how did it work?

### Database issues

### Logging issues

### VM Corruption

### Spillover


## Use of Generative AI

### For niche issues (undocumented)
* Database thieves
* Alloy config

### CLI Commands for debugging
* nginx logs
* keepalived logs
* docker logs

