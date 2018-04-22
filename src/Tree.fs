module FSharp_i3wm.Tree

open FSharpx.Functional.Prelude
open FSharpx.Option

type T = {
    id : int
    name : string
    typ : Container
    border : BorderStyle
    current_border_width : int
    layout : Layout
    orientation : Orientation
    percent : option<float>
    rect : Rect
    window_rect : Rect
    deco_rect : Rect
    geometry : Rect
    window : option<int>
    urgent : bool
    focused : bool
    focus : array<int>
    tiling_nodes : array<T>
    floating_nodes: array<T>
}
type Tree = T

let private mapFst (f: 'A -> 'C) (a: 'A, b: 'B): 'C * 'B = f a, b
let private mapSnd (f: 'B -> 'C) (a: 'A, b: 'B): 'A * 'C = a, f b
let private duplicate (x: 'A): 'A * 'A = x, x

let getID ({id=id}: Tree): int = id
let getName ({name=name}: Tree): string = name
let getFocus ({focus=focus}: Tree): array<int> = focus
let getTilingNodes (tree: Tree): array<Tree> = tree.tiling_nodes
let getFloatingNodes (tree: Tree): array<Tree> = tree.floating_nodes

let getChild (id: int) (tree:Tree) : option<Tree> =
    if tree.id = id then Some tree else
    let tryFindID: array<Tree> -> option<Tree> =
        Array.tryFind(fun t -> t.id = id)
    tree |> duplicate
         |> mapFst (getTilingNodes   >> tryFindID)
         |> mapSnd (getFloatingNodes >> tryFindID)
         |> function | Some t, _
                     | _, Some t -> Some t
                     | _ -> None

type T with member this.getChild(id: int): option<Tree> = getChild id this

let rec getFocused (tree:Tree) : option<Tree> =
    if tree.focused then Some tree else
    Array.tryHead tree.focus
    >>= tree.getChild
    >>= getFocused

type T with member this.getFocused(): option<Tree> = getFocused this