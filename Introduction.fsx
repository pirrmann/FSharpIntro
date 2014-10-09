(**
- title : Introduction to F#
- description : Introduction to FSharp
- author : Pierre Irrmann
- theme : Default
- transition : default

***

# Introduction to F#

Pierre Irrmann / @pirrmann

"C# dev, F# addict"

***

![F# Paris User Group](./images/fsharp-ug-paris.png "F# Paris User Group")

@FSharpParis

***

## F# big picture

- Functional-first language that runs on the CLR
- Interoperable with all the exisiting .NET ecosystem
- Very low noise/signal ratio
- Open-source compiler written in F#
- Wonderful community!

---

## Functional?

- Functions and Types over classes
- Purity over mutability
- Composition over inheritance
- Higher order functions over method dispatch
- Options over nulls

[www.notonlyoo.org](http://www.notonlyoo.org)

***

## Why F#?

- Conciseness
- Convenience
- Correctness
- Concurrency
- Completeness

[http://fsharpforfunandprofit.com/why-use-fsharp/](http://fsharpforfunandprofit.com/why-use-fsharp/)

---

### Conciseness

*)
// one-liners
[1..100] |> List.sum |> printfn "sum=%d"

// no curly braces, semicolons or parentheses
let square x = x * x
let sq = square 42 

// simple types in one line
type Person = {First:string; Last:string}

// complex types in a few lines
type Employee = 
  | Worker of Person
  | Manager of Employee list

// type inference
let jdoe = {First="John";Last="Doe"}
let worker = Worker jdoe
(**

---

### Convenience

*)
// automatic equality and comparison
let person1 = {First="john"; Last="Doe"}
let person2 = {First="john"; Last="Doe"}
let text = sprintf "Equal? %A"  (person1 = person2)
(*** include-value: text ***)

// easy composition of functions
let add2times3 = (+) 2 >> (*) 3
let result = add2times3 5
(*** include-value: result ***)
(**

and the REPL!

***

###Isn't F# just for math & compilers?

No.

***

##Main language constructs

- Expressions
- Lists & collections
- Tuples
- Record types
- Functions
- Discriminated Unions
- Options
- Pattern matching
- Classes & Interfaces (yes, really)
- Sequence expressions...
- ... and more!

***

### Basic syntax
#### Let binding
*)
let aValue = "hello world!" // with a cool comment
(**

---
### Basic syntax
#### Type inference for literals

*)
let anInt = 1
let aDouble = 1.
let aDecimal = 2.5M
(**

---
### Basic syntax
#### Immutability is the default

*)
let a = 1
a = 2 // false
let mutable b = 1
b = 2 // still false, what would you expect?
b <- 2
b = 2 // true, now
(**

*)
// a <- 2 doesn't compile, by the way
(**

***
### List and collections (1)

*)
let aList = [1; 2; 3; 4; 5]
let anotherList = 1 :: 2 :: 3 :: 4 :: 5 :: []
let areListEqual = (aList = anotherList)
(**
Are they equal?
*)
(*** include-value: areListEqual ***)
(**

---
### List and collections (2)

*)
let listSyntax = [1; 2; 3]
let arraySyntax = [|1; 2; 3|]
let sequenceSyntax = seq { yield 1; yield 2; yield 3 }
(**

***
### Tuples

*)
let aTuple = 1, 1., "one"
let tupleFirstVal, tupleSecondVal, tupleThirdVal =  aTuple
(*** include-value: tupleFirstVal ***)
(*** include-value: tupleSecondVal ***)
(*** include-value: tupleThirdVal ***)
(**

***
### Record types (1)

*)
type Country = {
    Name: string
    CapitalCity: string
    Population: int
    }
let france = {
    Name = "France"
    CapitalCity = "Paris"
    Population = 66600000
}
(**

---
### Record types (2)

*)
// a new instance
let alternateFrance =  { france with Name = "AltFrance" }
(*** include-value: alternateFrance ***)
(**

***
### Functions (1)
#### (in a functional language, really?)

*)
let format country =
    sprintf "%s has a total poulation of %d" country.Name country.Population

let add x y = x + y
let add2 (x, y) = x + y
(**

---
### Functions (2)
#### Types - currying - partial application

*)
let increasePop increment country =
    { country with Population = country.Population + increment }
let rename newName country =
    { country with Name = newName }

let giveBirth = increasePop 1

let biggerFrance = giveBirth france
(** Welcome to a bigger France! *)
(*** include-value: biggerFrance ***)
(**

---
### Functions (3)
#### Piping - composition

*)
let test1 = france |> increasePop 10000 |> rename "Confédération Française"
(*** include-value: test1 ***)
let composition = increasePop 10000 >> rename "Confédération Française"
let test2 = france |> composition
let areCountryEqual = (test1 = test2)
(**
Are they equal?
*)
(*** include-value: areCountryEqual ***)
(**

***
### Discriminated Unions (1)
#### Like enums...
*)
// This just looks like an enum
type Delivery =
    | Cash
    | Physical
(**

---
### Discriminated Unions (2)
#### Like enums... on steroids
*)
// You can embed values
type ContactType =
    | Phone of string
    | Email of string
(**

---
### Discriminated Unions (3)
#### Like enums... on steroids
*)
// You can embed values... even of different types
type Amount = decimal // simplified model, no currency
type Discount =
    | Percentage of decimal
    | Absolute of Amount
    | Limited of discount:Discount * limit:Amount
(**

---
### Discriminated Unions (4)
#### Like enums... on steroids
*)
// And of course you can design trees ;)
type Tree<'T> =
    | Node of 'T
    | Tree of Tree<'T> * Tree<'T>

let aTree =
    Tree(
        Node 1,
        Tree(
            Tree(
                Tree(Node 2, Node 3),
                Tree(
                    Tree(Node 4, Node 5),
                    Node 6)),
            Tree(
                Node 7,
                Tree(Node 8, Node 9))))
(**

***
### Options (1)
#### Just a Discriminated Union
*)
// defined as
type Option<'T> =
    | Some of 'T
    | None

let aResult = Some "result"
let nothing = None
(**

---
### Options (2)

This might seem simple, but that's the reason why YOU DON'T GET ANY NullReferenceException IN F# !

***

### Pattern-matching (1)
*)

let rec serialize tree =
    match tree with
    | Tree(left, right) -> sprintf "(%s, %s)" (serialize left) (serialize right)
    | Node(value) -> value.ToString()

let serializedTree = serialize aTree
(*** include-value: serializedTree ***)
(**

---
### Pattern-matching (2)
*)

let rec cut tree =
    match tree with
    | Tree(Node _, right) -> cut right
    | Tree(left, Node _) -> cut left
    | Tree(left, right) -> Tree(cut left, cut right)
    | Node(value) -> Node(value)

let cutTree = aTree |> cut |> serialize
(*** include-value: cutTree ***)
(**

***
### Object-oriented constructs (1)
#### Classes
*)

type ParsedValue<'T> (tag:int, value:'T) =
    member x.Tag = tag
    member x.Value = value
    member x.GetValueType() = typeof<'T>
    override x.ToString() = sprintf "{Tag: %d; Value: %A}" tag value

let parsed = new ParsedValue<_>(48, "Z Z4")
(**

--- 
### Object-oriented constructs (2)
#### Interfaces & object expressions
*)

type IMashable =
    abstract Mash: paramName:string -> string

let mashable =
    {
        new IMashable with
            member x.Mash(s:string) = sprintf "***-%s-***" s
    }

(**

***
### Sequence expressions (1)
*)

let seq1 = seq {
    yield 1
    yield 2
    yield 3
    yield 4
}

let seq1Evaluated = seq1 |> Seq.toArray
(*** include-value: seq1Evaluated ***)
(**

---
### Sequence expressions (2)
*)

let seq2 = seq {
    yield 1
    yield! seq1 // yield a full sequence
    yield 2
}

let seq2Evaluated = seq2 |> Seq.toArray
(*** include-value: seq2Evaluated ***)
(**

---
### Sequence expressions (3)
*)

let products = seq {
    for i in seq1 do
    for j in seq1 do
    yield i, j, i * j
}
(*** include-value: products ***)
let productsEvaluated = products |> Seq.toArray
(*** include-value: productsEvaluated ***)
(**

---
### Sequence expressions (4)
*)

let sortedEvenProducts = 
    products
    |> Seq.filter (fun (_, _, x) -> x % 2 = 0)
    |> Seq.sortBy (fun (_, _, x) -> x)

let sortedEvenProductsEvaluated = sortedEvenProducts |> Seq.toArray
(*** include-value: sortedEvenProductsEvaluated ***)
(** 

***

##Many more language constructs & features

- Units of measures
- Active patterns
- Quotations
- Async workflows (async on steroids)
- Mailbox processors (actors)
- Computation expressions
- Type providers...

and that's only F# 3.1, wait for 4.0!

***

##Forgotten bullets

- IDE Integration? (it's getting better all the time)
- Testing story (FsUnit, FsCheck, TickSpec)
- GUI / Web dev?
- Cross-platform & Xamarin
- DDD / Event sourcing (@thinkb4coding)
- ALT.NET Coding breakfast

***

## Questions?

Don't be shy

---

##Resources

- [Try F#](http://www.tryfsharp.org/)
- [The F# software foundation](http://fsharp.org/)
- [F# for fun and profit](http://fsharpforfunandprofit.com/)
- [F# Koans](https://github.com/ChrisMarinos/FSharpKoans/)
- [F# Paris User Group](http://www.meetup.com/Functional-Programming-in-F/)

---

##Contact

@pirrmann

[www.pirrmann.net](http://www.pirrmann.net)

*)