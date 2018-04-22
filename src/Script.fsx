// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#I @"../.paket/load/net471/"
#load "FSharpx.Extras.fsx"
#load "FSharpx.Collections.Experimental.fsx"
#load "FSharp.Data.fsx"
#load "FSharp_i3wm.fs"
#load "Prelude.fs"
#load "Node.fs"
#load "Tree.fs"
#load "Message.fs"

open FSharpx.Functional.Prelude
open FSharp_i3wm
open FSharp_i3wm.Message

module String = FSharpx.String
module Tree    = FSharp_i3wm.Tree

let rec waitUntil (f: Tree.T -> bool): unit =
    if f ^ getTree() then () else waitUntil f

let getFocusedName: Tree.T -> option<string> =
    Tree.getFocused >> Option.map Tree.getName

let focusedNameHasPrefix (prefix: string): Tree.T -> bool =
    getFocusedName
    >> Option.map ^ String.startsWith prefix
    >> function | Some true -> true
                | _ -> false
let focusedNameHasSuffix (suffix: string): Tree.T -> bool =
    getFocusedName
    >> Option.map ^ String.endsWith suffix
    >> function | Some true -> true
                | _ -> false

focusOutput "HDMI-A-0";
exec "gnome-terminal --execute cmus"
waitUntil ^ focusedNameHasPrefix "cmus"
split "vertical"
exec "telegram"
waitUntil ^ focusedNameHasPrefix "Telegram"
split "horizontal"
layout "tabbed"
exec "qbittorrent"
waitUntil ^ focusedNameHasPrefix "qBittorrent"
focus "left"
waitUntil ^ focusedNameHasPrefix "Telegram"
focus "up"
waitUntil ^ focusedNameHasPrefix "cmus"
split "horizontal"
layout "tabbed"
focus "parent"
focus "parent"
waitUntil ^ focusedNameHasPrefix "1"
split "horizontal"
exec "chromium-browser"
waitUntil ^ focusedNameHasSuffix "Chromium"
split "horizontal"
layout "tabbed"

//getTree() |> getFocusedName |> Option.iter(printfn "%s")