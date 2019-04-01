namespace ContextChat.ServerClients.Auth

open System
open System.Linq
open System.Collections
open System.Diagnostics

open Newtonsoft.Json
open Newtonsoft.Json.Linq

open Xamarin.Auth
open Xamarin.Essentials

open ContextChat


type IAuthenticate =
    abstract member authenticate :unit -> Async<bool>


type MySecureStorage () =
    static member addAccount (account :Account, serviceId :string) =
        SecureStorage.SetAsync(serviceId, JsonConvert.SerializeObject(account))
        |> Async.AwaitTask 

    static member getAccount (serviceId) =
        async {
            let! accountString = SecureStorage.GetAsync(serviceId) |> Async.AwaitTask
            try 
                return Some(JsonConvert.DeserializeObject<Account>(accountString))
            with exn -> 
                Debug.WriteLine(exn.Message)
                return None
            }



type GoogleOAuth2 () =
    (*31:42:12:42:31:32:21:31:56:53:12:43:90:32:87:11:12:56:52:21
    {"installed":{"client_id":"597529832691-aocctpi2ig5m85kfmj2gnh49iuuc7o4q.apps.googleusercontent.com","project_id":"geochat2","auth_uri":"https://accounts.google.com/o/oauth2/auth","token_uri":"https://oauth2.googleapis.com/token","auth_provider_x509_cert_url":"https://www.googleapis.com/oauth2/v1/certs","redirect_uris":["urn:ietf:wg:oauth:2.0:oob","http://localhost"]}}
    *)

    static member val private clientId = 
        "597529832691-aocctpi2ig5m85kfmj2gnh49iuuc7o4q.apps.googleusercontent.com"
    static member val private scope = "https://www.googleapis.com/auth/userinfo.email"
    static member val private authorizeUrl = "https://accounts.google.com/o/oauth2/auth"
    static member val private redirectUrl =
        "com.googleusercontent.apps.597529832691-aocctpi2ig5m85kfmj2gnh49iuuc7o4q:/oauth2redirect"
    static member val private accessTokenUrl = "https://oauth2.googleapis.com/token"
    static member val private useNativeUI = true

    static member val authenticator = 
        let auth =
            OAuth2Authenticator(GoogleOAuth2.clientId, null, GoogleOAuth2.scope
                , Uri(GoogleOAuth2.authorizeUrl), Uri(GoogleOAuth2.redirectUrl)
                , Uri(GoogleOAuth2.accessTokenUrl)
                , null //GetUsernameAsyncFunc(fun user -> new Threading.Tasks.Task<string>(fun u -> failwith "aaa"; ""))
                , GoogleOAuth2.useNativeUI)
        (fun (eventArgs :AuthenticatorCompletedEventArgs) -> 
            if eventArgs.IsAuthenticated then 
                MySecureStorage.addAccount(eventArgs.Account, Constants.GoogleAccountService)
                |> Async.StartImmediate)
        |> auth.Completed.Add
        auth



    

