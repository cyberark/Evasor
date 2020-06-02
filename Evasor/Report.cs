using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evasor
{
    public static class Report
    {
        static string Technical_Details = string.Empty;
        static string Issues = string.Empty;
        static string Recommendations_for_mitigation = string.Empty;
        static string Refrences = string.Empty;
        static string Proof_of_Concept = string.Empty;
        static string apps = string.Empty;
        static string apps_h = string.Empty;
        static string apps_r = string.Empty;
        static string pocs_and_pic = string.Empty;

        public static void Scan_Vulnerable_Process_To_DLL_Injection(List<string> i_Results, List<string> i_Pocs, List<string> i_Pics)
        {
            Technical_Details = @"DLL injection is a technique which allows an attacker to run arbitrary code in the context of the address space of another process. If this process is running with excessive privileges then it could be abused by an attacker in order to execute malicious code in the form of a DLL file in order to elevate privileges or migrating to the target process in order to persist the open session to the target or to abuse the process ability to access is folder resources. Specifically, this technique follows the steps below: 
• A DLL needs to be dropped into the disk 
• The “CreateRemoteThread” calls the “LoadLibrary” 
• The reflective loader function will try to find the Process Environment Block (PEB) of the target process using the appropriate CPU register and from that will try to find the address in memory of kernel32dll and any other required libraries. 
• Discovery of the memory addresses of required API functions such as LoadLibraryA, GetProcAddress, and VirtualAlloc. The functions above will be used to properly load the DLL into memory and call its entry point DllMain which will execute the DLL.
";
            Refrences = @"https://pentestlab.blog/2017/04/04/dll-injection/";
            foreach (string app in i_Results)
            {
                string[] subPaths = app.Split('^');
                apps += subPaths[0] + " Owner:" + subPaths[2] + "\r\n";
            }

            for (int i = 0; i < i_Pocs.Count; i++)
            {
                pocs_and_pic += i_Pocs[i] + "\r\n" + i_Pics[i] + "\r\n";
            }

            Issues = @"We had found that we can inject DLL's to the following Process:" + "\r\n" + apps;
            Recommendations_for_mitigation = "None";
            Proof_of_Concept = "Conducted at " + DateTime.Now.ToString() + " -As you can see typing at the RUN text-box the following Proof of concept will result with process being DLL injected to them. \r\n" + pocs_and_pic;
            create_word_report(Technical_Details, Issues, Refrences, Recommendations_for_mitigation, Proof_of_Concept, i_Pics, "3.DLL injection", "3");
            ResetArguments();
        }

        public static void Scan_For_Executibles_That_Can_Bypass_Windows_AppControl(List<string> i_Results, List<string> i_Pocs, List<string> i_Pics)
        {
            Technical_Details = @"The goal of this test is to check the most common techniques to bypass AppControl restrictions and block rules. This test contains a complete list of all known bypasses. Since AppControl rules can be configured in different ways it makes sense to check them all.";
            Refrences = @"https://github.com/milkdevil/UltimateAppLockerByPassList";
            foreach (string app in i_Results)
            {
                apps += app + "\r\n";
            }
            for (int i = 0; i < i_Pocs.Count; i++)
            {
                pocs_and_pic += i_Pocs[i] + "\r\n" + i_Pics[i] + "\r\n";
            }
            Issues = @"We had found that we can execute the following applications which can be used to bypass windows AppControl restriction block applications:" + "\r\n" + apps;
            Recommendations_for_mitigation = "Configure the OS not to allow running those applications!!!";
            Proof_of_Concept = "Conducted at " + DateTime.Now.ToString() + "- As you can see typing at the RUN text-box the following Proof of concept will bypass AppControl restricted rules. \r\n" + pocs_and_pic;
            create_word_report(Technical_Details, Issues, Refrences, Recommendations_for_mitigation, Proof_of_Concept, i_Pics, "1.AppControl restriction rules bypass", "1");
            ResetArguments();
        }

        public static void Scan_for_Process_vulnerable_to_DLL_hijack_DLL_replacement(List<string> i_Results1, List<string> i_Results2, List<string> i_Pocs, List<string> i_Pics)
        {
            Technical_Details = @"In Windows environments when an application or a service is starting it looks for a number of DLL’s in order to function properly. If these DLL’s doesn’t exist or are implemented in an insecure way (DLL’s are called without using a fully qualified path) then it is possible to escalate privileges by forcing the application to load and execute a malicious DLL file. It should be noted that when an application needs to load a DLL it will go through the following order: 
The directory from which the application is loaded 
C:\Windows\System32 
C:\Windows\System 
C:\Windows 
The current working directory 
Directories in the system PATH environment variable Directories in the user PATH environment variable.
";
            Refrences = @"https://pentestlab.blog/2017/03/27/dll-hijacking/";
            foreach (string app in i_Results1)
            {
                string[] subPaths = app.Split('^');
                apps_h += subPaths[1] + "\r\n";
            }
            foreach (string app in i_Results2)
            {
                string[] subPaths = app.Split('^');
                apps_r += subPaths[1] + "\r\n";
            }
            for (int i = 0; i < i_Pocs.Count; i++)
            {
                pocs_and_pic += i_Pocs[i] + "\r\n" + i_Pics[i] + "\r\n";
            }
            Issues = @"We had found that we can hijack/replace the following DLL’s:" + "\r\n" + apps_h + "\r\n" + apps_r;
            Recommendations_for_mitigation = "None!!!";
            Proof_of_Concept = "Conducted at " + DateTime.Now.ToString() + " -As you can see, making the following will result with DLL Hijack attack.\r\n" + pocs_and_pic;
            create_word_report(Technical_Details, Issues, Refrences, Recommendations_for_mitigation, Proof_of_Concept, i_Pics, "2.DLL Hijacking", "2");
            ResetArguments();
        }

        public static void Scan_for_resource_hijacking_files()
        {
            Technical_Details = @"In this test we search for files on all the disk that can be abuesd in order to gain privlige eascalation by examining the content of all xml,config,json,bat,cmd,ps1,vbs,ini,js,exe,dll,mui,msi,yaml,lib,inf files exists on the disk and checks if it contains paths to other xml,config,json,bat,cmd,ps1,vbs,ini,js,exe,dll,mui,msi,yaml,lib,inf files and checks if there are premonitions to edit the file, by that it can hijack the resource and gain privlige escalation.";
            Refrences = "None.";
        }

        public static void ResetArguments()
        {
            Technical_Details = string.Empty;
            Issues = string.Empty;
            Recommendations_for_mitigation = string.Empty;
            Refrences = string.Empty;
            Proof_of_Concept = string.Empty;
            apps = string.Empty;
            apps_h = string.Empty;
            apps_r = string.Empty;
            pocs_and_pic = string.Empty;
        }


        public static void Merge(string[] filesToMerge, string outputFilename, bool insertPageBreaks)
        {
            //object defaultTemplate = documentTemplate;
            object missing = System.Type.Missing;
            object pageBreak = Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage;
            object outputFile = outputFilename;

            // Create a new Word application
            Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();

            try
            {
                // Create a new file based on our template
                Microsoft.Office.Interop.Word.Document wordDocument = wordApplication.Documents.Add(
                                          ref missing
                                        , ref missing
                                        , ref missing
                                        , ref missing);

                // Make a Word selection object.
                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;

                //Count the number of documents to insert;
                int documentCount = filesToMerge.Length;

                //A counter that signals that we shoudn't insert a page break at the end of document.
                int breakStop = 0;

                // Loop thru each of the Word documents
                foreach (string file in filesToMerge)
                {
                    breakStop++;
                    // Insert the files to our template
                    selection.InsertFile(
                                                file
                                            , ref missing
                                            , ref missing
                                            , ref missing
                                            , ref missing);

                    //Do we want page breaks added after each documents?
                    if (insertPageBreaks && breakStop != documentCount)
                    {
                        selection.InsertBreak(ref pageBreak);
                    }
                }

                // Save the document to it's output file.
                wordDocument.SaveAs(
                                ref outputFile
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing);

                // Clean up!
                wordDocument = null;
            }
            catch (Exception ex)
            {
                //I didn't include a default error handler so i'm just throwing the error
                throw ex;
            }
            finally
            {
                // Finally, Close our Word application
                wordApplication.Quit(ref missing, ref missing, ref missing);
            }
        }


        public static void create_word_report(string i_Technical_Details, string i_Issues, string i_References, string i_Recommendations_for_mitigation, string i_Proof_of_Concept, List<string> i_pic, string vector, string test_number)
        {
            try
            {
                //Create an instance for word app  
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
                //Set animation status for word application  
                winword.ShowAnimation = false;
                //Set status for word application is to be visible or not.  
                winword.Visible = false;
                //Create a missing variable for missing value  
                object missing = System.Reflection.Missing.Value;
                //Create a new document  
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                //Add header into the document  
                foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
                {
                    //Get the header range and add the header details.  
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdGray25;
                    headerRange.Font.Size = 10;
                    headerRange.Text = "-Confidential-";
                }

                string style = "Normal";
                object objstyle = style;

                //Add paragraph with Heading 1 style  
                Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading1 = "Heading 1";
                para1.Range.set_Style(ref styleHeading1);
                para1.Range.Text = vector;
                para1.Range.InsertParagraphAfter();
                Microsoft.Office.Interop.Word.Range r1 = para1.Range;
                r1.set_Style(ref objstyle);

                //Add paragraph with Heading 1 style  
                Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading2 = "Heading 2";
                para2.Range.set_Style(ref styleHeading2);
                para2.Range.Text = "Overall Risk Level:";
                para2.Range.InsertParagraphAfter();
                Microsoft.Office.Interop.Word.Range r2 = para2.Range;
                r2.set_Style(ref objstyle);
                r2.Text = "";

                //Add paragraph with Heading 1 style  
                Microsoft.Office.Interop.Word.Paragraph para3 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading3 = "Heading 4";
                para3.Range.set_Style(ref styleHeading3);
                para3.Range.Text = "Technical Details:";
                para3.Range.InsertParagraphAfter();
                Microsoft.Office.Interop.Word.Range r3 = para3.Range;
                r3.set_Style(ref objstyle);
                r3.Text = i_Technical_Details;
                r3.InsertParagraphAfter();


                //Add paragraph with Heading 2 style  
                Microsoft.Office.Interop.Word.Paragraph para4 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading4 = "Heading 4";
                para4.Range.set_Style(ref styleHeading4);
                para4.Range.Text = "Issues:";
                para4.Range.InsertParagraphAfter();
                Microsoft.Office.Interop.Word.Range r4 = para4.Range;
                r4.set_Style(ref objstyle);
                r4.Text = i_Issues;
                r4.InsertParagraphAfter();


                //Add paragraph with Heading 1 style  
                Microsoft.Office.Interop.Word.Paragraph para5 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading5 = "Heading 4";
                para5.Range.set_Style(ref styleHeading5);
                para5.Range.Text = "References:";
                para5.Range.InsertParagraphAfter();
                Microsoft.Office.Interop.Word.Range r5 = para5.Range;
                r5.set_Style(ref objstyle);
                r5.Text = i_References;
                r5.InsertParagraphAfter();

                //Add paragraph with Heading 1 style  
                Microsoft.Office.Interop.Word.Paragraph para6 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading6 = "Heading 4";
                para6.Range.set_Style(ref styleHeading6);
                para6.Range.Text = "Recommendations for mitigation:";
                para6.Range.InsertParagraphAfter();
                Microsoft.Office.Interop.Word.Range r6 = para6.Range;
                r6.set_Style(ref objstyle);
                r6.Text = i_Recommendations_for_mitigation;
                r6.InsertParagraphAfter();

                //Add paragraph with Heading 1 style  
                Microsoft.Office.Interop.Word.Paragraph para7 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading7 = "Heading 4";
                para7.Range.set_Style(ref styleHeading7);
                para7.Range.Text = "Proof of Concept:";
                para7.Range.InsertParagraphAfter();
                Microsoft.Office.Interop.Word.Range r7 = para7.Range;
                r7.set_Style(ref objstyle);
                r7.Text = i_Proof_of_Concept;
                r7.InsertParagraphAfter();

                for (int i = i_pic.Count - 1; 0 <= i; i--)
                {
                    string fileName = i_pic[i];  //the picture file to be inserted
                    Object oMissed = para5.Range; //the position you want to insert
                    Object oLinkToFile = false;  //default
                    Object oSaveWithDocument = true;//default
                    document.InlineShapes.AddPicture(fileName, ref oLinkToFile, ref oSaveWithDocument, ref oMissed);
                }
                //Save the document  
                object filename = Path.GetDirectoryName(Path.GetFullPath(Process.GetCurrentProcess().ProcessName + ".exe")) + @"\TEMP\"+ Process.GetCurrentProcess().ProcessName + "-" + test_number + "-Report.docx";
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
            }
            catch (Exception ex)
            { }
        }


    }
}
