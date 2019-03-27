module ContextChat.Geo

open System
open System.Diagnostics
open Plugin.Geolocator

type Location (altitude, longitude) =
    (*static member current =
        let locator = Plugin.Geolocator.CrossGeolocator.Current
        locator.GetPositionAsync(Nullable(TimeSpan.FromSeconds(10.0)))*)
    member x.altitude: float = altitude
    member x.longitude: float = longitude
    override x.ToString() = "altitude: " + x.altitude.ToString() + "; longitude: " + x.longitude.ToString()