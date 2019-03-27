module ContextChat.Droid.Helper

open Android.Util

let logd tag msg = 
    Log.Debug(tag, msg) |> ignore
