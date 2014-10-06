#load @"packages\FsReveal.0.0.5-beta\fsreveal\fsreveal.fsx"
open FsReveal

open System.IO
let inputFsx = Path.Combine( __SOURCE_DIRECTORY__, "Introduction.fsx")

FsReveal.GenerateOutputFromScriptFile __SOURCE_DIRECTORY__ "Introduction.html" inputFsx