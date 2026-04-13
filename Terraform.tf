terraform {
  required_providers {
    digitalocean = {
      source = "digitalocean/digitalocean"
      version = "~> 2.0"
    }
  }
}

variable "DIGITAL_OCEAN_TOKEN" {}

provider "digitalocean" {
  token = var.DIGITAL_OCEAN_TOKEN
}

# Added the stuff beneath, it prints the IP address to the console
output "droplet_ip"{
  value = digitalocean_droplet.ubuntu-s-2vcpu-4gb-fra1-01.ipv4_address
}

data "digitalocean_ssh_keys" "all" {
}

resource "digitalocean_droplet" "ubuntu-s-2vcpu-4gb-fra1-01" {
  image              = "ubuntu-24-04-x64"
  name               = "web"
  region             = "fra1"
  size               = "s-2vcpu-4gb"
  ssh_keys           = data.digitalocean_ssh_keys.all.ssh_keys[*].id
  private_networking = true


  provisioner "remote-exec" {
    inline = [
      "sudo apt-get update -y",
      "sudo apt-get install -y apt-transport-https ca-certificates curl gnupg lsb-release git",
      "curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg",
      "echo \"deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable\" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null",
      "sudo apt-get update -y",
      "sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin",
      "sudo systemctl enable docker",
      "sudo systemctl start docker",
      "cd /root"
    ]
    
    connection {
      type        = "ssh"
      user        = "root"
      private_key = file("~/.ssh/id_rsa")
      host        = digitalocean_droplet.ubuntu-s-2vcpu-4gb-fra1-01.ipv4_address
    }
  }
}
