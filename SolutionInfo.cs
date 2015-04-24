using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyTrademark("mkoertgen")]
[assembly: AssemblyProduct("HelloSquirrel")]

[assembly: AssemblyDescription(@"© mkoertgen 2015")]
[assembly: AssemblyCompany("mkoertgen")]
[assembly: AssemblyCopyright("Copyright © mkoertgen 2015")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.MainAssembly)]
