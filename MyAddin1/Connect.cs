using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using CodeWithMe;
using MyAddin1.Properties;

namespace MyAddin1
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;
            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;
                string toolsMenuName = "Tools";

                //Place the command on the tools menu.
                //Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
                CommandBar menuBarCommandBar = ((CommandBars)_applicationObject.CommandBars)["MenuBar"];

                //Find the Tools command bar on the MenuBar command bar:
                CommandBarControl toolsControl = menuBarCommandBar.Controls[toolsMenuName];
                CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;

                CommandBarControl myMenu;

                //try
                //{
                //    // Get the menu bar if it already exists
                //    myMenu = menuBarCommandBar.Controls["My Menu"];
                //}
                //catch (Exception)
                //{
                //    // Doesnt exist so crate a new one.
                //    myMenu = menuBarCommandBar.Controls.Add(Type: MsoControlType.msoControlPopup, Id: 1234567890, Before: toolsControl.Index - 1);
                    
                //    myMenu.Caption = "My Menu";
                //}
                //myMenu.Enabled = true;

                //This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Add-in,
                //  just make sure you also update the QueryStatus/Exec method to include the new command names.
                try
                {
                    //Add a command to the Commands collection:
                    Command command = commands.AddNamedCommand2(_addInInstance, "MyAddin1", "CodeWithMe", "Executes the command for MyAddin1", true, 59, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStyleText, vsCommandControlType.vsCommandControlTypeButton);

                    //Add a control for the command to the tools menu:
                    if ((command != null) && (toolsPopup != null))
                    {
                        command.AddControl(toolsPopup.CommandBar, 1);
                        //command.AddControl((CommandBarPopup)myMenu, 0);
                    }
                }
                catch (System.ArgumentException)
                {
                    //If we are here, then the exception is probably because a command with that name
                    //  already exists. If so there is no need to recreate the command and we can 
                    //  safely ignore the exception.
                }

                //object[] contextGUIDS = new object[] { };
                //Commands2 commands = (Commands2)_applicationObject.Commands;
                //Microsoft.VisualStudio.CommandBars.CommandBar standardToolBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["Project"];
                //try
                //{
                //    Command command = commands.AddNamedCommand2(_addInInstance,
                //                                                "MyAddin1", 
                //                                                "CodeWithMe", 
                //                                                "Executes the command for My Addin",
                //                                                true, 59, ref contextGUIDS,
                //                                                (int)vsCommandStatus.vsCommandStatusSupported +
                //                                                (int)vsCommandStatus.vsCommandStatusEnabled,
                //                                                (int)vsCommandStyle.vsCommandStylePictAndText,
                //                                                vsCommandControlType.vsCommandControlTypeButton);
                //    if ((command != null) && (standardToolBar != null))
                //    {
                //        CommandBarControl ctrl = (CommandBarControl)command.AddControl(standardToolBar, 1);
                //        ctrl.TooltipText = "Executes the command for MyAddin";
                //    }
                //}
                //catch (System.ArgumentException)
                //{
                //}

            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {

        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
            ServerIngredients data;
            //data = new ServerIngredients(_applicationObject, new UserCredentials(Settings.Default.Email, Settings.Default.Password), 5000, true, new StorageCredentials(Settings.Default.DBHost, Settings.Default.DBPort, Settings.Default.DBName, Settings.Default.DBUsername, Settings.Default.DBPassword));
            ApiHandler _apiHandler = new ApiHandler(Settings.Default.ApiUrl,
                            Settings.Default.ApiKeyPrivate,
                            Settings.Default.ApiKeyPublic);
            data = new ServerIngredients(_applicationObject, _apiHandler, Settings.Default.AutosaveInterval * 1000, Settings.Default.UseAutosave);
            Server = new CodeWithMeServer(data);
            //System.Windows.Forms.MessageBox.Show("_applicationObject: " + (_applicationObject == null).ToString());
            //System.Windows.Forms.MessageBox.Show("Server: " + (Server == null).ToString());
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
            if (Server != null)
            {
                Server.Stop();
            }
        }

        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
        /// <param term='commandName'>The name of the command to determine state for.</param>
        /// <param term='neededText'>Text that is needed for the command.</param>
        /// <param term='status'>The state of the command in the user interface.</param>
        /// <param term='commandText'>Text requested by the neededText parameter.</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (commandName == "MyAddin1.Connect.MyAddin1")
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
        /// <param term='commandName'>The name of the command to execute.</param>
        /// <param term='executeOption'>Describes how the command should be run.</param>
        /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
        /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
        /// <param term='handled'>Informs the caller if the command was handled or not.</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (commandName == "MyAddin1.Connect.MyAddin1")
                {
                    if (Server != null)
                    {
                        //System.Windows.Forms.MessageBox.Show((Server == null).ToString());
                        new FormSettings(Server).ShowDialog();
                    }
                    handled = true;
                    return;
                }
            }
        }
        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        private CodeWithMeServer _server = null;
        private CodeWithMeServer Server
        {
            get { return _server; }
            set
            {
                if (_server == null)
                {
                    _server = value;
                }
            }
        }
    }
}