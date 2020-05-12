module App

open Fable.Core
open System
open P5

JS.console.log "Hello from Fable!"

type Grid = int array array

let make2DArray cols rows: Grid =
    let arr = Array.create cols Array.empty
    for i in 0 .. cols - 1 do
        arr.[i] <- Array.zeroCreate rows
    arr

let width = 1500.
let height = 1200.
let resolution = 5.
let cols = int (width / resolution)
let rows = int (height / resolution)
let mutable grid: Grid = make2DArray cols rows

JS.console.log (string cols)
JS.console.log (string rows)

// let replaceAtPos (x: Grid) col row newValue: Grid =
//     [| for a in 0 .. (cols - 1) -> [| for b in 0 .. (rows - 1) -> if a = col && b = row then newValue else x.[a].[b] |] |]

let rng = new Random()
let randomCh = fun () -> rng.Next(2)

let countNeighbors (grid: Grid) x y =
    let mutable sum = 0
    for i in -1 .. 1 do
        for j in -1 .. 1 do
            let col = (x + i + cols) % cols
            let row = (y + j + rows) % rows
            sum <- sum + grid.[col].[row]

    sum <- sum - grid.[x].[y]
    sum

let nextGrid (abc: Grid) (x: int) (y: int) state neighbors =
    if state = 0 && neighbors = 3 then abc.[x].[y] <- 1
    else if state = 1 && (neighbors < 2 || neighbors > 3) then abc.[x].[y] <- 0
    else abc.[x].[y] <- state

let init () =
    for i in 0 .. (cols - 1) do
        for j in 0 .. (rows - 1) do
            let rndValue = randomCh ()
            grid.[i].[j] <- rndValue

let sketch (it: p5) =
    it.setup <-
        fun () ->
            //it.frameRate (45)
            it.createCanvas (width, height)
            //it.noLoop ()
            init ()

    it.draw <-
        fun () ->
            it.background (0)
            for i in 0 .. (cols - 1) do
                for j in 0 .. (rows - 1) do
                    let x = float i * resolution
                    let y = float j * resolution
                    if grid.[i].[j] = 1
                    then it.rect (x, y, resolution - 1., resolution - 1.)

            let next = make2DArray cols rows
            for i in 0 .. (cols - 1) do
                for j in 0 .. (rows - 1) do
                    let neighbors = countNeighbors grid i j
                    nextGrid next i j grid.[i].[j] neighbors

            grid <- next

    it.mousePressed <- fun o -> init ()

// draw
p5 (sketch) |> ignore
