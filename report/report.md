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

![Systems Diagram](.\Diagrams\DevOps_Systems_Final.drawio.png "Systems Structure Diagram")

#### Application [HALF-WRITTEN] 
MiniTwit is an ASP.NET Core web application based on the earlier 'Chirp' project: It extendeds the support for both browser-based interaction and API-based simulator traffic. Razor Pages handle the user-facing web interface, while controllers expose API endpoints used to handle simulator traffic.

For persistence, the application uses Entity Framework Core with a MySQL database hosted on Aiven instead of the earlier used SQLite setup from Chirp. All of the deployed application containers share the same database through a common connection string. This allows multiple running instances of the application to serve requests while persisting data in the same database.

The application is wrapped within a container and joined with monitoring and logging systems through a shared network.

- Possibly Talk about LoginMetrics + Add other Metrics.
[Needs info from team]



#### Infrastructure [WRITTEN] [Needs-Diagram]
The production system is hosted on two DigitalOcean droplets (Virtual Machines). The droplets contain the runtime environment for the application while supporting horizontal scaling and failover. A reserved IP address is used for both droplets, giving the system a single point of entry. Incoming traffic is then distributed by Nginx among the available application instances.

The deployed services are managed with Docker Compose, defining the containers that run in the runtime environment, setting up a shared network and the supporting monitoring and logging systems. The application is deployed as multiple containers, based on the same image. The applications all refer to the same MySQL database hosted on Aiven through a common connection string.

Nginx is used as a reverse proxy and load balancer, repsonsible for forwarding incomming traffic to the currently active environment and available containers while using an 'ip_hash' . Keepalived is used between the two droplets, supporting recovery of the deployed environment, if one of the droplets become unavailable.

To support safer deployment, the infrastructure uses a blue-green setup. Two blue and two green containers are defined in the docker compose file, allwoing a new version of the application to be started and verified before traffic is switched.

The infrastructure includes monitoring and logging components such as Prometheus and Grafana, and Alloy and Loki.


#### Monitoring and Logging [WRITTEN]
As mentioned, our monitoring was mainly done with Prometheus and Grafana. Via the prometheus.yml file, Prometheus collected metrics from both blue and green containers, while Grafana's UI made it visually structured.
Prometheus was used to mainly monitor the 'performance' of the different containers. Latency was monitored to make sure that the response time of the application was acceptable - specifically with new deployments.
The total memory use of dotnet was monitored, to detect unusual resource consumption, and verify that the application was stable for new deployments (by comparing them to the old deployments).
The number of requests were also monitored to see which endpoints were visited often, and what kind of responses those requests would get. (Although this was not relied upon in this course, - since the simulator used API endpoints - in a real-life scenario this sort of monitoring would be important.)

With the alloy.config file Alloy scraped logs from the containers and passed them to Loki. Loki collected the logs and by using Grafana, the data also became visually structured.
We used Loki to differentiate between simulator 404 responses and bot 404 responses, making it possible to debug and act upon 'real' failed requests to the webserver API.




#### Dependencies [EMPTY] + [Needs-Diagram]
##### .NET
Within the .NET library we used Entitiy Frameowkr Core as a way to read and write to the databse, while Identity was used as a way to structure the data and used for authentication.

##### Docker / Docker Compose
Docker was used for conainerising the application and Docker Compose was used to manage services and ensure a shared network.
hadolint...?
##### Github Actions
Github actions managed the pipeline of integrating and deploying the application. 

##### Grafana, Prometheus, Loki, Alloy
Grafana, Prometheus, Loki, and Alloy were used as the system’s observability stack, supporting monitoring, log collection and storage, and visualization of operational data.

##### Keepalived
Keepalived was setup as a channel of communication between droplets ensuring failover.

##### Nginx
Nginx worked like the reverse proxy and also as a Load Balancer, ensuring traffic was handled effectively.


##### SonarCloud
SonarCloud was used to quality check the system.


##### MySQL
Used as the database




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

