module FSharp_i3wm.Node

type T = {
    id: int
    name: string
    typ: Container
    border: BorderStyle
    current_border_width: int
    layout: Layout
    orientation: Orientation
    percent: option<float>
    rect: Rect
    window_rect: Rect
    deco_rect: Rect
    geometry: Rect
    window: option<int>
    urgent: bool
    focused: bool
    focus: array<int>
}

type Node = T

let getId (node: Node): int = node.id
let getName (node: Node): string = node.name
let getType (node: Node): Container = node.typ
let getBorder (node: Node): BorderStyle = node.border
let getCurrentBorderWidth (node: Node): int = node.current_border_width
let getLayout (node: Node): Layout = node.layout
let getOrientation (node: Node): Orientation = node.orientation
let getPercent (node: Node): option<float> = node.percent
let getRect (node: Node): Rect = node.rect
let getWindowRect (node: Node): Rect = node.window_rect
let getDecoRect (node: Node): Rect = node.deco_rect
let getGeometry (node: Node): Rect = node.geometry
let getWindow (node: Node): option<int> = node.window
let urgent (node: Node): bool = node.urgent
let focused (node: Node): bool = node.focused
let getFocus (node: Node): array<int> = node.focus