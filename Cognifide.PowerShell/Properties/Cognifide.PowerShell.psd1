#######################################################################################################################
# File:             Cognifide.PowerShell.psd1                                                                         #
# Author:           Adam Najmanowicz, Michael West                                                                    #
# Publisher:        Cognifide Limited                                                                                 #
# Copyright:        © 2010-2017 Adam Najmanowicz, Michael West. All rights reserved.                                  #
# Usage:            To load this module in your Script Editor:                                                        #
#                   1. Open the Script Editor.                                                                        #
#                   2. Select "PowerShell Libraries" from the File menu.                                              #
#                   3. Check the Cognifide PowerShell Console for Sitecore module.                                    #
#                   4. Click on OK to close the "PowerShell Libraries" dialog.                                        #
#                   Alternatively you can load the module from the embedded console by invoking this:                 #
#                       Import-Module -Name \Cognifide PowerShell Console for Sitecore                                #
#                   Please provide feedback on the PowerGUI Forums.                                                   #
#######################################################################################################################

<#

	Module manifest for module 'MyModule'

	Created by: PowerGUI

	NOTE: This file should be saved with the same name as your module and a psd1
	file extension.

	For more information, see:
		Get-Help New-ModuleManifest

	Or for a complete description of the format, effects, and requirements of a
	module manifest, see the following documents in the MSDN (Microsoft Developer
	Network) library:
	
		How to Write a Module Manifest
		http://msdn.microsoft.com/en-us/library/dd878297(VS.85).aspx

		Module Manifest Example
		http://msdn.microsoft.com/en-us/library/dd878317(VS.85).aspx

#>

@{

# Script module or binary module file associated with this manifest
ModuleToProcess = 'Cognifide.Powershell.dll'

# Version number of this module.
ModuleVersion = '2.0.0.0'

# ID used to uniquely identify this module
GUID = '{32739bb7-7f92-443f-abf8-ce7828fc998b}'

# Author of this module
Author = 'Adam Najmanowicz, Michael West'

# Company or vendor of this module
CompanyName = 'Cognifide Limited'

# Copyright statement for this module
Copyright = '© 2010-2017 Adam Najmanowicz, Michael West. All rights reserved.'

# Description of the functionality provided by this module
Description = 'PowerShell scripting environment for Sitecore CMS.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '2.0'

<#
# Name of the Windows PowerShell host required by this module
PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
PowerShellHostVersion = ''
#>

# Minimum version of the .NET Framework required by this module
DotNetFrameworkVersion = '2.0'

# Minimum version of the common language runtime (CLR) required by this module
CLRVersion = '3.5.21022.8'

# Processor architecture (None, X86, Amd64, IA64) required by this module
ProcessorArchitecture = 'None'

# Modules that must be imported into the global environment prior to importing
# this module
RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @()

# Script files (.ps1) that are run in the caller's environment prior to
# importing this module
ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
FormatsToProcess = @()

# Modules to import as nested modules of the module specified in
# ModuleToProcess
NestedModules = @()

# Functions to export from this module
FunctionsToExport = '*'

# Cmdlets to export from this module
CmdletsToExport = '*'

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module
AliasesToExport = '*'

# List of all modules packaged with this module
ModuleList = @()

# List of all files packaged with this module
FileList = @(
	'.\Cognifide PowerShell Console for Sitecore.psd1'
	'.\Cognifide.PowerShell.dll'
)

# Private data to pass to the module specified in ModuleToProcess
PrivateData = @{}

}