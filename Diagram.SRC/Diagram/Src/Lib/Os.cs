﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

#if !MONO
using Shell32;
#endif

namespace Diagram
{
    class Os
    {
#if !MONO

        /// <summary>
        /// get path from lnk file in windows  </summary>
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

        /// <summary>
        ///get icon from lnk file in windows  </summary>
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

        /// <summary>
        /// open path in system if exist  </summary>
        public static void openPathInSystem(String path)
        {
            if (File.Exists(path))       // OPEN FILE
            {
                try
                {
                    string parent_diectory = new FileInfo(path).Directory.FullName;
                    System.Diagnostics.Process.Start(parent_diectory);
                }
                catch (Exception ex) { Program.log.write("openPathInSystem open file: error:" + ex.Message); }
            }
            else if (Directory.Exists(path))  // OPEN DIRECTORY
            {
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch (Exception ex) { Program.log.write("openPathInSystem open directory: error:"+ex.Message); }
            }
        }

        /// <summary>
        /// check if diagramPath file path has good extension  </summary>
        public static bool isDiagram(String diagramPath)
        {
            diagramPath = normalizePath(diagramPath);
            if (File.Exists(diagramPath) && Path.GetExtension(diagramPath).ToLower() == ".diagram")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// open diagram file in current runing application with system call command </summary>
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

        /// <summary>
        /// get parent directory of FileName path </summary>
        public static String getFileDirectory(String FileName)
        {
            if (FileName.Trim().Length > 0 && File.Exists(FileName))
            {
                return new FileInfo(FileName).Directory.FullName;
            }
            return null;
        }

        /// <summary>
        /// run command in system and wait for output </summary>
        public static void runCommand(String cmd, string workdir = null)
        {
            try
            {

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if !MONO
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c " + "\"" + cmd + "\"";
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

        /// <summary>
        /// run command in system and discard output </summary>
        public static void runCommandAndExit(String cmd)
        {

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if MONO
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = "-c " + cmd;
#else
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + "\"" + cmd + "\"";
#endif

            process.StartInfo = startInfo;
            process.Start();
        }

        /// <summary>
        /// get string from clipboard </summary>
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

        /// <summary>
        /// run application in current os </summary>
        public static void runProcess(String path)
        {
            path = normalizePath(path);
            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// find line number with first search string occurrence </summary>
        public static int fndLineNumber(String fileName, String search)
        {
            int pos = 0;
            string line;
            using (StreamReader file = new StreamReader(fileName))
            {
                while ((line = file.ReadLine()) != null)
                {
                    pos++;
                    if (line.Contains(search))
                    {
                        return pos;
                    }
                }
            }

            return pos;
        }

        /// <summary>
        /// meke filePath relative to currentPath. 
        /// If is set inCurrentDir path is converted to relative only 
        /// if currentPath is parent of filePath</summary>
		public static string makeRelative(String filePath, String currentPath, bool inCurrentDir = true)
		{
#if !MONO
            // in windows lovercase path character for path comparison
            filePath = filePath.ToLower();
            currentPath = currentPath.ToLower();
#endif
            filePath = filePath.Trim();
			currentPath = currentPath.Trim();

			if (currentPath == "") 
			{
				return filePath;
			} 

			if (!File.Exists(filePath) && !Directory.Exists(filePath)) 
			{
				return filePath;
			}

			filePath = Path.GetFullPath(filePath);

			if (File.Exists(currentPath)) 
			{
				currentPath = Path.GetDirectoryName(currentPath);
			}

			if (!Directory.Exists(currentPath))
			{
				return filePath;
			}

			currentPath = Path.GetFullPath(currentPath);

			Uri pathUri = new Uri(filePath);
			// Folders must end in a slash
			if (!currentPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				currentPath += Path.DirectorySeparatorChar;
			}

			int pos = filePath.ToLower().IndexOf(currentPath.ToLower());
			if( inCurrentDir &&  pos != 0) // skip files outside of currentPath
			{
				return filePath;
			}

			Uri folderUri = new Uri(currentPath);
			return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
		}

        /// <summary>
        /// check if file exist independent on os </summary>
        public static bool FileExists(string path)
        {
            return File.Exists(normalizePath(path));
        }

        /// <summary>
        /// check if directory exist independent on os </summary>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(normalizePath(path));
        }

        /// <summary>
        /// check if directory or file exist independent on os </summary>
        public static bool Exists(string path)
        {
            return FileExists(path) || DirectoryExists(path);
        }

        /// <summary>
        /// get current running application executable directory </summary>
        public static string getCurrentApplicationDirectory()
        {
            String currentApp = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return Os.getFileDirectory(currentApp); 
        }

        /// <summary>
        /// add slash before slash and quote </summary>
        public static String escape(String text)
        {
            return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        /// <summary>
        /// convert slash dependent on current os </summary>
        public static string normalizePath(string path)
        {

#if MONO
            return path.Replace("\\","/");
#else
            return path.Replace("/","\\");
#endif
        }

        /// <summary>
        /// normalize path and get full path from relative path </summary>
        public static string normalizedFullPath(string path)
        {
            return Path.GetFullPath(normalizePath(path));
        }
        /// <summary>
        /// convert win path slash to linux type slash </summary>
        public static string toBackslash(string text)
        {
            return text.Replace("\\", "/");
        }

        /// <summary>
        /// get path separator dependent on os </summary>
        public static string getSeparator()
        {
            return Path.DirectorySeparatorChar.ToString();
        }

        /// <summary>
        /// get file extension</summary>
        public static string getExtension(string file)
        {
            string ext = "";
            if (file != "" && Os.FileExists(file))
            {
                ext = Path.GetExtension(file).ToLower();
            }

            return ext;
        }

        public static bool isDirectory(string path)
        {
            return Os.DirectoryExists(path);
        }

        public static bool isFile(string path)
        {
            return Os.FileExists(path);
        }

        /// <summary>
        /// Scans a folder and all of its subfolders recursively, and updates the List of files
        /// </summary>
        /// <param name="path">Full path for scened directory</param>
        /// <param name="files">out - file list</param>
        /// <param name="directories">out - directories list</param>
        public static void search(string path, List<string> files, List<string> directories)
        {
            try
            {
                foreach (string f in Directory.GetFiles(path))
                {
                    files.Add(f);
                }

                foreach (string d in Directory.GetDirectories(path))
                {
                    directories.Add(d);
                    search(d, files, directories);
                }

            }
            catch (System.Exception ex)
            {
                Program.log.write(ex.Message);
            }
        }
    }
}
