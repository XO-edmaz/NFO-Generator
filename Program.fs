module FSI =
    [<System.Runtime.InteropServices.DllImport("user32.dll")>]
    extern bool ShowWindow(nativeint hWnd, int flags)
    let HideConsole() = 
        let proc = System.Diagnostics.Process.GetCurrentProcess()
        ShowWindow(proc.MainWindowHandle, 0)

open System
open System.IO
open System.Drawing
open System.Windows.Forms


FSI.HideConsole() |> ignore


let Form = new Form(BackColor = Color.White, Height = 255, Icon = new Icon("AppIcon.ico"), Text = "NFO Generator")
let Browse = new Button(Text = "Browse", Location = new Point (10, 10))
let OK = new Button(Text = "Create NFO", Location = new Point(10, Browse.Location.Y + 30))
let Quit = new Button(Text = "Quit", Location = new Point(10, OK.Location.Y + 30))
let Copy = new Label(Text = "by edmaz, 2020", AutoSize = true, ForeColor = Color.DarkGray, Location = new Point(200, 200))
let Exp = new FolderBrowserDialog(RootFolder = Environment.SpecialFolder.UserProfile, ShowNewFolderButton = false)
let NFOGen = new Label(Text = "NFO Generator", AutoSize = true, Font = new Font("Microsoft Sans Serif", 16.0f), Location = new Point(Browse.Size.Width + 20, 10))
let ForFL = new Label(Text = "For FL Studio", AutoSize = true, Font = new Font("Microsoft Sans Serif", 8.0f), Location = new Point(Browse.Size.Width + 23, 35))
let Version = new Label(Text = "v 1.0.0", Location = new Point(0, 200))
let Done = new Label(Text = "Done!", Location = new Point(10, Quit.Location.Y + 30))


let (Controls : Control[])= [|OK; Browse; Quit; Copy; ForFL; NFOGen; Version|]
Form.Controls.AddRange(Controls)


let GetFilesArray (s : string) = 
    Directory.GetFiles(s)
    |> Array.map (fun x -> x.[s.Length..x.Length-5])
    |> Array.distinct
let CreateNFO (s : string) =
    let Path = Exp.SelectedPath + s + ".nfo"
    let FS = File.CreateText(Path)
    FS.Write("Bitmap="+s+".png")
    FS.Close()
    File.SetAttributes(Path, FileAttributes.Hidden)


do Browse.Click.Add (fun _ -> 
    let DialogRes = Exp.ShowDialog (Form)
    ()
)
do Quit.Click.Add (fun _ -> 
    Application.Exit ()
    ()
)
do OK.Click.Add (fun _ -> 
    GetFilesArray Exp.SelectedPath |> Array.iter CreateNFO
    Form.Controls.Add(Done)
    ()
)


[<STAThread>]
do Application.Run(Form)

