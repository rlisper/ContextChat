namespace ContextChat.ViewModels

open System
open System.Net.Http

open Xamarin.Auth

open Newtonsoft.Json.Linq

open ContextChat
open ContextChat.Geo
open ContextChat.Models
open ContextChat.ServerClients.Azure.ApiClient

open MvvmCross
open MvvmCross.ViewModels
open MvvmCross.Commands

type HomeViewModel () =
    inherit MvxViewModel()

    let _homeModel = GeoHome()
    let _text = 
        match AccountStore.Create().FindAccountsForService(Constants.AppName) |> List.ofSeq with
        | account :: _ -> ref ("Hello " + account.Username)
        | _ -> ref "Hello guest"

    let _location: Location option ref = ref None
    let client = ApiClient()


    (*------------------------- Initialize -------------------------*)
    override x.Prepare () = 
        ()

    override x.Initialize () =
        base.Initialize()


    (*------------------------- Properties -------------------------*)
    member x.text 
        with get () = _text.Value
        and set (value) = base.SetProperty(_text, value, "text") |> ignore

    member x.location 
        with get () = _location.Value
        and set (value) = base.SetProperty(_location, value, "location") |> ignore


    (*------------------------- Commands -------------------------*)
    member x.commandResetText = new MvxCommand(fun () -> x.resetText())
    member x.resetText () = 
        x.text <- "You reset me!"

    member x.commandAuthenticate =
        fun _ ->
            match ApiClient.authenticator with
            | Some(a) -> a.authenticate() |> Async.Ignore |> Async.StartImmediate
            | None -> ()
        |> (fun c -> new Commands.MvxCommand(new System.Action(c)))

    member x.commandSave =
        fun _ -> 
            async{ 
                do! client.add(TodoItem(text = "test at" + System.DateTime.Now.ToShortTimeString())) 
                }
            |> Async.StartImmediate
        |> (fun c -> new Commands.MvxCommand(new System.Action(c)))

    member x.commandRetrive =
        fun _ ->
            async {
                let! res = client.load() 
                res
                |> Seq.toList
                |> List.map (fun item -> item.text)
                |> List.reduce (fun a b -> a + Environment.NewLine + b)
                |> (fun res -> x.text <- res)
                }
            |> Async.StartImmediate
        |> (fun c -> new Commands.MvxCommand(new System.Action(c)))

    member x.commandUpdateLocationOnServer =
       fun _ ->
           async{
               let httpClient = new HttpClient()
               let url = "https://geochatapi2.azurewebsites.net/echo/hello"
               let! response = httpClient.GetAsync(url) |> Async.AwaitTask
               let! responseStr = response.Content.ReadAsStringAsync() |> Async.AwaitTask
               x.text <- responseStr
           }
           |> Async.StartImmediate
       |> (fun c -> new Commands.MvxCommand(new System.Action(c)))