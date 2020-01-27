module App

open Fable.Core

JS.console.log "Hello from Fable!"

(* window.alert *)

// interface
type Window =
    // function description
    abstract alert: ?message: string -> unit

// wiring-up JavaScript and F# with [<Global>] and jsNative
let [<Global>] window: Window = jsNative

// client calls
window.alert ("Global Fable window.alert")
window.alert "Global Fable window.alert without parentheses"

[<Emit("window.alert($0)")>]
let alert (message: string): unit = jsNative

alert ("Emit from Fable window.alert")
alert "Emit from Fable window.alert without parentheses"
"Emit from Fable window.alert with F# style" |> alert 

(* Math.random *)

// interface
type Math =
    abstract random: unit -> float

let [<Global>] Math: Math = jsNative

// client call
JS.console.log (Math.random())

// emit
[<Emit("Math.random()")>]
let random(): float = jsNative

JS.console.log (random())

(* DOM *)

// interfaces
type Node =
    abstract appendChild: child: Node -> Node
    abstract insertBefore: node: Node * ?child: Node -> Node

type Document =
    abstract createElement: tagName: string -> Node
    abstract createTextNode: data: string -> Node
    abstract getElementById: elementId: string -> Node
    abstract body: Node with get, set

let [<Global>] document: Document = jsNative

// client code
let newDiv = document.createElement("div")

"Good news everyone! Generated dynamically by Fable!"
|> document.createTextNode
|> newDiv.appendChild
|> ignore

let currentDiv = document.getElementById("app")
document.body.insertBefore (newDiv, currentDiv) |> ignore

(* p5.js *)
// p5.js interface
[<StringEnum>]
type Renderer =
    | [<CompiledName("p2d")>] P2D
    | [<CompiledName("webgl")>] WebGL
        
type [<Import("*", "p5/lib/p5.js")>] p5(?sketch: p5 -> unit, ?id: string) =    
    member __.setup with set(v: unit -> unit): unit = jsNative
    member __.draw with set(v: unit -> unit): unit = jsNative
    member __.createCanvas(w: float, h: float, ?renderer: Renderer): unit = jsNative
    member __.background(value: int): unit = jsNative
    member __.millis(): float = jsNative
    member __.rotateX(angle: float): unit = jsNative
    member __.box(): unit = jsNative

// client code
let sketch (it: p5) =
    it.setup <- fun () -> it.createCanvas(300., 300., WebGL)
    it.draw <- fun () ->
        it.background(255)
        it.rotateX(it.millis() / 1000.)
        it.box()

// draw    
p5(sketch) |> ignore