module ContextChat.Models

type GeoHome (?location) =
    let mutable _location = match location with None -> "" | Some(o) -> o

    member x.location 
        with get() = _location
        and set(value) = _location <- value

    