#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="SolutionAssemblyInfo.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

#region using...
using System.Reflection;
using System.Resources;

#endregion

// Shared assembly info that is common for all assemblies of this project
////[assembly: AssemblyTitle("DEFINED IN ACTUAL ASSEMBLYINFO")]
////[assembly: AssemblyDescription("DEFINED IN ACTUAL ASSEMBLYINFO")]

[assembly: AssemblyProduct("Tank Levels")]
[assembly: AssemblyCompany("Orc Simulation")]
[assembly: AssemblyCopyright("Copyright (c) Orc Simulation 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]

#if DEBUG

#if NET35
[assembly: AssemblyConfiguration("NET35, Debug")]
#elif NET40

[assembly: AssemblyConfiguration("NET40, Debug")]
#elif NET45
[assembly: AssemblyConfiguration("NET45, Debug")]
#elif NET50
[assembly: AssemblyConfiguration("NET45, Debug")]
#elif SL4
[assembly: AssemblyConfiguration("Silverlight 4, Debug")]
#elif SL5
[assembly: AssemblyConfiguration("Silverlight 5, Debug")]
#elif WINDOWS_PHONE
[assembly: AssemblyConfiguration("Windows Phone 7, Debug")]
#elif NETFX_CORE
[assembly: AssemblyConfiguration("WinRT, Debug")]
#else

[assembly: AssemblyConfiguration("Unknown, Debug")]
#endif

#else

#if NET35
[assembly: AssemblyConfiguration("NET35, Release")]
#elif NET40
[assembly: AssemblyConfiguration("NET40, Release")]
#elif NET45
[assembly: AssemblyConfiguration("NET45, Release")]
#elif NET50
[assembly: AssemblyConfiguration("NET45, Release")]
#elif SL4
[assembly: AssemblyConfiguration("Silverlight 4, Release")]
#elif SL5
[assembly: AssemblyConfiguration("Silverlight 5, Release")]
#elif WINDOWS_PHONE
[assembly: AssemblyConfiguration("Windows Phone 7, Release")]
#elif NETFX_CORE
[assembly: AssemblyConfiguration("WinRT, Release")]
#else
[assembly: AssemblyConfiguration("Unknown, Release")]
#endif

#endif

// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// Note: This version numbers are set by build process (based on version.txt) if using FAKE build script

[assembly: AssemblyVersion("0.9.0.0")]
[assembly: AssemblyFileVersion("0.9.0.0")]
[assembly: AssemblyInformationalVersion("0.9.0.beta")]