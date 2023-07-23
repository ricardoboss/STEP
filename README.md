> HILFE = HILFE Interpreted Language For Education

Behold, a new programming language arrives:

```
double pi = 3.1415
string name = "John"

if (name == "John") {
    print("Hello, $name", EOL)
} else {
    print("Who are you?", EOL)
}

print("Guess PI: ", EOL)
string guess
double parsedGuess
while (guess = readline()) {
    parsedGuess = parseDouble(guess)
    if (parsedGuess == null) {
        print("Enter a valid double!", EOL)

        continue
    }

    if (parsedGuess == pi) {
        print("You got it!", EOL)
        break
    } elseif (guess > 3 || guess < 3) {
        print("Way off!", EOL, "Guess PI:")
    } else {
        print("Nope, try again", EOL, "Guess PI: ")
    }
}

print("Goodbye", EOL)
```

# FAQ

## What is this?

A programming language.

## Why is this?

I want to learn how to create a programming language.

## What does it do better than other languages?

Nothing. It's probably worse than existing ones too.

## What can it do?

At the moment, not much. Although it is Turing complete, meaning you _could_ do anything with it.

## Can I use it?

Sure. But you should probably use a real programming language, designed by people who know what they do.

## Is it fast?

No.

## Can I contribute?

Sure, open a PR with your feature and tests.
If its something I want to develop myself however, I might not merge it.
This is because, well, I want to learn it myself.

## Why is this open source?

Why should it be private?

## Are you going to maintain it?

Probably not. Its just a small project to learn about programming languages.

# License

Copyright 2023 Ricardo Boss

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


