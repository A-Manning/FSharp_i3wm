module FSharp_i3wm.Tree

open FSharp_i3wm.Prelude
open FSharpx.Collections.Experimental

open FSharpx.Functional.Prelude
open FSharpx.Option
open FSharp.Data
open FSharp.Data.JsonExtensions
module RT = FSharpx.Collections.Experimental.EagerRoseTree
module Node = FSharp_i3wm.Node
module Rect = FSharp_i3wm.Rect

type private RoseTree<'A> = 
    FSharpx.Collections.Experimental.RoseTree<'A>

type T = {
    node: Node.T
    tiling_nodes: list<T>
    floating_nodes: list<T>
}
type Tree = T

let getNode (tree: Tree): Node.T = tree.node
let getTilingNodes (tree: Tree) = tree.tiling_nodes
let getFloatingNodes (tree: Tree) = tree.floating_nodes

let getId: Tree -> int                    = getNode >> Node.getId
let getName: Tree -> string               = getNode >> Node.getName
let getType: Tree -> Container.T          = getNode >> Node.getType
let getBorder: Tree -> BorderStyle.T      = getNode >> Node.getBorder
let getCurrentBorderWidth: Tree -> int    = getNode >> Node.getCurrentBorderWidth
let getLayout: Tree -> Layout.T           = getNode >> Node.getLayout
let getOrientation: Tree -> Orientation.T = getNode >> Node.getOrientation
let getPercent: Tree -> option<float>     = getNode >> Node.getPercent
let getRect: Tree -> Rect.T               = getNode >> Node.getRect
let getWindowRect: Tree -> Rect.T         = getNode >> Node.getWindowRect
let getDecoRect: Tree -> Rect.T           = getNode >> Node.getDecoRect
let getGeometry: Tree -> Rect.T           = getNode >> Node.getGeometry
let getWindow: Tree -> option<int>        = getNode >> Node.getWindow
let urgent: Tree -> bool                  = getNode >> Node.urgent
let focused: Tree -> bool                 = getNode >> Node.focused
let getFocus: Tree -> array<int>          = getNode >> Node.getFocus

type T with
    member this.id(): int = getId this
    member this.name(): string               = getName this
    member this.typ(): Container.T           = getType this
    member this.border(): BorderStyle.T      = getBorder this
    member this.current_border_width(): int  = getCurrentBorderWidth this
    member this.layout(): Layout.T           = getLayout this
    member this.orientation(): Orientation.T = getOrientation this
    member this.percent(): option<float>     = getPercent this
    member this.rect(): Rect.T               = getRect this
    member this.window_rect(): Rect.T        = getWindowRect this
    member this.deco_rect(): Rect.T          = getDecoRect this
    member this.geometry(): Rect.T           = getGeometry this
    member this.window(): option<int>        = getWindow this
    member this.urgent(): bool               = urgent this
    member this.focused(): bool              = focused this
    member this.focus(): array<int>          = getFocus this
    
    member this.getChild(id: int): option<Tree> =
        if this.id() = id then Some this else
        let tryFindID: list<Tree> -> option<Tree> =
            List.tryFind(fun t -> t.id() = id)
        this |> duplicate
             |> mapFst (getTilingNodes   >> tryFindID)
             |> mapSnd (getFloatingNodes >> tryFindID)
             |> function | Some t, _
                         | _, Some t -> Some t
                         | _ -> None

let rec mapTiling (f: Node.T -> 'A) (tree:Tree): EagerRoseTree<'A> =
    { Root = f tree.node
      Children = tree.tiling_nodes |> List.map (mapTiling f) }

let rec mapFloating (f: Node.T -> 'A) (tree:Tree): EagerRoseTree<'A> =
    { Root = f tree.node
      Children = tree.floating_nodes |> List.map (mapFloating f) }

let getChild (id: int) (tree:Tree): option<Tree> = tree.getChild(id)

let rec getFocused (tree:Tree) : option<Tree> =
    if tree.focused() then Some tree else
    Array.tryHead ^ getFocus tree
    >>= tree.getChild
    >>= getFocused

let rec getWorkspaces (tree: Tree): list<Tree> =
    begin match tree.typ() with
    | Container.WorkspaceContainer -> [tree]
    | _ -> []
    end
    @ List.collect getWorkspaces tree.tiling_nodes
    @ List.collect getWorkspaces tree.floating_nodes
    

type T with
    member this.getFocused(): option<Tree> = getFocused this

let rec parseJson (tree: JsonValue): Tree =
    let parseArray = Array.map parseJson >> List.ofArray
    { node = Node.parseJson tree
      tiling_nodes   = tree?nodes.AsArray()          |> parseArray
      floating_nodes = tree?floating_nodes.AsArray() |> parseArray }

let parse: string -> Tree =
    JsonValue.Parse >> parseJson