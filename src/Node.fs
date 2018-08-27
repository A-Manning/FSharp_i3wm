module FSharp_i3wm.Node

open FSharp.Data
open FSharp.Data.JsonExtensions

module BorderStyle = FSharp_i3wm.BorderStyle
module Container   = FSharp_i3wm.Container
module Layout      = FSharp_i3wm.Layout
module Orientation = FSharp_i3wm.Orientation
module Rect        = FSharp_i3wm.Rect

type T = {
    id                  : int64
    name                : string
    typ                 : Container.T
    border              : BorderStyle.T
    current_border_width: int
    layout              : Layout.T
    orientation         : Orientation.T
    percent             : option<float>
    rect                : Rect.T
    window_rect         : Rect.T
    deco_rect           : Rect.T
    geometry            : Rect.T
    window              : option<int>
    urgent              : bool
    focused             : bool
    focus               : array<int64>
}
type Node = T

let getId                 (node: Node): int64         = node.id
let getName               (node: Node): string        = node.name
let getType               (node: Node): Container.T   = node.typ
let getBorder             (node: Node): BorderStyle.T = node.border
let getCurrentBorderWidth (node: Node): int           = node.current_border_width
let getLayout             (node: Node): Layout.T      = node.layout
let getOrientation        (node: Node): Orientation.T = node.orientation
let getPercent            (node: Node): option<float> = node.percent
let getRect               (node: Node): Rect.T        = node.rect
let getWindowRect         (node: Node): Rect.T        = node.window_rect
let getDecoRect           (node: Node): Rect.T        = node.deco_rect
let getGeometry           (node: Node): Rect.T        = node.geometry
let getWindow             (node: Node): option<int>   = node.window
let urgent                (node: Node): bool          = node.urgent
let focused               (node: Node): bool          = node.focused
let getFocus              (node: Node): array<int64>  = node.focus
    
let parseJson (tree: JsonValue): Node =
    {
        id = try tree?id.AsInteger64() 
             with _ -> failwith "Failed to parse id"
        name = try tree?name.AsString()
               with _ -> failwith "Failed to parse name"
        typ = try tree.GetProperty("type").AsString() 
                  |> Container.parse 
              with _ -> failwith "Failed to parse typ"
        border = try tree?border.AsString() 
                     |> BorderStyle.parse
                 with _ -> failwith "Failed to parse border"
        current_border_width =
            try tree?current_border_width.AsInteger() 
            with _ -> failwith "Failed to parse current_border_width"
        layout = try tree?layout.AsString() 
                     |> Layout.parse
                 with _ -> failwith "Failed to parse layout"
        orientation = try tree?orientation.AsString() 
                          |> Orientation.parse
                      with _ -> failwith "Failed to parse orientation"
        percent =  try let percent = tree?percent
                       if percent = JsonValue.Null then None else
                       Some (percent.AsFloat())
                   with _ -> failwith "Failed to parse percent"
        rect        = try tree?rect        |> Rect.parseJson
                      with _ -> failwith "Failed to parse rect"
        window_rect = try tree?window_rect |> Rect.parseJson 
                      with _ -> failwith "Failed to parse window_rect"
        deco_rect   = try tree?deco_rect   |> Rect.parseJson
                      with _ -> failwith "Failed to parse deco_rect"
        geometry    = try tree?geometry    |> Rect.parseJson
                      with _ -> failwith "Failed to parse geometry"
        window = try let window = tree?window
                     if window = JsonValue.Null then None else
                     Some (window.AsInteger())
                 with _ -> failwith "Failed to parse window"
        urgent = try tree?urgent.AsBoolean()
                 with _ -> failwith "Failed to parse urgent"
        focused = try tree?focused.AsBoolean()
                  with _ -> failwith "Failed to parse focused"
        focus = try tree?focus.AsArray() 
                    |> Array.map (fun focus -> focus.AsInteger64())
                with _ -> failwith "Failed to parse focus"
    }