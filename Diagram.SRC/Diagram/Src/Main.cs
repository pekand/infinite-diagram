﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Security;

#if !MONO
using Microsoft.Win32;
#endif

namespace Diagram
{
    /// <summary>    
    /// Processing global options files
    /// Start server
    /// Processing command line arguments.
    /// Create mainform
    /// </summary>
    public class Main //UID7118462915
    {

        /*************************************************************************************************************************/
        // OPTONS

        /// <summary>
        /// Global program options</summary>
        public ProgramOptions options = null;

        /// <summary>
        /// managing file with global program options</summary>
        private OptionsFile optionsFile = null;

        /// <summary>
        /// keyboard shorcut mapping</summary>
        public KeyMap keyMap = null;

        /// <summary>
        /// open directori with global configuration</summary>
        public void OpenConfigDir()
        {
            this.optionsFile.ShowDirectoryWithConfiguration();
        }

        /*************************************************************************************************************************/
        // Plugins

        /// <summary>
        /// load plugins</summary>
        public string pluginsDirectoryName = "plugins";
        public Plugins plugins = null;

        /*************************************************************************************************************************/
        // SERVER

        /// <summary>
        /// local messsaging server for communication between running program instances</summary>
        private Server server = null;


        /*************************************************************************************************************************/
        // MAIN APPLICATION

        // command line arguments
        private string[] args = null;

        /// <summary>
        /// form for catching messages from local server</summary>
        public MainForm mainform = null;

        /// <summary>
        /// parse command line arguments and open forms</summary>
        public Main() //UID8239288102
        {
            // inicialize program UID3013916734
            options = new ProgramOptions();
            optionsFile = new OptionsFile(options);

            // load external plugins UID9841812564
            plugins = new Plugins();

            // executable location directory 
            string pluginsLocalDirectory = Os.Combine(Os.GetCurrentApplicationDirectory(), this.pluginsDirectoryName);
            if (Os.DirectoryExists(pluginsLocalDirectory))
            {
                plugins.LoadPlugins(pluginsLocalDirectory);
            }
            
#if !DEBUG
            string pluginsGlobalDirectory = Os.Combine(optionsFile.GetGlobalConfigDirectory(), this.pluginsDirectoryName);
            if (Os.DirectoryExists(pluginsGlobalDirectory))
            {
                plugins.LoadPlugins(pluginsGlobalDirectory);
            }
#endif

            // create local server for comunication between local instances UID2964640610
            server = new Server(this);

            if(server.StartServer())
            {
                Update.UpdateApplication(this);
            }

            Program.log.Write("Program: Main");

#if DEBUG
            Program.log.Write("program: debug mode");
#else
			Program.log.Write("program: release mode");
#endif

            // TODO: Load global options file and save it when application is closed
            // load options
            // options.loadOptions();


            // get command line arguments
            this.args = Environment.GetCommandLineArgs();
#if DEBUG
            // custom debug arguments
            if (this.args.Length == 1 && Os.FileExists("test.diagram")) // if not argument is added from system ad some testing arguments
            {
                // comand line arguments testing
                this.args = new string[] {
                    System.Reflection.Assembly.GetExecutingAssembly().Location
                    ,"test.diagram"
                };
            }
#endif
            // process comand line arguments
            this.ParseCommandLineArguments(this.args);

#if !MONO
            // sleep or hibernate event UID7641650028
            SystemEvents.PowerModeChanged += OnPowerChange;
#endif

            // check if this program instance created server (is main application)
            // or if running debug console from command line parameter
            if (server.mainProcess || this.console != null)
            {
                this.mainform = new MainForm(this);
            }
        }

