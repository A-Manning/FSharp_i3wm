module FSharp_i3wm.Command

open FSharp_i3wm.Prelude
open FSharpx.Functional.Prelude

type T =
    | RUN_COMMAND = 0
    | GET_WORKSPACES = 1
    | SUBSCRIBE = 2
    | GET_OUTPUTS = 3
    | GET_TREE = 4
    | GET_MARKS = 5
    | GET_BAR_CONFIG = 6
    | GET_VERSION = 7
    | GET_BINDING_MODES = 8
    | GET_CONFIG = 9
    | SEND_TICK = 10

type Command = T

let toInt: Command -> int = int 
let ofInt: int -> Command = enum

let toBytes: Command -> array<byte> =
    toInt >> intToBytes
let ofBytes: array<byte> -> Command =
    enum << bytesToInt
