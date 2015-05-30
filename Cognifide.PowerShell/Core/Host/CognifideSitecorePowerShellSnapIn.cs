﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Xml;
using Cognifide.PowerShell.Core.Provider;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Pipeline = Sitecore.Pipelines.Pipeline;

namespace Cognifide.PowerShell.Core.Host
{
    [RunInstaller(true)]
    public class CognifideSitecorePowerShellSnapIn : CustomPSSnapIn
    {
        private static readonly List<CmdletConfigurationEntry> commandlets = new List<CmdletConfigurationEntry>();

        /// <summary>
        ///     Specify the providers that belong to this custom PowerShell snap-in.
        /// </summary>
        private Collection<ProviderConfigurationEntry> _providers;

        private bool initialized;

        static CognifideSitecorePowerShellSnapIn()
        {
            var cmdltsToIncludes = Factory.GetConfigNodes("powershell/commandlets/add");
            foreach (XmlElement cmdltToInclude in cmdltsToIncludes)
            {
                var cmdltTypeDef = cmdltToInclude.Attributes["type"].Value.Split(',');
                var cmdletType = cmdltTypeDef[0];
                var cmdletAssembly = cmdltTypeDef[1];
                var wildcard = GetWildcardPattern(cmdletType);
                try
                {
                    var assembly = Assembly.Load(cmdletAssembly);
                    GetCommandletsFromAssembly(assembly, wildcard);
                }
                catch (Exception ex)
                {
                    if (ex is ReflectionTypeLoadException)
                    {
                        var typeLoadException = ex as ReflectionTypeLoadException;
                        var loaderExceptions = typeLoadException.LoaderExceptions;
                        var message = loaderExceptions.Aggregate(string.Empty, (current, exc) => current + exc.Message);
                        Log.Error(string.Format("Error while loading commandlets list: {0}",message), ex, typeof (CognifideSitecorePowerShellSnapIn));
                    }
                }
            }
        }

        /// <summary>
        ///     Specify the name of the PowerShell snap-in.
        /// </summary>
        public override string Name
        {
            get { return "CognifideSitecorePowerShellSnapIn"; }
        }

        /// <summary>
        ///     Specify the vendor for the PowerShell snap-in.
        /// </summary>
        public override string Vendor
        {
            get { return "Cognifide"; }
        }

        /// <summary>
        ///     Specify the localization resource information for the vendor.
        ///     Use the format: resourceBaseName,VendorName.
        /// </summary>
        public override string VendorResource
        {
            get { return "CognifideSitecorePowerShellSnapIn,Cognifide"; }
        }

        /// <summary>
        ///     Specify a description of the PowerShell snap-in.
        /// </summary>
        public override string Description
        {
            get { return "This snap-in integrates Sitecore & Powershell."; }
        }

        /// <summary>
        ///     Specify the localization resource information for the description.
        ///     Use the format: resourceBaseName,Description.
        /// </summary>
        public override string DescriptionResource
        {
            get { return "CognifideSitecorePowerShellSnapIn,This snap-in integrates Sitecore & Powershell."; }
        }

        /// <summary>
        ///     Specify the cmdlets that belong to this custom PowerShell snap-in.
        /// </summary>
        public override Collection<CmdletConfigurationEntry> Cmdlets
        {
            get { return new Collection<CmdletConfigurationEntry>(commandlets); }
        }

        public override Collection<ProviderConfigurationEntry> Providers
        {
            get
            {
                if (!initialized)
                {
                    Initialize();
                }
                if (_providers == null)
                {
                    _providers = new Collection<ProviderConfigurationEntry>
                    {
                        new ProviderConfigurationEntry("Sitecore PowerShell Provider",
                            typeof (PsSitecoreItemProvider),
                            @"..\sitecore modules\PowerShell\Assets\Cognifide.PowerShell.dll-Help.maml")
                    };
                }

                return _providers;
            }
        }

        public static List<CmdletConfigurationEntry> Commandlets
        {
            get { return commandlets; }
        }

        private static void GetCommandletsFromAssembly(Assembly assembly, WildcardPattern wildcard)
        {
            var helpPath = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath) +
                           @"\sitecore modules\PowerShell\Assets\Cognifide.PowerShell.dll-Help.maml";
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof (CmdletAttribute), true).Length > 0 &&
                    wildcard.IsMatch(type.FullName))
                {
                    var attribute = (CmdletAttribute) (type.GetCustomAttributes(typeof (CmdletAttribute), true)[0]);
                    Commandlets.Add(new CmdletConfigurationEntry(attribute.VerbName + "-" + attribute.NounName, type,
                        helpPath));
                }
            }
        }

        protected static WildcardPattern GetWildcardPattern(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                name = "*";
            }
            const WildcardOptions options = WildcardOptions.IgnoreCase | WildcardOptions.Compiled;
            var wildcard = new WildcardPattern(name, options);
            return wildcard;
        }

        private void Initialize()
        {
            initialized = true;
            Pipeline.Start("initialize", new PipelineArgs(), true);
        }
    }
}