namespace ContextChat.Droid

open System

open Android.App
open Android.OS
open Android.Content
open Android.Content.PM

open ContextChat.ServerClients.Auth


[<Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)>]
[<IntentFilter(
    [| Intent.ActionView |],
    Categories = [| Intent.CategoryDefault; Intent.CategoryBrowsable |],
    DataSchemes = [| "com.googleusercontent.apps.597529832691-aocctpi2ig5m85kfmj2gnh49iuuc7o4q" |],
    DataPath = "/oauth2redirect") 
    >]   
type OAuth2InterceptorActivity () =
     inherit Activity()

     override x.OnCreate(b :Bundle) =
         base.OnCreate(b)

         let uri = new System.Uri(x.Intent.DataString)
         GoogleOAuth2.authenticator.OnPageLoading(uri)

         x.Finish() 