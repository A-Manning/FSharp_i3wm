module FSharp_i3wm.Message

open FSharp_i3wm.Prelude
open FSharpx.Functional.Prelude
open System.Diagnostics

let private send (s: string): string =
    use p = new Process()
    p.StartInfo.FileName <- "i3-msg";
    p.StartInfo.Arguments <- s
    p.StartInfo.UseShellExecute <- false;
    p.StartInfo.RedirectStandardOutput <- true;
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
