<img src="docs/logo.svg" align="left" alt="STEP" width="120" height="120">

<p>
  <h1>STEP</h1>
  <span>
    <strong>S</strong><span>imple</span>
    <strong>T</strong><span>ransition</span>
    <span>to</span>
    <strong>E</strong><span>levated</span>
    <strong>P</strong><span>rogramming</span>
  </span>
</p>

Behold, a new programming language arrives:

```step
number pi = 3.141592

string name = "John"
if (name == "John") {
	println("Hello, ", name)
} else {
	println("Who are you?")
}

string guess
number parsedGuess
number tries = 0
while (tries <= 3) {
	print("Guess PI: ")
	guess = readline()
	parsedGuess = parse("number", guess)
	if (parsedGuess == null) {
		println("Enter a valid number!")

		tries++

		continue
	}

	if (parsedGuess == pi) {
		println("Close enough!")
		break
	} else {
		if (parsedGuess > 4 || parsedGuess < 3) {
			println("Way off! ", 3 - tries, " tries left.")
		} else {
			println("Nope, try again. ", 3 - tries, " tries left.")
		}
	}

	tries++
}

print("Goodbye", EOL)
```

To get started yourself, head over to the [Wiki](https://github.com/ricardoboss/STEP/wiki)

## What is this?

This repository contains an interpreter and command line interface for the STEP programming language.

## What is STEP?

STEP is a programming language designed to be simple and easy to learn.
Is is a statically typed language with a C-like syntax.

## Why is this?

The main reason this exists is because I want to learn about programming languages and how they are designed.
Another reason was my desire to create a language that can be taught to people who have never programmed before.

## What does it do better than other languages?

It uses very little keywords and not many symbols, so rookie programmers can focus on the important stuff.
Like many other languages, it also uses C-like syntax, which is very common and prepares new programmers for other,
more advanced languages.

## How can I learn it?

Take a look at the [`StepLang/Examples`](./StepLang/Examples) folder.
It contains a few examples of STEP code, which you can run using the interpreter.

Tutorials are planned in https://github.com/ricardoboss/STEP/issues/29.

If you ever need help, feel free to join @ricardoboss' [Discord server](https://discord.gg/ySpmcdCqFN).

## Can I contribute?

Of course!
Make sure to read [`CONTRIBUTING.md`](./CONTRIBUTING.md) and make yourself familiar with the workflow.

Check out the [`good first issue` issues](https://github.com/ricardoboss/STEP/contribute) to get started.

## License

This project is licensed under the MIT license.
For more information, read [`LICENSE.md`](./LICENSE.md).
