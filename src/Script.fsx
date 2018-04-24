// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#I @"../.paket/load/net471/"
#load "FSharpx.Extras.fsx"
#load "FSharpx.Collections.Experimental.fsx"
#load "FSharp.Data.fsx"
#load "FSharp_i3wm.fs"
#load "Prelude.fs"
#load "BorderStyle.fs"
#load "Container.fs"
#load "Layout.fs"
#load "Orientation.fs"
#load "Rect.fs"
#load "Node.fs"
#load "Tree.fs"
#load "Message.fs"


open FSharpx.Functional.Prelude
open FSharp_i3wm
open FSharp_i3wm.Message

module Option = FSharpx.Option
module String = FSharpx.String
module Tree   = FSharp_i3wm.Tree

let rec waitUntil (f: Tree.T -> bool): unit =
    if f ^ getTree() then () else waitUntil f

let getFocusedName: Tree.T -> option<string> =
    Tree.getFocused >> Option.map Tree.getName

let focusedNameHasPrefix (prefix: string): Tree.T -> bool =
    getFocusedName
    >> Option.map ^ String.startsWith prefix
    >> Option.getOrElse false
    
let focusedNameHasSuffix (suffix: string): Tree.T -> bool =
    getFocusedName
    >> Option.map ^ String.endsWith suffix
    >> Option.getOrElse false

let smartSplith: unit -> unit = getTree >> Tree.getLayout 
                                        >> function | Layout.Splith -> ()
                                                    | _ -> layout "splith"
let smartSplitv: unit -> unit = getTree >> Tree.getLayout 
                                        >> function | Layout.Splitv -> ()
                                                    | _ -> layout "splitv"
let smartSplitTabbed: unit -> unit = getTree >> Tree.getLayout 
                                             >> function | Layout.Tabbed -> ()
                                                         | _ -> layout "tabbed"

focusOutput "HDMI-A-0"
workspace "1"
waitUntil ^ focusedNameHasPrefix "1"
// workspace 1 | workspace 2
(* *[]* | [] *)
exec "gnome-terminal --execute cmus"
waitUntil ^ focusedNameHasPrefix "cmus"
split "vertical" 
(* [*cmus*] | [] *)
exec "telegram"
waitUntil ^ focusedNameHasPrefix "Telegram"
(* [cmus      ] | ⌈⌉
   [*Telegram*] | ⌊⌋ *)
split "horizontal"
layout "tabbed"
exec "qbittorrent"
waitUntil ^ focusedNameHasPrefix "qBittorrent"
(* [cmus                  ] | ⌈⌉
   [Telegram|*qBittorrent*] | ⌊⌋ *)
focus "left"
waitUntil ^ focusedNameHasPrefix "Telegram"
(* [cmus                  ] | ⌈⌉
   [*Telegram*|qBittorrent] | ⌊⌋ *)
focus "up"
waitUntil ^ focusedNameHasPrefix "cmus"
(* [*cmus              *] | ⌈⌉
   [Telegram|qBittorrent] | ⌊⌋ *)
split "horizontal"
layout "tabbed"
focus "parent"
(* *[cmus                ]* | ⌈⌉
    [Telegram|qBittorrent]  | ⌊⌋ *)
focus "parent"
waitUntil ^ focusedNameHasPrefix "1"
(* *[cmus                ]* | ⌈⌉
   *[Telegram|qBittorrent]* | ⌊⌋ *)
split "horizontal"
exec "chromium-browser"
waitUntil ^ focusedNameHasSuffix "Chromium"
(* [cmus                ]⌈*Chromium*⌉ | ⌈⌉
   [Telegram|qBittorrent]⌊ 	      ⌋ | ⌊⌋ *)
split "horizontal"
layout "tabbed"
workspace "2"
waitUntil ^ focusedNameHasPrefix "2"
(* [cmus                ]⌈Chromium⌉ | *⌈⌉*
   [Telegram|qBittorrent]⌊ 	    ⌋ | *⌊⌋* *)
exec "gnome-terminal"
waitUntil ^ focusedNameHasPrefix "Terminal"
(* [cmus                ]⌈Chromium⌉ | ⌈*Terminal*⌉
   [Telegram|qBittorrent]⌊ 	    ⌋ | ⌊          ⌋ *)
focus "parent"
waitUntil ^ (not << focusedNameHasPrefix "Terminal")
(* [cmus                ]⌈Chromium⌉ | *⌈Terminal⌉*
   [Telegram|qBittorrent]⌊ 	    ⌋ |  ⌊        ⌋ *)
exec "gnome-terminal"
waitUntil ^ focusedNameHasPrefix "Terminal"
layout "tabbed"
(* [cmus                ]⌈Chromium⌉ | ⌈Terminal|*Terminal*⌉
   [Telegram|qBittorrent]⌊ 	    ⌋ | ⌊        |          ⌋ *)

