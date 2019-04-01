namespace ContextChat.Droid

open Helper
open ContextChat
open ContextChat.ServerClients.Auth

open System.Threading.Tasks

open Android.App
open Android.OS
open Android.Content
open Android.Content.PM
open Android.Support.CustomTabs

open MvvmCross
open MvvmCross.Platforms.Android.Core
open MvvmCross.Platforms.Android.Views

open Microsoft.WindowsAzure.MobileServices

open Xamarin.Auth


[<Activity(Label = "Home", MainLauncher = true, NoHistory = true, LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait)>]
type HomeScreen () =
    inherit MvxSplashScreenActivity<Setup, ContextChat.App>(Resources.Layout.HomeScreen)

    member val user :MobileServiceUser option = None with get, set

    interface IAuthenticate with 
        member x.authenticate() =
            let success = ref false
            let message = ref ""
            let loginAsync =
                ServerClients.Azure.ApiClient.ApiClient.mobileServiceClient.LoginAsync(
                    x
                    , MobileServiceAuthenticationProvider.Google
                    , "goafter")
                |> Async.AwaitTask

            async {
                let! user = loginAsync
                match user with
                | null -> ()
                | user ->
                    success := true; x.user <- Some(user)
                return !success
            } 


    override x.OnCreate(b :Bundle) =
        base.OnCreate(b)

        CurrentPlatform.Init()
        App.Init(Some(x :> IAuthenticate))
        Xamarin.Essentials.Platform.Init(x, b)

        Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(x, b)
        match MySecureStorage.getAccount(Constants.GoogleAccountService) |> Async.RunSynchronously with
        | None ->
            let presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter()
            presenter.Login(GoogleOAuth2.authenticator)
        | Some(account) -> ()
        
        //let intent = ServerClients.Auth.OAuth2.googleAuth.GetUI(x)
        //x.StartActivity(intent)

    override x.OnResume() =
        base.OnResume()


    override x.OnRequestPermissionsResult(requestCode, permissions, grantResults) =
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults)
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults)
