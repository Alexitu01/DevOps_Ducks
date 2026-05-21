vrrp_script chk_nginx {
    script "pidof nginx"
    interval 2
    weight -101
    fall 2
    rise 2
}

vrrp_instance VI_1 {
    interface eth1
    state ${state}
    priority ${priority}

    virtual_router_id 33
    unicast_src_ip ${src_ip}
    unicast_peer {
        ${peer_ip}
    }

    authentication {
        auth_type PASS
        auth_pass ${password}
    }

    track_script {
        chk_nginx
    }

    notify_master /etc/keepalived/master.sh
}
