
open System
open System.IO

let rec allFilesUnder baseFolder = 
    seq {
        yield! Directory.GetFiles(baseFolder)
        for subDir in Directory.GetDirectories(baseFolder) do
            yield! allFilesUnder subDir 
        }
    
let (|EndsWith|_|) extension (file : string) = 
    if file.EndsWith(extension) 
    then Some() 
    else None

let foo path =
    let lines = File.ReadAllLines (path)
    for i in [0 .. lines.Length-1] do
        let s = lines.[i].Replace("1.3.1.6296", "1.4.0.6341");
        if s <> lines.[i] then
            lines.[i] <- s
            printfn "%s:%d" path i
    File.WriteAllLines(path, lines)

allFilesUnder Environment.CurrentDirectory
|> Seq.filter (function 
            | EndsWith ".csproj" _ -> true
            | EndsWith ".config" _ -> true
            | EndsWith ".yaml" _ -> true
            | _ -> false)
|> Seq.iter foo

