# STEP Functions

These functions are built-in to the STEP language.

They are categorized by how they interact with the given values and what they return.

## Overview

| Category                  | Description                                                                                        |
|---------------------------|----------------------------------------------------------------------------------------------------|
| [Mutating](#Mutating)     | These functions mutate the given value (`list` or `map`) and usually return nothing (i.e. `void`). |
| [Pure](#Pure)             | These functions do not modify the value given to them and return a new value.                      |
| [Converting](#Converting) | These functions convert the given value to a different type.                                       |
| [Other](#Other)           | These functions do not fit into any of the other categories.                                       |

## Mutating

Mutating functions modify the given value (`list` or `map`) and usually return nothing (i.e. `void`).

| Function                                                                                                      | Returns | Description                                                                                                      |
|---------------------------------------------------------------------------------------------------------------|---------|------------------------------------------------------------------------------------------------------------------|
| [`doAdd(list subject, any element)`](./Functions/DoAdd)                                                    | `void`  | Adds the given element to the end of the list.                                                                   |
| [`doRemove(list subject, any element)`](./Functions/DoRemove)                                              | `void`  | Removes the first occurrence of the given element from the list.                                                 |
| [`doRemoveAt(list subject, number index)`](./Functions/DoRemoveAt)                                         | `void`  | Removes the element at the given index from the list given to it and returns it.                                 |
| [`doInsertAt(list myList, number index, any value)`](./Functions/DoInsertAt)                               | `void`  | Inserts the given value into the list at the specified index.                                                    |
| [`doPop(list myList)`](./Functions/DoPop)                                                                  | `any`   | Removes the last element from a list and returns it.                                                             |
| [`doShift(list myList)`](./Functions/DoShift)                                                              | `any`   | Removes the first element from a list and returns the removed element.                                           |
| [`doSwap(list myList, number a, number b)`<br>`doSwap(map myMap, string a, string b)`](./Functions/DoSwap) | `bool`  | Swaps the values of two list indices or two map keys and returns `true` if the values were swapped successfully. |

## Pure

Pure functions can be called with the same arguments and always returns the same value.
They do not modify any of the given values.

| Function                                                                                                                                                                                  | Returns                      | Description                                                                                                                                                              |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [`substring(string subject, number start, number length)`](./Functions/Substring)                                                                                                      | `string` or `null`           | Takes a string and returns a certain part of the string, based on a start and a length.                                                                                  |
| [`length(string text)`<br>`length(list myList)`<br>`length(map myMap)`](./Functions/Length)                                                                                            | `number`                     | Takes a string, list, or map, and returns the count of characters or items.                                                                                              |
| [`indexOf(string subject, string substring)`<br>`indexOf(list subject, any element)`<br>`indexOf(map subject, any value)`](./Functions/IndexOf)                                        | `number`, `string` or `null` | Returns the index of the first occurrence of a substring in a string or of an element in a list or the key of the first occurence of a value in a map.                   |
| [`contains(string subject, string substring)`<br>`contains(list subject, any value)`<br>`contains(map subject, any value)`](./Functions/Contains)                                      | `bool`                       | Returns `true` if the string, list or map contains the specified value, otherwise `false`.                                                                               |
| [`startsWith(string subject, string prefix)`](./Functions/StartsWith)                                                                                                                  | `bool`                       | Returns `true` if the string starts with the specified value.                                                                                                            |
| [`endsWith(string subject, string suffix)`](./Functions/EndsWith)                                                                                                                      | `bool`                       | Returns `true` if the string ends with the specified value.                                                                                                              |
| [`converted(list subject, function callback)`](./Functions/Converted)                                                                                                                  | `list`                       | Returns a new list with the elements converted using the given callback.                                                                                                 |
| [`filtered(list subject, function callback)`](./Functions/Filtered)                                                                                                                    | `list`                       | Returns a list of items that match a given condition.                                                                                                                    |
| [`reversed(list subject)`](./Functions/Reversed)                                                                                                                                       | `list`                       | Returns a new list with the elements in reverse order.                                                                                                                   |
| [`sorted(list list, function compare = compareTo)`](./Functions/Sorted)                                                                                                                | `list`                       | Takes a list and returns a new list with the same elements but sorted in ascending order.                                                                                |
| [`compareTo(number a, number b)`<br>`compareTo(string a, string b)`<br>`compareTo(bool a, bool b)`<br>`compareTo(list a, list b)`<br>`compareTo(map a, map b)`](./Functions/CompareTo) | `number`                     | Takes two arguments of the same type and returns a number indicating whether the first argument is less than, equal to, or greater than compared to the second argument. |
| [`sin(number x)`](./Functions/Sin)<br>[`cos(number x)`](./Functions/Cos)<br>[`tan(number x)`](./Functions/Tan)                                                                   | `number`                     | Takes a number and returns the sine, cosine or tangent of the number.                                                                                                    |
| [`min(number ...x)`](./Functions/Min)<br>[`max(number ...x)`](./Functions/Max)                                                                                                      | `number`                     | Returns the smallest/biggest value from a list of numbers.                                                                                                               |
| [`clamp(number min, number max, number x)`](./Functions/Clamp)                                                                                                                         | `number`                     | Returns a value that is clamped between two values.                                                                                                                      |
| [`interpolate(number a, number b, number t)`](./Functions/Interpolate)                                                                                                                 | `number`                     | Calculates a linear interpolation between two values based on a delta.                                                                                                   |
| [`abs(number x)`](./Functions/Abs)                                                                                                                                                     | `number`                     | Returns the absolute value of a number.                                                                                                                                  |
| [`floor(number x)`](./Functions/Floor)                                                                                                                                                 | `number`                     | Returns the largest integer smaller than or equal to a number.                                                                                                           |
| [`ceil(number x)`](./Functions/Ceil)                                                                                                                                                   | `number`                     | Returns the smallest integer greater than or equal to a number.                                                                                                          |
| [`round(number x)`](./Functions/Round)                                                                                                                                                 | `number`                     | Rounds a number to the nearest integer.                                                                                                                                  |
| [`sqrt(number x)`](./Functions/Sqrt)                                                                                                                                                   | `number`                     | Returns the square root of a number.                                                                                                                                     |
| [`clone(any subject)`](./Functions/Clone)                                                                                                                                              | `any`                        | Can be used to create a copy of a value.                                                                                                                                 |

## Converting

Converting functions convert the given value to a different type or retrieve an emerging property of the given value.

| Function                                                               | Returns            | Description                                                   |
|------------------------------------------------------------------------|--------------------|---------------------------------------------------------------|
| [`fromJson(string json)`](./Functions/FromJson)                     | `any`              | Converts the given JSON string to a STEP value.               |
| [`toBool(any subject)`](./Functions/ToBool)                         | `bool`             | Converts the given value to a bool.                           |
| [`toJson(any subject)`](./Functions/ToJson)                         | `string`           | Converts the given value to a JSON string.                    |
| [`toKeys(map subject)`](./Functions/ToKeys)                         | `list`             | Returns a list of keys from the given map.                    |
| [`toNumber(string value, number radix = 10)`](./Functions/ToNumber) | `number` or `null` | Converts the given string to a number.                        |
| [`toRadix(number value, number radix = 10)`](./Functions/ToRadix)   | `string`           | Converts the given number to a string in the specified radix. |
| [`toString(any subject)`](./Functions/ToString)                     | `string`           | Converts the given value to a string.                         |
| [`toTypeName(any subject)`](./Functions/ToTypeName)                 | `string`           | Returns the type name of the given value as a string.         |
| [`toValues(map subject)`](./Functions/ToValues)                     | `list`             | Returns a list of values from the given map.                  |

## Other

Other functions do not fit into any of the other categories.

| Function                                          | Returns  | Description                                                       |
|---------------------------------------------------|----------|-------------------------------------------------------------------|
| [`random()`](./Functions/Random)               | `number` | Returns a random number between 0 and 1.                          |
| [`seed()`](./Functions/Seed)                   | `number` | Initializes the random number generator.                          |
| [`print(any ...value)`](./Functions/Print)     | `void`   | Writes a value to `StdOut`.                                       |
| [`println(any ...value)`](./Functions/Println) | `void`   | writes a value to `StdOut` and appends a new line.                |
| [`readline()`](./Functions/Readline)           | `string` | Reads a line from `StdIn` and returns it as a string.             |
| [`read()`](./Functions/Read)                   | `string` | Reads a single character from `StdIn` and returns it as a string. |
