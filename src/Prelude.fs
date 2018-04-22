module FSharp_i3wm.Prelude

let intToBytes: int -> array<byte> = System.BitConverter.GetBytes
let bytesToInt (bytes: array<byte>): int = System.BitConverter.ToInt32(bytes, 0)
let bytesToString: array<byte> -> string = System.Text.Encoding.ASCII.GetString
