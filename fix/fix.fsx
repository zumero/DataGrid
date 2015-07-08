
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
    printfn "%s" path
    let lines = File.ReadAllLines (path)
    for i in [0 .. lines.Length-1] do
        let s = lines.[i]
        if s.IndexOf("HintPath") >= 0 then 
            // portable-win+net45+wp80+MonoAndroid10+MonoTouch10
            if s.IndexOf("/ios/Zumero.DataGrid.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\..\..\lib\ios\Zumero.DataGrid.dll</HintPath>"
            else if s.IndexOf("/ios/Zumero.DataGrid.iOS.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\..\..\lib\ios\Zumero.DataGrid.iOS.dll</HintPath>"
            else if s.IndexOf(@"\ios\Xamarin.Forms.Core.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Forms.1.2.3.6257\lib\MonoTouch10\Xamarin.Forms.Core.dll</HintPath>"
            else if s.IndexOf(@"\ios\Xamarin.Forms.Xaml.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Forms.1.2.3.6257\lib\MonoTouch10\Xamarin.Forms.Xaml.dll</HintPath>"
            else if s.IndexOf(@"\ios\Xamarin.Forms.Platform.iOS.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Forms.1.2.3.6257\lib\MonoTouch10\Xamarin.Forms.Platform.iOS.dll</HintPath>"
            else if s.IndexOf("/android/Zumero.DataGrid.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\..\..\lib\android\Zumero.DataGrid.dll</HintPath>"
            else if s.IndexOf("/android/Zumero.DataGrid.Android.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\..\..\lib\android\Zumero.DataGrid.Android.dll</HintPath>"
            else if s.IndexOf(@"\android\Xamarin.Forms.Core.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Forms.1.2.3.6257\lib\MonoAndroid10\Xamarin.Forms.Core.dll</HintPath>"
            else if s.IndexOf(@"\android\Xamarin.Forms.Xaml.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Forms.1.2.3.6257\lib\MonoAndroid10\Xamarin.Forms.Xaml.dll</HintPath>"
            else if s.IndexOf(@"\android\Xamarin.Forms.Platform.Android.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Forms.1.2.3.6257\lib\MonoAndroid10\Xamarin.Forms.Platform.Android.dll</HintPath>"
            else if s.IndexOf(@"\android\Xamarin.Android.Support.v13.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Android.Support.v13.20.0.0.4\lib\MonoAndroid32\Xamarin.Android.Support.v13.dll</HintPath>"
            else if s.IndexOf(@"\android\Xamarin.Android.Support.v4.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Android.Support.v4.20.0.0.4\lib\MonoAndroid32\Xamarin.Android.Support.v4.dll</HintPath>"
            else if s.IndexOf(@"\android\FormsViewGroup.dll") >= 0 then
                lines.[i] <- @"      <HintPath>..\packages\Xamarin.Forms.1.2.3.6257\lib\MonoAndroid10\FormsViewGroup.dll</HintPath>"
            if s <> lines.[i] then
                printfn "    FIXED: %s" s
                printfn "      now: %s" (lines.[i])
            else
                printfn "       ok: %s" s
    File.WriteAllLines(path, lines)

allFilesUnder Environment.CurrentDirectory
|> Seq.filter (function 
            | EndsWith ".csproj" _
                -> true
            | _ -> false)
|> Seq.iter foo

