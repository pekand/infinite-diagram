﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

#if !MONO
using Shell32;
#endif

namespace Diagram
{
    class Os
    {
#if !MONO
        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        public static string GetShortcutIcon(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                string iconlocation = "";
                link.GetIconLocation(out iconlocation);
                return iconlocation;
            }

            return string.Empty;
        }
#endif

        public static void openPathInSystem(String path)
        {
            if (File.Exists(path))       // OPEN FILE
            {
                try
                {
                    string parent_diectory = new FileInfo(path).Directory.FullName;
                    System.Diagnostics.Process.Start(parent_diectory);
                }
                catch (Exception ex) { Program.log.write(ex.Message); }
            }
            else if (Directory.Exists(path))  // OPEN DIRECTORY
            {
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch (Exception ex) { Program.log.write(ex.Message); }
            }
        }

        public static bool isDiagram(String diagramPath)
        {
            if (File.Exists(diagramPath) && Path.GetExtension(diagramPath).ToLower() == ".diagram")
            {
                return true;
            }

            return false;
        }

        public static String escape(String text)
        {
            return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        public static void openDiagram(String diagramPath)
        {
            try
            {

                String currentApp = System.Reflection.Assembly.GetExecutingAssembly().Location;
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = currentApp;
                startInfo.Arguments = "\"" + escape(diagramPath) + "\"";
                Program.log.write("diagram: openlink: open diagram: " + currentApp + "\"" + escape(diagramPath) + "\"");
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Program.log.write(ex.Message);
            }
        }

        public static String getFileDirectory(String FileName)
        {
            if (FileName.Trim().Length > 0 && File.Exists(FileName))
            {
                return new FileInfo(FileName).Directory.FullName;
            }
            return null;
        }

        public static void runCommand(String cmd, string workdir = null)
        {
            try
            {

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if !MONO
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c " + cmd;
#else
				startInfo.FileName = "/bin/bash";
                startInfo.Arguments = "-c " + cmd;
#endif
                startInfo.WorkingDirectory = workdir;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                Program.log.write("output: " + output);
                Program.log.write("error: " + error);
            }
            catch (Exception ex)
            {
                Program.log.write("exception: " + ex.Message);
            }
        }

        public static String getTextFormClipboard()
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();
            String clipboard = "";
            if (retrievedData != null && retrievedData.GetDataPresent(DataFormats.Text))  // [PASTE] [TEXT] insert text
            {
                clipboard = retrievedData.GetData(DataFormats.Text) as string;
            }

            return clipboard;
        }

        public static void runProcess(String cmd)
        {
            System.Diagnostics.Process.Start(cmd);
        }
    }
}
