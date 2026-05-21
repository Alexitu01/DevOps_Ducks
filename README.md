## About the project
This project was first developed during a course where it was called Chirp!. For this course, DevOps Software Evolution and Software Maintenance, during which the project was further supported with load balancing, monitoring, scaling and more.

## How to run the project
Start by cloning the repo to your local machine.
```bash
git clone https://github.com/Alexitu01/DevOps_Ducks.git
```
In order to run the webapplication on your local machine do the following:
```bash
cd src/ITUMiniTwit.Web
dotnet run
```
To spin up the entire project configuration for the first time in deployment, run the following command in the root of the repository:
```bash
terraform apply
```
This will create two new Virtual Machines, hosted on Digital Ocean, with logging, monitoring, reverse proxy, load balancing etc.

## Contributing to project in production
To contribute to the project, start by creating a branch:
```bash
git checkout -b <branch_name>
```
Once done with adding the new feature, fixing a bug etc., push the local branch to the remote.
```bash
git push -u origin <branch_name>
```
When the branch has been pushed to GitHub, create a pull request, wait for the workflows to run the code through mandatory checks, wait for review and approvement from code reviwers and lastly merge the branch to main.

## Tech stack
- ASP .NET libraries
    - Identity
    - Razor Pages
    - EF Core
- Grafana Alloy and Loki
- Prometheus
- Nginx
- Keepalived
- Docker & Docker Compose
- Terraform

## Legacy branch
The branch `ITUMiniTwit.Legacy` contains the original Python-based MiniTwit implementation used earlier in the course.  
It is kept for reference purposes only and is **not under active development**.
