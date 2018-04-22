namespace FSharp_i3wm

//open Mono.Unix
open System.Diagnostics
open System.Text
open System.Net.Sockets
open FSharp.Data
//open FSharp.Data.JsonExtensions

type Rect = {
    x : int
    y : int
    width : int
    height : int
}

type Response = {
    success : bool
    error : option<string>
}

type Output = {
    name : string
    active : bool
    primary : bool
    current_workspace : string
    rect : Rect
}

type Container =
    | RootContainer
    | OutputContainer
    | Container
    | FloatingContainer
    | WorkspaceContainer
    | DockAreaContainer

type BorderStyle =
    | Normal
    | NoBorderStyle
    | Pixel

type Orientation =
    | Horizontal
    | Vertical
    | NoOrientation

type Layout =
    | Splith
    | Splitv
    | Stacked
    | Tabbed
    | DockArea
    | Output

type MarksResponse = string[]

(*

let parseRect (rect:JsonValue) : Rect =
    { x = rect?x.AsInteger()
      y = rect?y.AsInteger()
      width = rect?width.AsInteger()
      height = rect?height.AsInteger() }

let toContainer : string -> Container = function
    | "root" -> RootContainer
    | "output" -> OutputContainer
    | "con" -> Container
    | "floating_con" -> FloatingContainer
    | "workspace" -> WorkspaceContainer
    | "dockarea" -> DockAreaContainer
    | _ -> failwith "Not a container type"

let toBorderStyle : string -> BorderStyle = function
    | "normal" -> Normal
    | "none" -> NoBorderStyle
    | "pixel" -> Pixel
    | _ -> failwith "Not a border style"

let toOrientation : string -> Orientation = function
    | "horizontal" -> Horizontal
    | "vertical" -> Vertical
    | "none" -> NoOrientation
    | _ -> failwith "Not an orientation"

let toLayout : string -> Layout = function
    | "splith" -> Splith
    | "splitv" -> Splitv
    | "stacked" -> Stacked
    | "tabbed" -> Tabbed
    | "dockarea" -> DockArea
    | "output" -> Output
    | _ -> failwith "Not a layout"

let rec parseTree (jsonTree : JsonValue) : Tree =
    { 
        id = jsonTree?id.AsInteger()
        name = jsonTree?name.AsString()
        typ = jsonTree.GetProperty("type").AsString() |> toContainer
        border = jsonTree?border.AsString() |> toBorderStyle
        current_border_width = jsonTree?current_border_width.AsInteger() 
        layout = jsonTree?layout.AsString() |> toLayout
        orientation = jsonTree?orientation.AsString() |> toOrientation
        percent = 
            let percent = jsonTree?percent
            if percent = JsonValue.Null then None else
            Some (percent.AsFloat())
        rect = jsonTree?rect |> parseRect 
        window_rect = jsonTree?window_rect |> parseRect 
        deco_rect = jsonTree?deco_rect |> parseRect 
        geometry = jsonTree?geometry |> parseRect 
        window =
            let window = jsonTree?window
            if window = JsonValue.Null then None else
            Some (window.AsInteger())
        urgent = jsonTree?urgent.AsBoolean()
        focused = jsonTree?focused.AsBoolean()
        focus = jsonTree?focus.AsArray() |> Array.map (fun focus -> focus.AsInteger())
        nodes = jsonTree?nodes.AsArray() |> Array.map parseTree
        floating_nodes = jsonTree?floating_nodes.AsArray() |> Array.map parseTree
    }

let toTree : string -> Tree =
    JsonValue.Parse >> parseTree

let writeMessage (stream:NetworkStream) (msg:byte[]) : unit =
    stream.Write(msg, 0, msg.Length)

*)
