module App

open Fable.Core
open Browser
open Fable.React
open Elmish
// open Elmish.React

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

let private mkProgram () =
    Program.mkProgram Index.init Index.update Index.view
    // Program.mkProgram Index.init Index.update (fun _ _ -> ())
#if DEBUG
    |> Program.withConsoleTrace
#endif
    // |> Program.withSubscription
    |> Program.withReactSynchronous "app-container"
#if DEBUG
    |> Program.withDebugger
#endif
    |> Program.run

// [<JSX.Component>]
// let App () =
//     let model, dispatch = React.useElmish (mkProgram, arg = ())

//     Index.view model dispatch

// let root = ReactDomClient.createRoot (document.getElementById ("app-container"))
// root.render (App ())

// JSX.jsx
//         $"""
//     { Index.view model dispatch }
//     """