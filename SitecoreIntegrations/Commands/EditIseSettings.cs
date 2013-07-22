﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using Cognifide.PowerShell.PowerShellIntegrations;
using Cognifide.PowerShell.PowerShellIntegrations.Settings;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using Sitecore.Diagnostics;
using Sitecore.Form.UI.Controls;
using Sitecore.Install.Utils;
using Sitecore.Shell.Applications.Layouts.IDE.Editors.Xslt;
using Sitecore.Shell.Applications.WebEdit;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace Cognifide.PowerShell.SitecoreIntegrations.Commands
{
    [Serializable]
    public class EditIseSettings : ExecuteFieldEditor
    {
        protected const string AppNameParameter = "appName";
        protected const string PersonalParameter = "personal";

        protected bool Personalized { get; set; }
        protected string AppName { get; set; }

        public override CommandState QueryState(CommandContext context)
        {
            return CommandState.Enabled;
        }

        public override bool CanExecute(CommandContext context)
        {
            return true;
        }

        protected override void EnsureContext(ClientPipelineArgs args)
        {
/*
            AppName = args.Parameters[AppNameParameter];
            Personalized = args.Parameters[PersonalParameter] == "1";
*/

            var settingsPath = ApplicationSettings.GetSettingsPath(AppName, Personalized);
            CurrentItem = Factory.GetDatabase(ApplicationSettings.SettingsDb).GetItem(settingsPath);

            Assert.IsNotNull(CurrentItem, CurrentItemIsNull);
            SettingsItem = Client.CoreDatabase.GetItem(args.Parameters[ButtonParameter]);
            Assert.IsNotNull(SettingsItem, SettingsItemIsNull);
        }

        public override void Execute(CommandContext context)
        {
            AppName = context.Parameters[AppNameParameter];
            Personalized = context.Parameters[PersonalParameter] == "1";

            Assert.ArgumentNotNull(context, "context");

            var settingsPath = ApplicationSettings.GetSettingsPath(AppName, Personalized);
            CurrentItem = Factory.GetDatabase(ApplicationSettings.SettingsDb).GetItem(settingsPath);
            if (CurrentItem == null)
            {
                var settings = ApplicationSettings.GetInstance(AppName, Personalized);
                settings.Save();
                CurrentItem = Factory.GetDatabase(ApplicationSettings.SettingsDb).GetItem(settingsPath);
            }

            Context.ClientPage.Start(this, "StartFieldEditor", new ClientPipelineArgs(context.Parameters)
            {
                Parameters =
                        {
                            {"uri", CurrentItem.Uri.ToString()}
                        }
            });
        }
 
        protected override void StartFieldEditor(ClientPipelineArgs args)
        {
            base.StartFieldEditor(args);
            ApplicationSettings.ReloadInstance(AppName, Personalized);
        }
    }
}