        /// <summary>
        /// process comand line arguments</summary>
        public void ParseCommandLineArguments(string[] args) // [PARSE] [COMMAND LINE] UID5172911205
        {

            // options - create new file with given name if not exist
            bool CommandLineCreateIfNotExistFile = false;

            bool ShowCommandLineHelp = false;
            bool ShowDebugConsole = false;

            // list of diagram files names for open
            List<String> CommandLineOpen = new List<String>();

            String arg = "";
            for (int i = 0; i < args.Count(); i++)
            {

                //skip application name
                if (i == 0)
                {
                    continue;
                }

                // current processing argument
                arg = args[i];

                // [COMAND LINE] [CREATE]  oprions create new file with given name if not exist
                if (arg == "-h" || arg == "--help" || arg == "/?")
                {
                    ShowCommandLineHelp = true;
                    break;
                    
                }
                if (arg == "-c" || arg == "--console")
                {
                    ShowDebugConsole = true;
                    break;
                } 
                
                if(arg == "-e")
                {
                    CommandLineCreateIfNotExistFile = true;
                    break;
                }
                
                // [COMAND LINE] [OPEN] check if argument is diagram file
                if (Os.GetExtension(arg).ToLower() == ".diagram")
                {
                    CommandLineOpen.Add(arg);
                    break;
                }
                
                Program.log.Write("bed commmand line argument: " + arg);
            }

            if (ShowDebugConsole) {
                this.ShowConsole();
            }

            // open diagram given as arguments
            if (ShowCommandLineHelp)
            {
                String help =
                "diagram -h --help /?  >> show this help\n" +
                "diagram -c --console /?  >> show debug console\n" +
                "diagram -e {filename} >> create file if not exist\n" +
                "diagram {filepath} {filepath} >> open existing file\n";
                MessageBox.Show(help, "Command line parameters");
                return;
            }

            if (CommandLineOpen.Count == 0)
            {
                //open empty diagram
                this.OpenDiagram();
                return;
            }
            
            for (int i = 0; i < CommandLineOpen.Count(); i++)
            {
                string file = CommandLineOpen[i];

                // tray create diagram file if command line option is set
                if (CommandLineCreateIfNotExistFile && !Os.FileExists(file))
                {
                    try
                    {
                        Os.CreateEmptyFile(file);
                    }
                    catch (Exception ex)
                    {
                        Program.log.Write("create empty diagram file error: " + ex.Message);
                    }
                }

                if (Os.FileExists(file))
                {
                    this.OpenDiagram(file);
                }
            }

            // cose application if is not diagram model opened
            this.CloseEmptyApplication();
        }

        /// <summary>
        /// close application if not diagram view or node edit form is open </summary>
        public void CloseEmptyApplication() //UID0787891060
        {
            Program.log.Write("Program : CloseApplication");

            bool canclose = true;

            if (Diagrams.Count > 0 || DiagramViews.Count > 0 || TextWindows.Count > 0)
            {
                canclose = false;
            }

            if (console != null)
            {
                // prevent close application if debug console is open
                // console must by closed mannualy by user
                Program.log.Write("Program : Console is still open...");
                canclose = false;
            }

            if (canclose)
            {
                ExitApplication();
            }
        }

        /// <summary>
        /// force close application</summary>
        public void ExitApplication() //UID0090378181
        {
            Program.log.Write("Program : ExitApplication");

            if (mainform != null)
            {
                mainform.Close();
            }

            if (passwordForm != null)
            {
                passwordForm.Close();
            }

            if (newPasswordForm != null)
            {
                newPasswordForm.Close();
            }

            if (changePasswordForm != null)
            {
                changePasswordForm.Close();
            }

            if (console != null)
            {
                console.Close();
            }

            if (this.server != null && this.server.mainProcess) {
                server.RequestStop();
            }

            this.optionsFile.SaveConfigFile();
            TerminateApplication();
        }

        public void TerminateApplication()
        {
            try
            {
                Application.Exit();
                Application.ExitThread();
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Program.log.Write("Terminate aplication exception: " + e.Message);
                Program.log.Write("Terminate aplication trace: " + e.StackTrace);
            }
        }

        /*************************************************************************************************************************/
        // DIAGRAMS

        /// <summary>
        /// all opened diagrams models</summary>
        private List<Diagram> Diagrams = new List<Diagram>();

        /// <summary>
        /// add diagram to list of all diagrams</summary>
        public void AddDiagram(Diagram diagram)
        {
            this.Diagrams.Add(diagram);
        }

        /// <summary>
        /// remove diagram from list of all diagrams</summary>
        public void RemoveDiagram(Diagram diagram)
        {
            this.Diagrams.Remove(diagram);
        }

