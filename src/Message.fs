module FSharp_i3wm.Message

open FSharp_i3wm.Prelude
open FSharpx.Functional.Prelude
open System.Diagnostics
module Tree = FSharp_i3wm.Tree

let private send (s: string): string =
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

let getTree():  Tree.T = send "-t get_tree"
                         |> Tree.parseTree
                         
let exec: string -> unit =
    sprintf "\"exec %s\"" >> send >> ignore

let execNoStartupId: string -> unit =
    sprintf "\"exec --no-startup-id %s\"" >> send >> ignore

let split (s: string): unit =
    match s with
    | "vertical"
    | "horizontal"
    | "toggle" -> send ("split " + s) |> ignore
    | s -> failwithf "not a valid argument to `split`: `%s`" s

let layout (s: string): unit =
    match s with
    | "default"
    | "tabbed"
    | "stacking"
    | "splitv"
    | "splith" -> send ("layout " + s) |> ignore
    | s -> failwithf "not a valid argument to `layout`: `%s`" s

let layoutToggleSplit = send "layout toggle split" |> ignore
let layoutToggleAll = send "layout toggle all" |> ignore

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
         |> ignore

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
    | "mode_toggle" -> send ("focus " + s) |> ignore
    | _ -> failwithf "not a valid argument to `focus`: `%s`" s

let focusOutput: string -> unit =
    sprintf "focus output %s" >> send >> ignore 