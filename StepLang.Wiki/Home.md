[releases page]: https://github.com/ricardoboss/STEP/releases
[.NET 7]: https://dotnet.microsoft.com/download/dotnet/7.0
[Visual Studio Code]: https://code.visualstudio.com/
[Language Reference]: ./Language-Reference
[Help]: ./Help
[Discord server]: https://discord.gg/ySpmcdCqFN

To get started using STEP, you first need to set up your computer.

Download the latest version of STEP from the [releases page].
You can either download the `STEP.exe` file or the `STEP.zip` file.

> **Note**
> If you download the `STEP.zip` file, you will need to extract it before you can use it.
> Also, you need to have [.NET 7] or higher installed on your computer.
> Then, instead of running `STEP.exe`, you need to run `dotnet STEP.dll`.

Remember the location of the STEP.exe file, because you will need it later.

# Your first STEP program

To create your first STEP program, you need to create a new file with the `.step` extension, for example `hello.step`.
You can do this with any text editor, for example [Visual Studio Code].

In this file, you can write your first STEP program:

```step
println("Hello, world!")
```

To run this program, you need to open a terminal in the current folder.
Then, you can run the following command:

```bash
STEP.exe run hello.step
```

> You might need to replace `STEP.exe` with the path to the STEP executable on your computer.

This will run your program and print `Hello, world!` to the terminal.

**Congratulations!** You have just written and run your first STEP program!

# Next steps

Now that you have written your first STEP program, you can learn more about the language in the [Language Reference].

If you run into any problems, check out the [Help] page of the wiki or ask for help on @ricardoboss' [Discord server] in the #step channel.
