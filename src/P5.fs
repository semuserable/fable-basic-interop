module P5

open Fable.Core

(* p5.js *)
// p5.js interface
[<StringEnum>]
type Renderer =
    | [<CompiledName("p2d")>] P2D
    | [<CompiledName("webgl")>] WebGL

[<Import("*", "p5/lib/p5.js")>]
type p5(?sketch: p5 -> unit, ?id: string) =
    member __.setup
        with set (v: unit -> unit): unit = jsNative

    member __.draw
        with set (v: unit -> unit): unit = jsNative

    member __.mousePressed
        with set (?event: obj -> unit): unit = jsNative

    member __.createCanvas(w: float, h: float, ?renderer: Renderer): unit = jsNative
    member __.background(value: int): unit = jsNative
    member __.millis(): float = jsNative
    member __.rotateX(angle: float): unit = jsNative
    member __.box(): unit = jsNative
    member __.frameRate(value: int): unit = jsNative
    member __.noLoop(): unit = jsNative
    member __.redraw(?n: int): unit = jsNative

    member __.rect(x: float, y: float, w: float, ?h: float, ?tl: float, ?tr: float, ?br: float, ?bl: float,
                   ?detailX: int, ?detailY: int): unit = jsNative
