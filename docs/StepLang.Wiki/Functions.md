# STEP Functions

These functions are built-in to the STEP language.

They are categorized by how they interact with the given values and what they return.

## Overview

| Category                  | Description                                                                                        |
|---------------------------|----------------------------------------------------------------------------------------------------|
| [Mutating](#Mutating)     | These functions mutate the given value (`list` or `map`) and usually return nothing (i.e. `void`). |
| [Pure](#Pure)             | These functions do not modify the value given to them and return a new value.                      |
| [Converting](#Converting) | These functions convert the given value to a different type.                                       |
| [Files](#Files)           | For working with files and filesystems.                                                            |
| [Other](#Other)           | These functions do not fit into any of the other categories.                                       |

## Mutating

Mutating functions modify the given value (`list` or `map`) and usually return nothing (i.e. `void`).

| Function                                                                                         | Returns | Description                                                                                                      |
|--------------------------------------------------------------------------------------------------|---------|------------------------------------------------------------------------------------------------------------------|
| [`doAdd(list subject, any element)`](./DoAdd)                                                    | `void`  | Adds the given element to the end of the list.                                                                   |
| [`doRemove(list subject, any element)`](./DoRemove)                                              | `void`  | Removes the first occurrence of the given element from the list.                                                 |
| [`doRemoveAt(list subject, number index)`](./DoRemoveAt)                                         | `void`  | Removes the element at the given index from the list given to it and returns it.                                 |
| [`doInsertAt(list myList, number index, any value)`](./DoInsertAt)                               | `void`  | Inserts the given value into the list at the specified index.                                                    |
| [`doPop(list myList)`](./DoPop)                                                                  | `any`   | Removes the last element from a list and returns it.                                                             |
| [`doShift(list myList)`](./DoShift)                                                              | `any`   | Removes the first element from a list and returns the removed element.                                           |
| [`doSwap(list myList, number a, number b)`<br>`doSwap(map myMap, string a, string b)`](./DoSwap) | `bool`  | Swaps the values of two list indices or two map keys and returns `true` if the values were swapped successfully. |

## Pure

Pure functions can be called with the same arguments and always returns the same value.
They do not modify any of the given values.

| Function                                                                                                                                                                     | Returns                      | Description                                                                                                                                                              |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [`substring(string subject, number start, number length)`](./Substring)                                                                                                      | `string` or `null`           | Takes a string and returns a certain part of the string, based on a start and a length.                                                                                  |
| [`length(string text)`<br>`length(list myList)`<br>`length(map myMap)`](./Length)                                                                                            | `number`                     | Takes a string, list, or map, and returns the count of characters or items.                                                                                              |
| [`indexOf(string subject, string substring)`<br>`indexOf(list subject, any element)`<br>`indexOf(map subject, any value)`](./IndexOf)                                        | `number`, `string` or `null` | Returns the index of the first occurrence of a substring in a string or of an element in a list or the key of the first occurence of a value in a map.                   |
| [`contains(string subject, string substring)`<br>`contains(list subject, any value)`<br>`contains(map subject, any value)`](./Contains)                                      | `bool`                       | Returns `true` if the string, list or map contains the specified value, otherwise `false`.                                                                               |
| [`startsWith(string subject, string prefix)`](./StartsWith)                                                                                                                  | `bool`                       | Returns `true` if the string starts with the specified value.                                                                                                            |
| [`endsWith(string subject, string suffix)`](./EndsWith)                                                                                                                      | `bool`                       | Returns `true` if the string ends with the specified value.                                                                                                              |
| [`converted(list subject, function callback)`](./Converted)                                                                                                                  | `list`                       | Returns a new list with the elements converted using the given callback.                                                                                                 |
| [`filtered(list subject, function callback)`](./Filtered)                                                                                                                    | `list`                       | Returns a list of items that match a given condition.                                                                                                                    |
| [`reversed(list subject)`<br>`reversed(string subject)`](./Reversed)                                                                                                         | `list` or `string`           | Returns a new list with the elements in reverse order.                                                                                                                   |
| [`sorted(list list, function compare = compareTo)`](./Sorted)                                                                                                                | `list`                       | Takes a list and returns a new list with the same elements but sorted in ascending order.                                                                                |
| [`compareTo(number a, number b)`<br>`compareTo(string a, string b)`<br>`compareTo(bool a, bool b)`<br>`compareTo(list a, list b)`<br>`compareTo(map a, map b)`](./CompareTo) | `number`                     | Takes two arguments of the same type and returns a number indicating whether the first argument is less than, equal to, or greater than compared to the second argument. |
| [`sin(number x)`](./Sin)<br>[`cos(number x)`](./Cos)<br>[`tan(number x)`](./Tan)                                                                                             | `number`                     | Takes a number and returns the sine, cosine or tangent of the number.                                                                                                    |
| [`min(number ...x)`](./Min)<br>[`max(number ...x)`](./Max)                                                                                                                   | `number`                     | Returns the smallest/biggest value from a list of numbers.                                                                                                               |
| [`clamp(number min, number max, number x)`](./Clamp)                                                                                                                         | `number`                     | Returns a value that is clamped between two values.                                                                                                                      |
| [`interpolate(number a, number b, number t)`](./Interpolate)                                                                                                                 | `number`                     | Calculates a linear interpolation between two values based on a delta.                                                                                                   |
| [`abs(number x)`](./Abs)                                                                                                                                                     | `number`                     | Returns the absolute value of a number.                                                                                                                                  |
| [`floor(number x)`](./Floor)                                                                                                                                                 | `number`                     | Returns the largest integer smaller than or equal to a number.                                                                                                           |
| [`ceil(number x)`](./Ceil)                                                                                                                                                   | `number`                     | Returns the smallest integer greater than or equal to a number.                                                                                                          |
| [`round(number x)`](./Round)                                                                                                                                                 | `number`                     | Rounds a number to the nearest integer.                                                                                                                                  |
| [`sqrt(number x)`](./Sqrt)                                                                                                                                                   | `number`                     | Returns the square root of a number.                                                                                                                                     |
| [`clone(any subject)`](./Clone)                                                                                                                                              | `any`                        | Can be used to create a copy of a value.                                                                                                                                 |
| [`isset(string variableName)`](./Isset)                                                                                                                                      | `bool`                       | Checks if a given variable name exists in the current scope.                                                                                                             |
| [`range(number start, number end, number step = 1)`](./Range)                                                                                                                | `list`                       | Returns a list of numbers from `start` towards `end`, stepping by `step` and including the end value when the step lands on it.                                          |

## Converting

Converting functions convert the given value to a different type or retrieve an emerging property of the given value.

| Function                                                  | Returns            | Description                                                   |
|-----------------------------------------------------------|--------------------|---------------------------------------------------------------|
| [`fromJson(string json)`](./FromJson)                     | `any`              | Converts the given JSON string to a STEP value.               |
| [`toBool(any subject)`](./ToBool)                         | `bool`             | Converts the given value to a bool.                           |
| [`toJson(any subject)`](./ToJson)                         | `string`           | Converts the given value to a JSON string.                    |
| [`toKeys(map subject)`](./ToKeys)                         | `list`             | Returns a list of keys from the given map.                    |
| [`toNumber(string value, number radix = 10)`](./ToNumber) | `number` or `null` | Converts the given string to a number.                        |
| [`toRadix(number value, number radix = 10)`](./ToRadix)   | `string`           | Converts the given number to a string in the specified radix. |
| [`toString(any subject)`](./ToString)                     | `string`           | Converts the given value to a string.                         |
| [`toTypeName(any subject)`](./ToTypeName)                 | `string`           | Returns the type name of the given value as a string.         |
| [`toValues(map subject)`](./ToValues)                     | `list`             | Returns a list of values from the given map.                  |

## Web

Functions for working with the web and web technologies.

| Function                                                                                                                 | Returns  | Description                                                                                                             |
|--------------------------------------------------------------------------------------------------------------------------|----------|-------------------------------------------------------------------------------------------------------------------------|
| [`fetch(string url, map options)`](./Fetch)                                                                              | `string` | Makes a request to a URL and returns the response as a string or `null` if the request failed.                          |
| [`httpServer(number port, function requestHandler)`<br>`httpServer(map options, function requestHandler)`](./HttpServer) | `void`   | Starts a HTTP server on the specified port/with the specified options and calls the callback for each incoming request. |
| [`fileResponse(string path)`](./FileResponse)                                                                            | `map`    | Reads the file contents and returns a map that can be returned from a `httpServer` request handler.                     |

## Files

Functions for working with files and filesystems.

| Function                                                                     | Returns  | Description                                         |
|------------------------------------------------------------------------------|----------|-----------------------------------------------------|
| [`fileRead(string path)`](./FileRead)                                        | `string` | Reads the file contents and returns it as a string. |
| [`fileExists(string path)`](./FileExists)                                    | `bool`   | Returns `true` if the file exists.                  |
| [`fileWrite(string path, string content, bool append = false)`](./FileWrite) | `void`   | Writes the given contents to the file.              |
| [`fileDelete(string path)`](./FileDelete)                                    | `void`   | Deletes the file.                                   |

## Other

Other functions do not fit into any of the other categories.

| Function                             | Returns  | Description                                                       |
|--------------------------------------|----------|-------------------------------------------------------------------|
| [`random()`](./Random)               | `number` | Returns a random number between 0 and 1.                          |
| [`seed()`](./Seed)                   | `number` | Initializes the random number generator.                          |
| [`print(any ...value)`](./Print)     | `void`   | Writes a value to `StdOut`.                                       |
| [`println(any ...value)`](./Println) | `void`   | writes a value to `StdOut` and appends a new line.                |
| [`readline()`](./Readline)           | `string` | Reads a line from `StdIn` and returns it as a string.             |
| [`read()`](./Read)                   | `string` | Reads a single character from `StdIn` and returns it as a string. |
