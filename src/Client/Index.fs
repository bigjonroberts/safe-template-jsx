module Index

open Elmish
open Fable.Remoting.Client
open Shared

type Model = { Todos: Todo list; Input: string }

type Msg =
    | GotTodos of Todo list
    | SetInput of string
    | AddTodo
    | AddedTodo of Todo

let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

let init () : Model * Cmd<Msg> =
    let model = { Todos = []; Input = "" }

    let cmd = Cmd.OfAsync.perform todosApi.getTodos () GotTodos

    model, cmd

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | GotTodos todos -> { model with Todos = todos }, Cmd.none
    | SetInput value -> { model with Input = value }, Cmd.none
    | AddTodo ->
        let todo = Todo.create model.Input

        let cmd = Cmd.OfAsync.perform todosApi.addTodo todo AddedTodo

        { model with Input = "" }, cmd
    | AddedTodo todo -> { model with Todos = model.Todos @ [ todo ] }, Cmd.none

open Fable.Core
// open Fable.Core.JsInterop
open Feliz
open Feliz.Bulma

let navBrand =
    Bulma.navbarBrand.div [
        Bulma.navbarItem.a [
            prop.href "https://safe-stack.github.io/"
            navbarItem.isActive
            prop.children [
                Html.img [
                    prop.src "/favicon.png"
                    prop.alt "Logo"
                ]
            ]
        ]
    ]

[<JSX.Component>]
let containerBox (model: Model) (dispatch: Msg -> unit) =
    JSX.jsx
        $"""
        <div className="box">
            <div className="content">
                <ol>
                    {model.Todos
                     |> List.map (fun todo -> Html.li [ prop.text todo.Description ])}
                </ol>
            </div>
            <div className="field is-grouped">
                <p className="control is-expanded">
                    <text   className="input"
                            value="{model.Input}"
                            placeholder="What needs to be done?"
                            onChange={(fun x -> SetInput x |> dispatch)}/>
                </p>
                <p className="control">
                    <a  className="button is-primary"
                        disabled={Todo.isValid model.Input |> not}
                        onClick={fun _ -> dispatch AddTodo} />
                </p>
            </div>
        </div>
    """
    |> toReact

let view (model: Model) (dispatch: Msg -> unit) =
    JSX.jsx
        $"""
        <div    className="hero is-full-height is-primary"
                style="background-size:cover;background-image-url:https://unsplash.it/1200/900?random;background-position:no-repeat center center fixed">
            <div className="hero-head">
                <div className="nav-bar">
                    <div className="container">
                        <div className="navbar-brand">
                            <a className="navbar-item is-active" href="https://safe-stack.github.io">
                                <img src="/favicon.png" alt="Logo"/>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            <div className="hero-body">
                <div className="container">
                    <div className="column is-6 is-offset-3">
                        <div className="title has-text-centered">
                            safe_template_jsx
                        </div>
                        {containerBox model dispatch}
                    </div>
                </div>
            </div>
        </div>
    """
    |> toReact

open Fable.React

[<JSX.Component>]
let App () =
    let model, dispatch = React.useElmish (init, update, arg = ())

    view model dispatch