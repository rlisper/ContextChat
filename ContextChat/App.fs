namespace ContextChat

open System.Threading.Tasks

open ContextChat.Models
open ContextChat.ViewModels
open ContextChat.ServerClients.Auth

open MvvmCross.IoC
open MvvmCross.ViewModels


[<AllowNullLiteral>]
type App () = 
    inherit MvxApplication ()

    let _geoHome = GeoHome("not available")
   
    override x.Initialize() =
        base.CreatableTypes()
            .EndingWith("Service")
            .AsInterfaces()
            .RegisterAsLazySingleton()

        base.RegisterAppStart<HomeViewModel>()

    member x.mainModel = _geoHome

    static member val authenticator :IAuthenticate option = None with get, set

    static member Init(authenticator) =
        App.authenticator <- authenticator
        ServerClients.Azure.ApiClient.ApiClient.authenticator <- authenticator