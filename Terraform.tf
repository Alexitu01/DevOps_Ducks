terraform {
  required_providers {
    digitalocean = {
      source  = "digitalocean/digitalocean"
      version = "~> 2.0"
    }
  }
}

variable "DIGITAL_OCEAN_TOKEN" {}

provider "digitalocean" {
  token = var.DIGITAL_OCEAN_TOKEN
}

output "vm1_ip" {
  value = digitalocean_droplet.vm1.ipv4_address
}

output "vm2_ip" {
  value = digitalocean_droplet.vm2.ipv4_address
}

data "digitalocean_ssh_keys" "all" {}

locals {
  provision_packages = [
    "sudo apt-get update -y",
    "sudo apt-get install -y apt-transport-https ca-certificates curl gnupg lsb-release git nginx",
    "curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg",
    "echo \"deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable\" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null",
    "sudo apt-get update -y",
    "sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin certbot python3-certbot-nginx",
    "sudo systemctl enable docker nginx",
    "sudo systemctl start docker nginx",
  ]
  provision_nginx = [
    "sudo ln -sf /etc/nginx/sites-available/devopsducks.studio /etc/nginx/sites-enabled/devopsducks.studio",
    "sudo rm -f /etc/nginx/sites-enabled/default",
    "echo 'proxy_pass http://blue;' | sudo tee /etc/nginx/active_upstream.conf",
    "sudo nginx -t && sudo systemctl reload nginx",
  ]
}

resource "digitalocean_droplet" "vm1" {
  image              = "ubuntu-24-04-x64"
  name               = "web-1"
  region             = "fra1"
  size               = "s-2vcpu-4gb"
  ssh_keys           = data.digitalocean_ssh_keys.all.ssh_keys[*].id
  private_networking = true

  provisioner "remote-exec" {
    inline = local.provision_packages

    connection {
      type        = "ssh"
      user        = "root"
      private_key = file("~/.ssh/id_rsa")
      host        = self.ipv4_address
    }
  }

  provisioner "file" {
    source      = "nginx/devopsducks.studio"
    destination = "/etc/nginx/sites-available/devopsducks.studio"

    connection {
      type        = "ssh"
      user        = "root"
      private_key = file("~/.ssh/id_rsa")
      host        = self.ipv4_address
    }
  }

  provisioner "remote-exec" {
    inline = local.provision_nginx

    connection {
      type        = "ssh"
      user        = "root"
      private_key = file("~/.ssh/id_rsa")
      host        = self.ipv4_address
    }
  }
}

resource "digitalocean_droplet" "vm2" {
  image              = "ubuntu-24-04-x64"
  name               = "web-2"
  region             = "fra1"
  size               = "s-2vcpu-4gb"
  ssh_keys           = data.digitalocean_ssh_keys.all.ssh_keys[*].id
  private_networking = true

  provisioner "remote-exec" {
    inline = local.provision_packages

    connection {
      type        = "ssh"
      user        = "root"
      private_key = file("~/.ssh/id_rsa")
      host        = self.ipv4_address
    }
  }

  provisioner "file" {
    source      = "nginx/devopsducks.studio"
    destination = "/etc/nginx/sites-available/devopsducks.studio"

    connection {
      type        = "ssh"
      user        = "root"
      private_key = file("~/.ssh/id_rsa")
      host        = self.ipv4_address
    }
  }

  provisioner "remote-exec" {
    inline = local.provision_nginx

    connection {
      type        = "ssh"
      user        = "root"
      private_key = file("~/.ssh/id_rsa")
      host        = self.ipv4_address
    }
  }
}

resource "digitalocean_firewall" "web" {
  name = "minitwit-firewall"

  droplet_ids = [
    digitalocean_droplet.vm1.id,
    digitalocean_droplet.vm2.id,
  ]

  # SSH
  inbound_rule {
    protocol         = "tcp"
    port_range       = "22"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # HTTP
  inbound_rule {
    protocol         = "tcp"
    port_range       = "80"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # HTTPS
  inbound_rule {
    protocol         = "tcp"
    port_range       = "443"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # Grafana
  inbound_rule {
    protocol         = "tcp"
    port_range       = "3000"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  outbound_rule {
    protocol              = "tcp"
    port_range            = "1-65535"
    destination_addresses = ["0.0.0.0/0", "::/0"]
  }

  outbound_rule {
    protocol              = "udp"
    port_range            = "1-65535"
    destination_addresses = ["0.0.0.0/0", "::/0"]
  }

  outbound_rule {
    protocol              = "icmp"
    destination_addresses = ["0.0.0.0/0", "::/0"]
  }
}

# To import the existing reserved IP: terraform import digitalocean_reserved_ip.web 68.183.242.15
resource "digitalocean_reserved_ip" "web" {
  region = "fra1"
}

output "reserved_ip" {
  value = digitalocean_reserved_ip.web.ip_address
}
