module FSharp_i3wm.Node

open FSharp.Data
open FSharp.Data.JsonExtensions

module BorderStyle = FSharp_i3wm.BorderStyle
module Container   = FSharp_i3wm.Container
module Layout      = FSharp_i3wm.Layout
module Orientation = FSharp_i3wm.Orientation
module Rect        = FSharp_i3wm.Rect

type T = {
    id                  : int
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
    focus               : array<int>
}
type Node = T

let getId                 (node: Node): int           = node.id
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
let getFocus              (node: Node): array<int>    = node.focus

let parseJson (tree: JsonValue): Node =
    {
        id = tree?id.AsInteger()
        name = tree?name.AsString()
        typ = tree.GetProperty("type").AsString() |> Container.parse
        border = tree?border.AsString() |> BorderStyle.parse
        current_border_width = tree?current_border_width.AsInteger() 
        layout = tree?layout.AsString() |> Layout.parse
        orientation = tree?orientation.AsString() |> Orientation.parse
        percent = 
            let percent = tree?percent
            if percent = JsonValue.Null then None else
            Some (percent.AsFloat())
        rect        = tree?rect        |> Rect.parseJson
        window_rect = tree?window_rect |> Rect.parseJson 
        deco_rect   = tree?deco_rect   |> Rect.parseJson 
        geometry    = tree?geometry    |> Rect.parseJson 
        window =
            let window = tree?window
            if window = JsonValue.Null then None else
            Some (window.AsInteger())
        urgent = tree?urgent.AsBoolean()
        focused = tree?focused.AsBoolean()
        focus = tree?focus.AsArray() |> Array.map (fun focus -> focus.AsInteger())
    }