        /// <summary>
        /// open existing diagram or create new empty diagram
        /// Create diagram model and then open diagram view on this model</summary>
        public void OpenDiagram(String FilePath = "") //UID1771511767
        {
            Program.log.Write("Program : OpenDiagram: " + FilePath);

            if (passwordForm != null) // prevent open diagram if another diagram triing open 
            {
                return;
            }
            
            // open new empty diagram
            if (FilePath == "" && !server.mainProcess)
            {
                // if server already exist in system, send him message whitch open empty diagram
                server.SendMessage("open:");
                return;
            }
            
            // open diagram in current program instance
            if (FilePath == "" && server.mainProcess)
            {
                // create new model
                Diagram emptyDiagram = new Diagram(this);
                Diagrams.Add(emptyDiagram);
                // open diagram view on diagram model
                emptyDiagram.OpenDiagramView();
                return;
            }
              
            // open existing diagram file
            
            if (!Os.FileExists(FilePath))
            {
                return;
            }
            
            FilePath = Os.NormalizedFullPath(FilePath);
            
            // if server already exist in system, send him message whitch open diagram file
            if (!server.mainProcess)
            {
                FilePath = Os.GetFullPath(FilePath);
                server.SendMessage("open:" + FilePath);
                return;
            }  

            // open diagram in current program instance

            // check if file is already opened in current instance
            bool alreadyOpen = false;
            foreach (Diagram openedDiagram in Diagrams)
            {
                if (openedDiagram.FileName != FilePath)
                {
                    continue;
                }

                Program.log.Write("window get focus");
                Program.log.Write("OpenDiagram: diagramView: setFocus");

                if (openedDiagram.DiagramViews.Count() != 0 && !openedDiagram.DiagramViews[0].Visible)
                {
                    openedDiagram.DiagramViews[0].Show();
                }

                Program.log.Write("bring focus");
                Media.BringToFront(openedDiagram.DiagramViews[0]); //UID4510272262
                    
                alreadyOpen = true;
                break;
            }

            if (alreadyOpen)
            {
                return;
            }
            
            Diagram diagram = new Diagram(this);
            lock (diagram)
            {
                // create new model
                if (!diagram.OpenFile(FilePath))
                {
                    return;
                }
                
                this.options.AddRecentFile(FilePath);
                Diagrams.Add(diagram);
                // open diagram view on diagram model
                DiagramView newDiagram = diagram.OpenDiagramView();

                this.plugins.OpenDiagramAction(diagram); //UID0290845816

                Program.log.Write("bring focus");
                Media.BringToFront(newDiagram); //UID4510272263
            }
        }

        /*************************************************************************************************************************/
        // VIEWS

        /// <summary>
        /// all opened form views to diagrams models</summary>
        private List<DiagramView> DiagramViews = new List<DiagramView>();

        /// <summary>
        /// add diagram view to list of all views</summary>
        public void AddDiagramView(DiagramView view)
        {
            this.DiagramViews.Add(view);
        }

        /// <summary>
        /// remove diagram view from list of all diagram views</summary>
        public void RemoveDiagramView(DiagramView view)
        {
            this.DiagramViews.Remove(view);
        }

        /// <summary>
        /// hide diagram views except diagramView</summary>
        public void SwitchViews(DiagramView diagramView = null) //UID9164062779
        {
            bool someIsHidden = false;
            foreach (DiagramView view in DiagramViews)
            {
                if (!view.Visible)
                {
                    someIsHidden = true;
                    break;
                }
            }

            if (someIsHidden)
            {
                ShowAllViews();
            }
            else
            {
                HideViews(diagramView);
            }
        }

        /// <summary>
        /// show views if last visible view is closed</summary>
        public void ShowIfIsLastViews(DiagramView diagramView = null) //UID3969703093
        {
            foreach (DiagramView view in DiagramViews)
            {
                if (!view.Visible && diagramView != view)
                {
                    view.Show();
                    break;
                }
            }
        }

        /// <summary>
        /// show diagram views</summary>
        public void ShowAllViews() //UID5230996149
        {
            foreach (DiagramView view in DiagramViews)
            {
                if (!view.Visible)
                {
                    view.Show();
                }
            }
        }

        /// <summary>
        /// hide diagram views</summary>
        public void HideViews(DiagramView diagramView = null) //UID3131107610
        {
            foreach (DiagramView view in DiagramViews)
            {
                if (view != diagramView) {
                    view.Hide();
                }
            }
        }

        /*************************************************************************************************************************/
        // DIAGRAM EDIT FORMS

        /// <summary>
        /// all opened node edit forms for all diagrams models</summary>
        private List<TextForm> TextWindows = new List<TextForm>();

