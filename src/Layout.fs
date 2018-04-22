module FSharp_i3wm.Layout

type T =
    | Splith
    | Splitv
    | Stacked
    | Tabbed
    | DockArea
    | Output
type Layout = T

let parse: string -> Layout = function
    | "splith" -> Splith
    | "splitv" -> Splitv
    | "stacked" -> Stacked
    | "tabbed" -> Tabbed
    | "dockarea" -> DockArea
    | "output" -> Output
    | _ -> failwith "Not a layout"