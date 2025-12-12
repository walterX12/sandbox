# Prompt: Fix VMware Ubuntu Guest Boot Failure via Windows MCP

## Context & Environment

You are Claude Code running on Windows with access to Windows MCP tools.

On this Windows machine:
- VMware Workstation is installed
- There is a Ubuntu Linux VM guest that cannot boot
- You have access to Windows filesystem, command line, and can control VMware via vmrun CLI tool

## Original Problem

The user had a working Ubuntu Linux VM in VMware Workstation. They expanded/resized the virtual hard disk in VMware settings. After the disk resize, the Ubuntu VM fails to boot.

### Symptoms Observed:
1. Initially stuck at: /dev/sda3: clean, 268343/950272 files, 3722087/3800064 blocks
2. Later: Kernel panic - VFS: Unable to mount root fs on unknown-block(0,0)
3. GRUB menu appears but all boot options (including recovery mode) fail
4. At grub> prompt: initrd files are missing (/boot/initrd.img-6.8.0-88-generic not found)

### Root Cause:
Resizing VMware virtual disk corrupted/invalidated the initramfs files. The kernel cannot find the initial ramdisk needed to mount the root filesystem.

### VM Disk Layout:
- /dev/sda - 20GB disk
- /dev/sda1 - 1MB (BIOS boot / grub2 core.img)
- /dev/sda2 - 513MB (EFI System Partition, FAT32)
- /dev/sda3 - ~19.5GB (Root partition, ext4) - already resized to use full disk

## What We Already Tried

### Partially Worked:
1. Booted from Ubuntu Live ISO
2. Mounted /dev/sda3 to /mnt
3. Ran grub-install /dev/sda - completed with warnings
4. Ran update-grub - found kernel images
5. Resized partition to use full 20GB via GParted - successful

### Did NOT Work:
1. Normal boot - kernel panic
2. Recovery mode boot - same kernel panic
3. Manual GRUB boot - initrd file not found

### Still Needed:
Regenerate initramfs - this was not completed. The command needed:

sudo chroot /mnt
update-initramfs -c -k all
exit

## Your Task

Use Windows MCP tools to fix this Ubuntu VM. You have access to:

### VMware CLI Tool (vmrun)
Located typically at: C:\Program Files (x86)\VMware\VMware Workstation\vmrun.exe

Available commands:

List running VMs:
vmrun list

Power operations:
vmrun stop "C:\path\to\vm.vmx" hard
vmrun start "C:\path\to\vm.vmx"
vmrun reset "C:\path\to\vm.vmx"

Snapshot management:
vmrun listSnapshots "C:\path\to\vm.vmx"
vmrun revertToSnapshot "C:\path\to\vm.vmx" "snapshot_name"

Guest operations (requires VMware Tools running - NOT available when VM won't boot):
vmrun runScriptInGuest "C:\path\to\vm.vmx" "/bin/bash" "command"

### VMX File Editing
You can edit the .vmx configuration file to:
- Change boot order
- Connect/disconnect CD/DVD ISO
- Modify VM settings

Example VMX entries for CD boot:

ide1:0.deviceType = "cdrom-image"
ide1:0.fileName = "C:\path\to\ubuntu.iso"
ide1:0.present = "TRUE"
bios.bootOrder = "cdrom,hdd"

## Step-by-Step Instructions

### Step 1: Find the VM
Use MCP to search for .vmx files or check common VMware VM locations:
- C:\Users\<username>\Documents\Virtual Machines\
- C:\VMs\

PowerShell command:
Get-ChildItem -Path "C:\Users" -Recurse -Filter "*.vmx" -ErrorAction SilentlyContinue

### Step 2: Find Ubuntu Live ISO
Search for existing ISO or note that user needs to download one:

Get-ChildItem -Path "C:\" -Recurse -Filter "ubuntu*.iso" -ErrorAction SilentlyContinue

### Step 3: Power Off the VM

& "C:\Program Files (x86)\VMware\VMware Workstation\vmrun.exe" stop "C:\path\to\vm.vmx" hard

### Step 4: Configure VM to Boot from ISO
Edit the .vmx file to:
1. Connect Ubuntu Live ISO to CD/DVD
2. Set CD-ROM as first boot device

### Step 5: Power On and Guide User
After VM starts, you CANNOT control the GRUB menu or type commands into the Live Ubuntu terminal via MCP.

You must instruct the user to:
1. Select "Try Ubuntu" from boot menu
2. Open Terminal
3. Run these commands:

sudo mount /dev/sda3 /mnt
sudo mount /dev/sda2 /mnt/boot/efi
sudo mount --bind /dev /mnt/dev
sudo mount --bind /proc /mnt/proc
sudo mount --bind /sys /mnt/sys
sudo chroot /mnt
update-initramfs -c -k all
update-grub
exit
sudo umount -R /mnt
sudo reboot

### Step 6: Disconnect ISO After Fix
Edit .vmx to disconnect ISO and set HDD as first boot:

ide1:0.present = "FALSE"
bios.bootOrder = "hdd,cdrom"

### Step 7: Boot Normally

& "C:\Program Files (x86)\VMware\VMware Workstation\vmrun.exe" start "C:\path\to\vm.vmx"

## MCP Limitations

What you CAN do via Windows MCP:
- Find VM files (.vmx, .vmdk)
- Read/edit VMX configuration
- Power on/off/reset VM via vmrun
- Manage snapshots
- Mount/unmount ISO images
- Change boot order

What you CANNOT do via Windows MCP:
- Type into VM console (GRUB prompt, Live USB terminal)
- Click GUI elements in VMware window
- Run commands inside guest when OS won't boot (no VMware Tools)
- Interact with BIOS/UEFI firmware menu

## Expected Outcome

After following these steps:
1. VM boots from Ubuntu Live ISO
2. User manually runs chroot commands to regenerate initramfs
3. VM reboots and boots normally from hard disk
4. Ubuntu login screen appears
5. Disk shows full 20GB capacity

## Fallback: If Nothing Works

If initramfs regeneration fails, the user may need to:
1. Backup data from /dev/sda3 using Live USB
2. Reinstall Ubuntu fresh
3. Restore data from backup

To backup via Live USB:

sudo mount /dev/sda3 /mnt
cp -r /mnt/home /media/ubuntu/external-drive/
