module ContextChat.Droid.Services.Location
open ContextChat.Droid.Helper

open Android.OS
open Android.App
open Android.Content
open Android.Locations
open Android.Util

[<Service>]
type LocationService () =
    inherit Service () 

    let locationManager = Application.Context.GetSystemService("location") :?> LocationManager
    let mutable _binder: IBinder = null

    let mutable _onLocationChanged = fun _ -> ()

    member x.onLocationChanged with set(closure) = _onLocationChanged <- closure

    override x.OnBind (intent: Intent): IBinder =
        logd "LocationService" "call OnBind"
        _binder <- new LocationServiceBinder (x)
        _binder

    interface ILocationListener with
        member x.OnLocationChanged (location: Location) = 
            location |> _onLocationChanged
        member x.OnProviderDisabled (provider) = 
            ()
        member x.OnProviderEnabled (provider) = 
            ()
        member x.OnStatusChanged (provider, status, extras) =
            ()

    member x.startReceiveUpdates () =
        logd "LocationService" "call startReceiveUpdates"
        let criteria = new Criteria()
        criteria.Accuracy <- Accuracy.NoRequirement
        criteria.PowerRequirement <- Power.NoRequirement
        let provider = locationManager.GetBestProvider(criteria, true)
        locationManager.RequestLocationUpdates(provider, int64(2000), float32(0), x)


and LocationServiceBinder (locationService: LocationService) =
    inherit Binder ()
    member x.service = locationService

and LocationServiceConnection (onLocationChanged) =
    inherit Java.Lang.Object ()

    interface IServiceConnection with
        member x.OnServiceConnected (name, binder) =
            logd "LocationServiceConnection" "call OnServiceConnected"
            match binder with
            | :? LocationServiceBinder as binder -> 
                binder.service.onLocationChanged <- onLocationChanged
                binder.service.startReceiveUpdates ()
            | _ -> ()

        member x.OnServiceDisconnected (name) =
            ()