        /// <summary>
        /// add text form to list of all text forms</summary>
        public void AddTextWindow(TextForm textWindows)
        {
            this.TextWindows.Add(textWindows);
        }

        /// <summary>
        /// remove text form from list of all text forms</summary>
        public void RemoveTextWindow(TextForm textWindows)
        {
            this.TextWindows.Remove(textWindows);
        }

        /*************************************************************************************************************************/
        // PASSWORD FORMS

        /// <summary>
        ///input form for password</summary>
        private PasswordForm passwordForm = null;

        /// <summary>
        /// input form for new password</summary>
        private NewPasswordForm newPasswordForm = null;

        /// <summary>
        /// input form for change old password</summary>
        private ChangePasswordForm changePasswordForm = null;

        /// <summary>
        /// show dialog for password for diagram unlock</summary>
        public string GetPassword(string subtitle = "")
        {
            string password = null;

            if (this.passwordForm == null)
            {
                this.passwordForm = new PasswordForm(this);
            }

            this.passwordForm.Text = Translations.password + " - " + subtitle;
            this.passwordForm.Clear();
            this.passwordForm.ShowDialog();
            if (!this.passwordForm.cancled)
            {
                password = this.passwordForm.GetPassword();
                this.passwordForm.Clear();
            }

            this.passwordForm = null;

            return password;
        }

        /// <summary>
        /// show dialog for new password for diagram</summary>
        public string GetNewPassword()
        {
            string password = null;

            if (this.newPasswordForm == null)
            {
                this.newPasswordForm = new NewPasswordForm(this);
            }

            this.newPasswordForm.Clear();
            this.newPasswordForm.ShowDialog();
            if (!this.newPasswordForm.cancled)
            {
                password = this.newPasswordForm.GetPassword();
                this.newPasswordForm.Clear();
            }

            this.newPasswordForm = null;

            return password;
        }

        /// <summary>
        /// show dialog for change password for diagram</summary>
        public string ChangePassword(SecureString currentPassword)
        {
            string password = null;

            if (this.changePasswordForm == null)
            {
                this.changePasswordForm = new ChangePasswordForm(this);
            }

            this.changePasswordForm.Clear();
            this.changePasswordForm.oldpassword = currentPassword;
            this.changePasswordForm.ShowDialog();
            if (!this.changePasswordForm.cancled)
            {
                password = this.changePasswordForm.GetPassword();
                this.changePasswordForm.Clear();
            }

            this.changePasswordForm = null;

            return password;
        }

        /*************************************************************************************************************************/
        // LOCK DIAGRAM
#if !MONO

        /// <summary>
        /// lock encrypted diagrams if computer go to sleep or hibernation</summary>
        private void OnPowerChange(object s, PowerModeChangedEventArgs e) // UID1864495676
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    break;
                case PowerModes.Suspend:
                    this.LockDiagrams();
                    break;
            }
        }

        /// <summary>
        /// forgot password if diagram is encrypted</summary>
        public void LockDiagrams() //UID6105963009
        {
            foreach (Diagram diagram in Diagrams)
            {
                diagram.LockDiagram();
            }
        }

        /// <summary>
        /// prompt for password if diagram is encrypted</summary>
        public void UnlockDiagrams()
        {
            foreach (Diagram diagram in Diagrams)
            {
                diagram.UnlockDiagram();
            }
        }
#endif
        /*************************************************************************************************************************/
        // ABOUT FORM        

        /// <summary>
        /// about form for display basic informations about application</summary>
        private AboutForm aboutForm = null;

        /// <summary>
        /// show about</summary>
        public void ShowAbout()
        {
            if (this.aboutForm == null)
            {
                this.aboutForm = new AboutForm(this);
            }

            this.aboutForm.ShowDialog();

            this.aboutForm = null;
        }

        /*************************************************************************************************************************/
        // CONSOLE

        /// <summary>
        /// form for display logged messages</summary>
        private Console console = null;

        /// <summary>
        /// show error console</summary>
        public void ShowConsole()
        {
            if (this.console == null)
            {
                this.console = new Console(this);
                this.console.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseConsole);
                Program.log.SetConsole(this.console);
            }

            this.console.Show();
        }

        /// <summary>
        /// clean after error console close</summary>
        private void CloseConsole(object sender, FormClosedEventArgs e)
        {
            Program.log.SetConsole(null);
            this.console = null;
            this.CloseEmptyApplication();
        }

    }
}
