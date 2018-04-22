module FSharp_i3wm.Message

open System.Text
module Command = FSharp_i3wm.Command

type private Message = byte[]
type T = Message

let private bytesOfInt : int -> byte[] =
    System.BitConverter.GetBytes
let private bytesOfString: string -> array<byte> = 
    Encoding.ASCII.GetBytes

let format (command: Command.T) (payload:string): Message =
    let payload = bytesOfString payload
    let payloadLength = bytesOfInt payload.Length
    Array.concat [ "i3-ipc"B
                   payloadLength
                   Command.toBytes command
                   payload ]