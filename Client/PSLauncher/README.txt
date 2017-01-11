The PSEmulator Launcher will perform client-side updates.

In the PSLauncher.exe.config, you will find various variables that can be 
configured to change how the program operations. Below is a detail list of 
what each variable does.

// TODO: Populate list.

UpdateServerIP: The IP address to the Update Server responsible for hosting
the client files.

UpdateServerUser: The username for FTP login. This user should have readonly
permissions to a directory containing the client files.

UpdateServerPassword: The password for the user described above.