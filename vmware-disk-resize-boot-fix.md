# VMware Linux VM Boot Fix After Disk Resize

## Problem Description

After expanding/resizing a virtual hard disk in VMware Workstation for a Linux (Ubuntu) VM, the system fails to boot with one of these symptoms:

- Stuck at GRUB prompt (`grub>`)
- Kernel panic: `VFS: Unable to mount root fs on unknown-block(0,0)`
- Black screen after GRUB
- Missing initrd/initramfs files

**Root Cause:** Resizing the VMware virtual disk can corrupt GRUB configuration, change partition UUIDs, or invalidate the initramfs, preventing the kernel from finding and mounting the root filesystem.

---

## Solution Steps

### Step 1: Boot from Live USB/ISO

1. Download Ubuntu ISO from https://ubuntu.com/download/desktop
2. In VMware: **VM → Settings → CD/DVD**
3. Select **"Use ISO image file"** → Browse to the `.iso`
4. Check **"Connect at power on"**
5. Start VM and press **Esc** repeatedly for boot menu
6. Select **CD-ROM Drive**
7. Choose **"Try Ubuntu"** (not Install)

### Step 2: Identify Partitions

Open Terminal and run:

```bash
lsblk
sudo blkid
```

Identify your root partition (usually the largest ext4 partition, e.g., `/dev/sda3`).

### Step 3: Mount the System

```bash
# Mount root partition
sudo mount /dev/sda3 /mnt

# Mount EFI partition (if exists)
sudo mount /dev/sda2 /mnt/boot/efi

# Mount required virtual filesystems
sudo mount --bind /dev /mnt/dev
sudo mount --bind /proc /mnt/proc
sudo mount --bind /sys /mnt/sys
```

### Step 4: Chroot and Repair

```bash
# Enter the installed system
sudo chroot /mnt

# Reinstall GRUB
grub-install /dev/sda

# Regenerate initramfs (THIS IS CRITICAL)
update-initramfs -c -k all

# Update GRUB configuration
update-grub

# Exit chroot
exit
```

### Step 5: Resize Partition (if needed)

If partition doesn't use full disk space:

```bash
# Using GParted (GUI - recommended)
sudo gparted

# Or command line
sudo parted /dev/sda resizepart 3 100%
sudo e2fsck -f /dev/sda3
sudo resize2fs /dev/sda3
```

### Step 6: Reboot

```bash
sudo umount -R /mnt
sudo reboot
```

Remove/disconnect the Live ISO before the system restarts.

---

## Prevention: Correct Way to Expand VMware Disk

To avoid boot issues in the future:

1. **Create a snapshot first** in VMware
2. Expand the disk in VMware settings (VM powered off)
3. Boot the VM normally
4. From within the running VM:

```bash
# Expand partition (install cloud-guest-utils if needed)
sudo growpart /dev/sda 3

# Expand filesystem
sudo resize2fs /dev/sda3

# Verify
df -h
```

---

## Troubleshooting

### If GRUB menu doesn't appear
- Press **Shift** (BIOS) or **Esc** (UEFI) during boot

### If stuck at `grub>` prompt
```
set root=(hd0,gpt3)
linux /boot/vmlinuz-<version>-generic root=/dev/sda3
initrd /boot/initrd.img-<version>-generic
boot
```

### If kernel panic persists
- Boot from Live USB
- Check `/mnt/etc/fstab` UUID matches `sudo blkid /dev/sda3`
- Regenerate initramfs: `update-initramfs -c -k all`

### Check for missing initrd
```bash
ls /mnt/boot/initrd*
ls /mnt/boot/vmlinuz*
```

If initrd files are missing, regenerate them from chroot.

---

## Quick Reference Commands

| Task | Command |
|------|---------|
| List partitions | `lsblk` |
| Show UUIDs | `sudo blkid` |
| Mount root | `sudo mount /dev/sda3 /mnt` |
| Enter chroot | `sudo chroot /mnt` |
| Fix GRUB | `grub-install /dev/sda && update-grub` |
| Fix initramfs | `update-initramfs -c -k all` |
| Resize partition | `sudo growpart /dev/sda 3` |
| Resize filesystem | `sudo resize2fs /dev/sda3` |
