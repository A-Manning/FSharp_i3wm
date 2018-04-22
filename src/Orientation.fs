module FSharp_i3wm.Orientation

type T =
    | Horizontal
    | Vertical
    | NoOrientation
type Orientation = T

let parse: string -> Orientation = function
    | "horizontal" -> Horizontal
    | "vertical" -> Vertical
    | "none" -> NoOrientation
    | _ -> failwith "Not an orientation"