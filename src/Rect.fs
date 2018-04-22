module FSharp_i3wm.Rect

open FSharp.Data
open FSharp.Data.JsonExtensions

type T = {
    x : int64
    y : int64
    width : int64
    height : int64
}

type Rect = T

let getX      (rect: Rect): int64 = rect.x
let getY      (rect: Rect): int64 = rect.y
let getWidth  (rect: Rect): int64 = rect.width
let getHeight (rect: Rect): int64 = rect.height

let parseJson (rect:JsonValue) : Rect =
    { x = rect?x.AsInteger64()
      y = rect?y.AsInteger64()
      width = rect?width.AsInteger64()
      height = rect?height.AsInteger64() }
