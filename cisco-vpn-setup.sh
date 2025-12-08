#!/bin/bash
#
# Cisco Secure Client Installation and Certificate Import Script
# For Ubuntu/Debian Linux
#
# Usage:
#   ./cisco-vpn-setup.sh install <path-to-cisco-package>
#   ./cisco-vpn-setup.sh import-cert <path-to-certificate.p12>
#   ./cisco-vpn-setup.sh connect <vpn-server-address>
#   ./cisco-vpn-setup.sh status
#   ./cisco-vpn-setup.sh disconnect
#

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Cisco Secure Client paths
CISCO_BIN="/opt/cisco/secureclient/bin"
CISCO_PROFILE_DIR="/opt/cisco/secureclient/profile"
CISCO_CERTS_DIR="/opt/cisco/secureclient/certs"

log_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

log_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

check_root() {
    if [[ $EUID -ne 0 ]]; then
        log_error "This script must be run as root (use sudo)"
        exit 1
    fi
}

install_dependencies() {
    log_info "Installing dependencies..."
    apt-get update
    apt-get install -y \
        libpango-1.0-0 \
        libpangocairo-1.0-0 \
        libgtk-3-0 \
        libwebkit2gtk-4.0-37 \
        libnss3 \
        libnspr4 \
        libsecret-1-0 \
        openssl \
        ca-certificates \
        network-manager
}

install_cisco_client() {
    local package_path="$1"

    if [[ -z "$package_path" ]]; then
        log_error "Please provide path to Cisco Secure Client package"
        echo "Usage: $0 install <path-to-package.deb|tar.gz>"
        exit 1
    fi

    if [[ ! -f "$package_path" ]]; then
        log_error "Package file not found: $package_path"
        exit 1
    fi

    check_root
    install_dependencies

    # Determine package type and install
    if [[ "$package_path" == *.deb ]]; then
        log_info "Installing from .deb package..."
        dpkg -i "$package_path" || true
        apt-get install -f -y

    elif [[ "$package_path" == *.tar.gz ]] || [[ "$package_path" == *.tgz ]]; then
        log_info "Installing from tarball..."
        local temp_dir=$(mktemp -d)
        tar -xzf "$package_path" -C "$temp_dir"

        # Find and run the installer script
        local installer=$(find "$temp_dir" -name "vpn_install.sh" -o -name "cisco-secure-client-linux*.sh" | head -1)

        if [[ -z "$installer" ]]; then
            # Try to find any install script
            installer=$(find "$temp_dir" -name "*.sh" -type f | head -1)
        fi

        if [[ -n "$installer" ]]; then
            chmod +x "$installer"
            bash "$installer"
        else
            log_error "Could not find installer script in package"
            rm -rf "$temp_dir"
            exit 1
        fi

        rm -rf "$temp_dir"
    else
        log_error "Unsupported package format. Use .deb or .tar.gz"
        exit 1
    fi

    # Verify installation
    if [[ -x "${CISCO_BIN}/vpn" ]]; then
        log_info "Cisco Secure Client installed successfully!"
        "${CISCO_BIN}/vpn" --version 2>/dev/null || true
    else
        log_error "Installation may have failed - vpn binary not found"
        exit 1
    fi
}

