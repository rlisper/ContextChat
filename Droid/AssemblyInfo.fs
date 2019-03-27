namespace ContextChat.Droid
open System.Reflection
open System.Runtime.CompilerServices

// the name of the type here needs to match the name inside the ResourceDesigner attribute
type Resources = ContextChat.Droid.Resource
[<assembly: Android.Runtime.ResourceDesigner("ContextChat.Droid.Resources", IsApplication=true)>]

[<assembly: AssemblyTitle("ContextChat.Droid")>]
[<assembly: AssemblyDescription("")>]
[<assembly: AssemblyConfiguration("")>]
[<assembly: AssemblyCompany("")>]
[<assembly: AssemblyProduct("")>]
[<assembly: AssemblyCopyright("${AuthorCopyright}")>]
[<assembly: AssemblyTrademark("")>]

// The assembly version has the format {Major}.{Minor}.{Build}.{Revision}

[<assembly: AssemblyVersion("1.0.0.0")>]

//[<assembly: AssemblyDelaySign(false)>]
//[<assembly: AssemblyKeyFile("")>]

()
