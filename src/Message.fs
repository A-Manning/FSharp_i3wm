module FSharp_i3wm.Message

open FSharp_i3wm.Prelude
open FSharpx.Functional.Prelude
open System.Diagnostics
module Tree = FSharp_i3wm.Tree

let private send' (s: string): string =
    use p = new Process()
    p.StartInfo.FileName <- "i3-msg";
    p.StartInfo.Arguments <- s
    p.StartInfo.UseShellExecute <- false;
    p.StartInfo.RedirectStandardOutput <- true;
    p.StartInfo.RedirectStandardError <- true;
    ignore <| p.Start()
    p.WaitForExit();
    let exitcode = p.ExitCode
    let stdOut = p.StandardOutput.ReadToEnd().Trim()
    let stdErr = p.StandardError .ReadToEnd().Trim()
    if exitcode = 0 then stdOut
    else failwith ^
            sprintf "`i3-msg %s` returned error code `%d`." s exitcode
            + sprintf "\nStandard Output:\n`%s`" stdOut
            + sprintf "\nStandard Error:\n`%s`"  stdErr

let private send: string -> unit = send' >> ignore

let getTree():  Tree.T = send' "-t get_tree"
                         |> Tree.parseTree
                         
let exec: string -> unit =
    sprintf "\"exec %s\"" >> send

let execNoStartupId: string -> unit =
    sprintf "\"exec --no-startup-id %s\"" >> send

let split (s: string): unit =
    match s with
    | "vertical"
    | "horizontal"
    | "toggle" -> send ^ "split " + s
    | s -> failwithf "not a valid argument to `split`: `%s`" s

let layout (s: string): unit =
    match s with
    | "default"
    | "tabbed"
    | "stacking"
    | "splitv"
    | "splith" -> send ^ "layout " + s
    | s -> failwithf "not a valid argument to `layout`: `%s`" s

let layoutToggleSplit = send "layout toggle split"
let layoutToggleAll = send "layout toggle all"

let layoutToggle (args: list<string>): unit =
    let checkArg: string -> unit = function
        | "split"
        | "tabbed"
        | "stacking"
        | "splitv"
        | "splith" -> ()
        | s -> failwithf "not a valid argument to `layout toggle`: `%s`" s
    args |>! List.iter checkArg
         |> String.concat " "
         |> sprintf "layout toggle %s"
         |> send

let focus (s: string): unit =
    match s with
    | "left"
    | "right"
    | "up"
    | "down"
    | "parent"
    | "child"
    | "floating"
    | "tiling"
    | "mode_toggle" -> send ^ "focus " + s
    | _ -> failwithf "not a valid argument to `focus`: `%s`" s

let focusOutput: string -> unit =
    sprintf "focus output %s" >> send
    
let workspace: string -> unit =
    sprintf "\"workspace %s\"" >> send

let workspaceNoAutoBackAndForth: string -> unit =
    sprintf "\"workspace --no-auto-back-and-forth %s\"" >> send