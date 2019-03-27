namespace ContextChat.Droid
open ContextChat.Droid.Helper
open ContextChat.Droid.Services.Location

open Android.App
open Android.Content

open MvvmCross
open MvvmCross.Platforms.Android.Core
open MvvmCross.ViewModels

type Setup(*<'a when 'a :> IMvxApplication>*) () =
    inherit MvxAndroidSetup<ContextChat.App> () 

    override x.InitializeFirstChance() =
        logd "Setup" "call InitializeFirstChance()"
        Mvx.IoCProvider.RegisterSingleton<LocationService>(new LocationService())

    override x.CreateApp() =
        logd "Setup" "call CreateApp()"
        new ContextChat.App() :> IMvxApplication
        
       