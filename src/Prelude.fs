module FSharp_i3wm.Prelude

let mapFst (f: 'A -> 'C) (a: 'A, b: 'B): 'C * 'B = f a, b
let mapSnd (f: 'B -> 'C) (a: 'A, b: 'B): 'A * 'C = a, f b
let duplicate (x: 'A): 'A * 'A = x, x

let intToBytes: int -> array<byte> = System.BitConverter.GetBytes
let bytesToInt (bytes: array<byte>): int = System.BitConverter.ToInt32(bytes, 0)
let bytesToString: array<byte> -> string = System.Text.Encoding.ASCII.GetString
