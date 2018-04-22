module FSharp_i3wm.Tree

open FSharp_i3wm.Prelude
open FSharpx.Collections.Experimental

open FSharpx.Functional.Prelude
open FSharpx.Option
open FSharp.Data
open FSharp.Data.JsonExtensions
module RT = FSharpx.Collections.Experimental.EagerRoseTree
module Node = FSharp_i3wm.Node

type T = {
    node: Node.T
    tiling_nodes: list<T>
    floating_nodes: list<T>
}
type Tree = T

let getNode (tree: Tree): Node.T = tree.node
let getTilingNodes (tree: Tree) = tree.tiling_nodes
let getFloatingNodes (tree: Tree) = tree.floating_nodes

let getId: Tree -> int                  = getNode >> Node.getId
let getName: Tree -> string             = getNode >> Node.getName
let getType: Tree -> Container          = getNode >> Node.getType
let getBorder: Tree -> BorderStyle      = getNode >> Node.getBorder
let getCurrentBorderWidth: Tree -> int  = getNode >> Node.getCurrentBorderWidth
let getLayout: Tree -> Layout           = getNode >> Node.getLayout
let getOrientation: Tree -> Orientation = getNode >> Node.getOrientation
let getPercent: Tree -> option<float>   = getNode >> Node.getPercent
let getRect: Tree -> Rect               = getNode >> Node.getRect
let getWindowRect: Tree -> Rect         = getNode >> Node.getWindowRect
let getDecoRect: Tree -> Rect           = getNode >> Node.getDecoRect
let getGeometry: Tree -> Rect           = getNode >> Node.getGeometry
let getWindow: Tree -> option<int>      = getNode >> Node.getWindow
let urgent: Tree -> bool                = getNode >> Node.urgent
let focused: Tree -> bool               = getNode >> Node.focused
let getFocus: Tree -> array<int>        = getNode >> Node.getFocus

type T with
    member this.id(): int = getId this
    member this.name(): string              = getName this
    member this.typ(): Container            = getType this
    member this.border(): BorderStyle       = getBorder this
    member this.current_border_width(): int = getCurrentBorderWidth this
    member this.layout(): Layout            = getLayout this
    member this.orientation(): Orientation  = getOrientation this
    member this.percent(): option<float>    = getPercent this
    member this.rect(): Rect                = getRect this
    member this.window_rect(): Rect         = getWindowRect this
    member this.deco_rect(): Rect           = getDecoRect this
    member this.geometry(): Rect            = getGeometry this
    member this.window(): option<int>       = getWindow this
    member this.urgent(): bool              = urgent this
    member this.focused(): bool             = focused this
    member this.focus(): array<int>         = getFocus this

let parseJsonRect (rect:JsonValue) : Rect =
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

let rec private parseJsonTree (jsonTree : JsonValue) : Tree =
    let node: Node.T = { 
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
        rect = jsonTree?rect |> parseJsonRect 
        window_rect = jsonTree?window_rect |> parseJsonRect 
        deco_rect = jsonTree?deco_rect |> parseJsonRect 
        geometry = jsonTree?geometry |> parseJsonRect 
        window =
            let window = jsonTree?window
            if window = JsonValue.Null then None else
            Some (window.AsInteger())
        urgent = jsonTree?urgent.AsBoolean()
        focused = jsonTree?focused.AsBoolean()
        focus = jsonTree?focus.AsArray() |> Array.map (fun focus -> focus.AsInteger())
        }
    {
        node = node
        tiling_nodes =   jsonTree?nodes.AsArray()
                         |> Array.map parseJsonTree
                         |> List.ofArray
        floating_nodes = jsonTree?floating_nodes.AsArray() 
                         |> Array.map parseJsonTree
                         |> List.ofArray
    }

let parseTree : string -> Tree =
    JsonValue.Parse >> parseJsonTree

type private RoseTree<'A> = FSharpx.Collections.Experimental.RoseTree<'A>

let rec mapTiling (f: Node.T -> 'A) (tree:Tree) : EagerRoseTree<'A> =
    { Root = f tree.node
      Children = tree.floating_nodes |> List.map (mapTiling f) }

let rec mapFloating (f: Node.T -> 'A) (tree:Tree) : EagerRoseTree<'A> =
    { Root = f tree.node
      Children = tree.floating_nodes |> List.map (mapFloating f) }

let getChild (id: int) (tree:Tree) : option<Tree> =
    if tree.id() = id then Some tree else
    let tryFindID: list<Tree> -> option<Tree> =
        List.tryFind(fun t -> t.id() = id)
    tree |> duplicate
         |> mapFst (getTilingNodes   >> tryFindID)
         |> mapSnd (getFloatingNodes >> tryFindID)
         |> function | Some t, _
                     | _, Some t -> Some t
                     | _ -> None

type T with member this.getChild(id: int): option<Tree> = getChild id this

let rec getFocused (tree:Tree) : option<Tree> =
    if tree.focused() then Some tree else
    Array.tryHead (tree.focus())
    >>= tree.getChild
    >>= getFocused

type T with member this.getFocused(): option<Tree> = getFocused this