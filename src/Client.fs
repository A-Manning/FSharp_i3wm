module FSharp_i3wm.Client

open FSharp_i3wm.Prelude
open FSharpx.Functional.Prelude
open Mono.Unix
open System.Diagnostics
open System.Net.Sockets

module Command = FSharp_i3wm.Command

let private getSocketPath () : string =
    use p = new Process()
    p.StartInfo.FileName <- "i3";
    p.StartInfo.Arguments <- "--get-socketpath"
    p.StartInfo.UseShellExecute <- false;
    p.StartInfo.RedirectStandardOutput <- true;
    ignore <| p.Start()
    p.WaitForExit();
    let exitcode = p.ExitCode
    let stdOut = p.StandardOutput.ReadToEnd().Trim()
    let stdErr = p.StandardError .ReadToEnd().Trim()
    if exitcode = 0 then stdOut
    else failwith ^
            sprintf "`i3 --getsocketpath` returned error code `%d`." exitcode
            + sprintf "\nStandard Output:\n`%s`" stdOut
            + sprintf "\nStandard Error:\n`%s`"  stdErr
            
let private readFromStream (size: int) (stream:NetworkStream): array<byte> =
    let buffer = Array.zeroCreate size
    if stream.Read(buffer, 0, size) = size then buffer
    else failwith "failed to read stream"

let private readMessageFromStream (stream:NetworkStream): (Command.T * string) =
    let read (size: int): byte[] = readFromStream size stream 
    match read 6 with
    | "i3-ipc"B -> ()
    | _ -> failwith "error reading from networkStream" 
    
    let messageLength = bytesToInt ^ read 4
    let command = Command.ofBytes ^ read 4
    let message = bytesToString ^ read messageLength
    command, message

let private writeMessageToStream (stream:NetworkStream) (msg:byte[]) : unit =
    stream.Write(msg, 0, msg.Length)

type T () =
    let client = new UnixClient(getSocketPath())
    let stream = client.GetStream()
    
    member this.readMessage(): Command.T * string = readMessageFromStream stream
    member this.writeMessage: array<byte> -> unit = writeMessageToStream  stream

type Client = T
let readMessage (client: Client): Command.T * string = client.readMessage()
let writeMessage(client: Client): array<byte> -> unit = client.writeMessage
