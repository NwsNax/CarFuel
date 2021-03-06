﻿//these are similar to C# using statements
open canopy
open runner
open System
let baseUrl = "http://localhost:2462" 
let userEmail = "user" + DateTime.Now.Ticks.ToString() + "@company.com"
let pwd = "Test999/*"
chromeDir <- "C:\\chromedriver" 
start chrome 


"Annonymous Users cannot add a new car" &&& fun _ ->
    url (baseUrl+ "/Cars")
    notDisplayed "a[href='/Cars/Add']"

"Sign Up" &&& fun _ ->
    url (baseUrl + "/Account/Register")
    "#Email" << userEmail
    "#Password" << pwd
    "#ConfirmPassword" << pwd
    click "input[type=submit]"
    on baseUrl

"Log in" &&& fun _ ->
    url (baseUrl + "/Account/Login")
    "#Email" << userEmail
    "#Password" << pwd
    click "input[type=submit]"
    on baseUrl
     

"Click add link then go to create page" &&& fun _ ->
    url (baseUrl + "/Cars")
    displayed "a[href='/Cars/Add']"
    click "a[href='/Cars/Add']"
    on (baseUrl + "/Cars/Add")

"New User can add a car and see it on the list page" &&& fun _ ->    
    url (baseUrl + "/Cars/Add")
    "input#Name" << "Accord" + userEmail    
    click "button[type=submit]"
    on (baseUrl + "/Cars")

    "span#total" == "1"
    "div.car-name"=="Accord" + userEmail
    

//***
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()