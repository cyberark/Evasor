# Overview

The Evasor is an automated security assessment tool which locates  existing executables on the Windows operating system that can be used to bypass any Application Control rules.
It is very easy to use, quick, saves time and fully automated which generates for you a report including description, screenshots and mitigations suggestions, suites for both blue and red teams in the assessment of a post-exploitation phase.

## Requirements

* Windows OS.
* Visual studio 2017 installed.

## Usage instructions

Download the Evasor project and complie it.
Verify to exclude from the project the App.config file from the reference tree.

<img src="https://github.com/cyberark/Evasor/blob/master/devenv_vTcX5EfWI2.png" width="1000">

run Evasor.exe from the bin folder.
Choose your numeric option from the follwoing:

<img src="https://github.com/cyberark/Evasor/blob/master/chrome_qd6KAL13in.png" width="1000">

1. Locating executable files that can be used to bypass the Application Control!
  *	 Retrieving the all running processes relative paths
  *	 Checking every process (executable file) if it vulnerable to DLL Injection by:
     1.	Running “MavInject” Microsoft component from path C:\Windows\System32\mavinject.exe with default parameters.
     2.	Checking the exit code of the MavInject execution, if the process exited normally it means that the process is vulnerable to DLL Injection and can be used to bypass the Application Control.
2. Locating processes that vulnerable to DLL Hijacking!
  *	 Retrieving the all running processes
  * For each running Process:
    1. Retrieving the loaded process modules
    2. Checking if there is a permission to write data into the directory of the working process by creating an empty file with the name of the loaded module (DLL) or overwriting an existence module file on the working process directory.
    3. If the write operation succeeds – it seems that the process is vulnerable to DLL Hijacking.
3.	Locating for potential hijackable resource files
  *	Searching for specific files on the computer by their extension.
  *	Trying to replace that files to another place in order to validate that the file can be replaceable and finally, potentially vulnerable to Resource Hijacking.
  *	Extensions: xml,config,json,bat,cmd,ps1,vbs,ini,js,exe,dll,msi,yaml,lib,inf,reg,log,htm,hta,sys,rsp
4.	Generating an automatic assessment report word document includes a description of tests and screenshots taken.

## Contributing

We welcome contributions of all kinds to this repository. For instructions on how to get started and descriptions
of our development workflows, please see our [contributing guide](https://github.com/cyberark/conjur-api-go/blob/master/CONTRIBUTING.md).

## License

This repository is licensed under Apache License 2.0 - see [`LICENSE`](LICENSE) for more details.

## Share Your Thoughts And Feedback

For more comments, suggestions or questions, you can contact Arik Kublanov from CyberArk Labs. You can find more projects developed by us in https://github.com/cyberark/.
