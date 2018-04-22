module FSharp_i3wm.BorderStyle

type T =
    | Normal
    | NoBorderStyle
    | Pixel

type BorderStyle = T

let parse: string -> BorderStyle = function
    | "normal" -> Normal
    | "none" -> NoBorderStyle
    | "pixel" -> Pixel
    | _ -> failwith "Not a border style"