import_certificate() {
    local cert_path="$1"
    local cert_password="$2"

    if [[ -z "$cert_path" ]]; then
        log_error "Please provide path to certificate file (.p12 or .pfx)"
        echo "Usage: $0 import-cert <certificate.p12> [password]"
        exit 1
    fi

    if [[ ! -f "$cert_path" ]]; then
        log_error "Certificate file not found: $cert_path"
        exit 1
    fi

    check_root

    # Create certs directory if it doesn't exist
    mkdir -p "$CISCO_CERTS_DIR"
    mkdir -p "/root/.cisco/certificates"

    # Get password if not provided
    if [[ -z "$cert_password" ]]; then
        read -s -p "Enter certificate password: " cert_password
        echo
    fi

    local cert_basename=$(basename "$cert_path" | sed 's/\.[^.]*$//')
    local pem_cert="/root/.cisco/certificates/${cert_basename}.pem"
    local pem_key="/root/.cisco/certificates/${cert_basename}-key.pem"

    log_info "Extracting certificate and private key..."

    # Extract certificate
    openssl pkcs12 -in "$cert_path" -clcerts -nokeys -out "$pem_cert" -passin pass:"$cert_password" 2>/dev/null

    # Extract private key
    openssl pkcs12 -in "$cert_path" -nocerts -nodes -out "$pem_key" -passin pass:"$cert_password" 2>/dev/null

    # Set proper permissions
    chmod 600 "$pem_key"
    chmod 644 "$pem_cert"

    # Also copy .p12 to Cisco certs directory
    cp "$cert_path" "${CISCO_CERTS_DIR}/"
    chmod 600 "${CISCO_CERTS_DIR}/$(basename "$cert_path")"

    # Import to system certificate store (for some configurations)
    log_info "Adding certificate to system store..."

    # Create directory for user certificates
    mkdir -p /usr/local/share/ca-certificates/cisco-vpn/
    cp "$pem_cert" "/usr/local/share/ca-certificates/cisco-vpn/${cert_basename}.crt"
    update-ca-certificates 2>/dev/null || true

    # Import to NSS database (used by Cisco client)
    if command -v certutil &>/dev/null; then
        log_info "Importing to NSS database..."
        mkdir -p /root/.pki/nssdb
        certutil -d sql:/root/.pki/nssdb -N --empty-password 2>/dev/null || true
        pk12util -d sql:/root/.pki/nssdb -i "$cert_path" -W "$cert_password" 2>/dev/null || true
    else
        log_warn "certutil not found, installing libnss3-tools..."
        apt-get install -y libnss3-tools
        mkdir -p /root/.pki/nssdb
        certutil -d sql:/root/.pki/nssdb -N --empty-password 2>/dev/null || true
        pk12util -d sql:/root/.pki/nssdb -i "$cert_path" -W "$cert_password" 2>/dev/null || true
    fi

    log_info "Certificate imported successfully!"
    log_info "Certificate: $pem_cert"
    log_info "Private key: $pem_key"
    log_info "P12 copy: ${CISCO_CERTS_DIR}/$(basename "$cert_path")"

    # Display certificate info
    echo
    log_info "Certificate details:"
    openssl x509 -in "$pem_cert" -noout -subject -dates 2>/dev/null
}

connect_vpn() {
    local server="$1"

    if [[ -z "$server" ]]; then
        log_error "Please provide VPN server address"
        echo "Usage: $0 connect <vpn-server-address>"
        exit 1
    fi

    if [[ ! -x "${CISCO_BIN}/vpn" ]]; then
        log_error "Cisco Secure Client not installed. Run: $0 install <package>"
        exit 1
    fi

    log_info "Connecting to $server..."
    "${CISCO_BIN}/vpn" connect "$server"
}

disconnect_vpn() {
    if [[ ! -x "${CISCO_BIN}/vpn" ]]; then
        log_error "Cisco Secure Client not installed"
        exit 1
    fi

    log_info "Disconnecting..."
    "${CISCO_BIN}/vpn" disconnect
}

show_status() {
    if [[ ! -x "${CISCO_BIN}/vpn" ]]; then
        log_error "Cisco Secure Client not installed"
        exit 1
    fi

    "${CISCO_BIN}/vpn" status
}

show_help() {
    echo "Cisco Secure Client Setup Script"
    echo
    echo "Usage: $0 <command> [options]"
    echo
    echo "Commands:"
    echo "  install <package>      Install Cisco Secure Client from .deb or .tar.gz"
    echo "  import-cert <p12>      Import .p12/.pfx certificate for authentication"
    echo "  connect <server>       Connect to VPN server"
    echo "  disconnect             Disconnect from VPN"
    echo "  status                 Show connection status"
    echo "  help                   Show this help message"
    echo
    echo "Examples:"
    echo "  sudo $0 install cisco-secure-client.deb"
    echo "  sudo $0 import-cert mycert.p12"
    echo "  sudo $0 connect vpn.company.com"
    echo
    echo "Certificate Export (Windows):"
    echo "  1. Open certmgr.msc"
    echo "  2. Find your certificate under Personal > Certificates"
    echo "  3. Right-click > All Tasks > Export"
    echo "  4. Select 'Yes, export the private key'"
    echo "  5. Choose PKCS #12 (.PFX) format"
    echo "  6. Set a password and save"
    echo "  7. Copy the .pfx file to Linux"
}

# Main entry point
case "${1:-help}" in
    install)
        install_cisco_client "$2"
        ;;
    import-cert)
        import_certificate "$2" "$3"
        ;;
    connect)
        connect_vpn "$2"
        ;;
    disconnect)
        disconnect_vpn
        ;;
    status)
        show_status
        ;;
    help|--help|-h)
        show_help
        ;;
    *)
        log_error "Unknown command: $1"
        show_help
        exit 1
        ;;
esac
