module ContextChat.ServerClients.Azure.ApiClient

open ContextChat.ServerClients.Auth

open System.Linq.Expressions

open Microsoft.WindowsAzure.MobileServices

open Newtonsoft.Json
open Newtonsoft.Json.Linq


type TodoItem (?idArg, ?textArg) =
    (*let mutable _id = id
    let mutable _text = text
    member x.id with get() = _id and set(v) = _id <- v
    member x.Text with get() = _text and set(v) = _text <- v
    *)
    [<JsonProperty(PropertyName = "id")>]
    member val id = defaultArg idArg null with get, set

    [<JsonProperty(PropertyName = "text")>]
    member val text = defaultArg textArg "" with get, set

    [<JsonProperty(PropertyName = "complete")>]
    member val donep = false with get, set


type ApiClient () =
    do ()

    static member val authenticator: IAuthenticate option = None with get, set

    static member val mobileServiceClient = 
        new MobileServiceClient("https://geochatapi2.azurewebsites.net") with get, set

    member x.table = ApiClient.mobileServiceClient.GetTable<TodoItem>() 

    member x.add(item :TodoItem) =
        if(item.id = null) then x.table.InsertAsync(item) else x.table.UpdateAsync(item)
        |> Async.AwaitTask

    member x.load() = 
        x.table.Where(fun a -> true).ToEnumerableAsync() // fun (item :TodoItem) -> item.Done |> not).ToEnumerableAsync() 
        |> Async.AwaitTask        

    