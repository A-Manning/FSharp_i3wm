module FSharp_i3wm.Container

type T =
    | RootContainer
    | OutputContainer
    | Container
    | FloatingContainer
    | WorkspaceContainer
    | DockAreaContainer

type Container = T

let parse: string -> Container = function
    | "root" -> RootContainer
    | "output" -> OutputContainer
    | "con" -> Container
    | "floating_con" -> FloatingContainer
    | "workspace" -> WorkspaceContainer
    | "dockarea" -> DockAreaContainer
    | _ -> failwith "Not a container type"