# DevOpsDucks - Group 1

## Introduction [EMPTY]

## System's Perspective

Description and illustration of:
1. Design and architecture of your ITU-MiniTwit systems.
2. All dependencies of your ITU-MiniTwit systems on all levels of abstraction and development stages. That is, list and briefly describe all technologies and tools you applied and depend on.
3. Describe the current state of your systems, for example using results of static analysis and quality assessments
\
This chapter outlines the system structure of our MiniTwit application and its current dependencies. The current system state will be illustrated and explained in detail. 


### System Structure [Needs-Diagram]
- Design and architecture
[Insert architecture Diagram] 

#### Application [HALF-WRITTEN] 
MiniTwit is an ASP.NET Core web application based on the earlier 'Chirp' project: It extendeds the support for both browser-based interaction and API-based simulator traffic. Razor Pages handle the user-facing web interface, while controllers expose API endpoints used to handle simulator traffic.

For persistence, the application uses Entity Framework Core with a MySQL database hosted on Aiven instead of the earlier used SQLite setup from Chirp. All of the deployed application containers share the same database through a common connection string. This allows multiple running instances of the application to serve requests while persisting data in the same database.

The application is wrapped within a container and joined with monitoring and logging systems through a shared network.

- Possibly Talk about LoginMetrics + Add other Metrics.
[Needs info from team]



#### Infrastructure [EMPTY] + [Needs-Diagram]
The production system is hosted on two DigitalOcean droplets (Virtual Machines). The droplets contain the runtime environment for the application while supporting horizontal scaling and failover. A reserved IP address is used for both droplets, giving the system a single point of entry. Incoming traffic is then distributed by Nginx among the available application instances.

The deployed services are managed with Docker Compose, defining the containers that run in the runtime environment, setting up a shared network and the supporting monitoring and logging systems. The application is deployed as multiple containers, based on the same image. The applications all refer to the same MySQL database hosted on Aiven through a common connection string.

Nginx is used as a reverse proxy and load balancer, repsonsible for forwarding incomming traffic to the currently active environment and available containers while using an 'ip_hash' . Keepalived is used between the two droplets, supporting recovery of the deployed environment, if one of the droplets become unavailable.

To support safer deployment, the infrastructure uses a blue-green setup. Two blue and two green containers are defined in the docker compose file, allwoing a new version of the application to be started and verified before traffic is switched.

The infrastructure includes monitoring and logging components such as Prometheus and Grafana, and Alloy and Loki.


#### Monitoring and Logging [EMPTY]




### Dependencies [EMPTY] + [Needs-Diagram]
- List of dependencies


### Current System State [EMPTY] 
- Talk about the current "condition"
*-* Weaknesses
*-* Known bugs
*-* SonarCloud
*-* Tests?


## Process' Perspective [EMPTY]
Clarify how code or other artifacts come from idea to running system. Include the following:
1. A complete description and illustration of stages and tools included in the CI/CD pipelines, including deployment and release of your systems.
2. How do you monitor your systems and what precisely do you monitor?
3. What do you log in your systems and how do you aggregate logs?
4. Brief description of how you security hardened your systems.
5.  How do you handle availability and scaling in your systems?

### System Stages [EMPTY]
- Illustration of individual steps in CI/CD pipeline
*-* Github Actions
*-* Docker Compose
*-* Terraform


### Monitoring [EMPTY]
- Minotor structure + what are we monitoring

### Logging [EMPTY]


### Security [EMPTY]
- Closing off exposed ports
- Firewall
- Github Secrets
- HTTPS
- Reverse proxy...?



### Availability [EMPTY]
- Multiple droplets
- Load Balancer
- Green-blue architecture


## Reflection Perspective [EMPTY]
The biggest issues and how they were solved:
1. evolution and refactoring
2. operation, and
3. maintenance
Link back to commit messages, issues, tickets etc.
Also reflect and describe what was the "DevOps" style of your work. For example, what did you do differently to previous development projects and how did it work?


### Database issues [EMPTY]

### Logging issues [EMPTY]

### VM Corruption [EMPTY]

### Spillover [EMPTY]


## Use of Generative AI [EMPTY]


### For niche issues (undocumented) [EMPTY]
* Database thieves
* Alloy config


### CLI Commands for debugging [EMPTY]
* nginx logs
* keepalived logs
* docker logs

