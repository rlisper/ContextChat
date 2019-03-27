namespace ContextChat.Droid.Views
open ContextChat.Droid
open ContextChat.Droid.Helper

open System.Windows.Input

open MvvmCross
open MvvmCross.Platforms.Android.Core

open Android.Content
open Android.App
open Android.OS
open MvvmCross.Platforms.Android.Presenters.Attributes
open MvvmCross.Platforms.Android.Views

[<MvxActivityPresentation>]
[<Activity(Label = "View for HomeViewModel")>]
type HomeView () =
    inherit MvxActivity ()

    override x.OnCreate (bundle: Bundle) =
        base.OnCreate (bundle)       
        base.SetContentView (Resources.Layout.HomeView)

        let context = x.ApplicationContext 
        let locationIntent = new Intent(context, typeof<Services.Location.LocationService>)
        let locationService = Mvx.IoCProvider.Resolve<Services.Location.LocationService>()      
        let locationConn = new Services.Location.LocationServiceConnection(x.locationUpdateAction)
        if base.BindService(locationIntent, locationConn, Bind.AutoCreate)
        then logd "HomeView" "LocationService.BindService return TRUE"
        else logd "HomeVIew" "LocationService.BindService return FALSE"

    member x.locationUpdateAction =
        fun (location: Android.Locations.Location) ->
            let geoLocation = 
                ContextChat.Geo.Location(location.Altitude, location.Longitude)
            match x.ViewModel with
            | :? ContextChat.ViewModels.HomeViewModel as vm ->
              vm.location <- Some(geoLocation)
            | _ -> ()
            
