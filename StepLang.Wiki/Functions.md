# STEP Functions

These functions are built-in to the STEP language.

They are categorized by how they interact with the given values and what they return.

## Overview

| Category                  | Description                                                                                        |
|---------------------------|----------------------------------------------------------------------------------------------------|
| [Mutating](#Mutating)     | These functions mutate the given value (`list` or `map`) and usually return nothing (i.e. `void`). |
| [Pure](#Pure)             | These functions do not modify the value given to them and return a new value.                      |
| [Converting](#Converting) | These functions convert the given value to a different type.                                       |

## Mutating

| Function                                                                                                      | Returns | Description                                                                                                      |
|---------------------------------------------------------------------------------------------------------------|---------|------------------------------------------------------------------------------------------------------------------|
| [`doAdd(list subject, any element)`](./Functions/DoAdd.md)                                                    | `void`  | Adds the given element to the end of the list.                                                                   |
| [`doRemove(list subject, any element)`](./Functions/DoRemove.md)                                              | `void`  | Removes the first occurrence of the given element from the list.                                                 |
| [`doRemoveAt(list subject, number index)`](./Functions/DoRemoveAt.md)                                         | `void`  | Removes the element at the given index from the list given to it and returns it.                                 |
| [`doInsertAt(list myList, number index, any value)`](./Functions/DoInsertAt.md)                               | `void`  | Inserts the given value into the list at the specified index.                                                    |
| [`doPop(list myList)`](./Functions/DoPop.md)                                                                  | `any`   | Removes the last element from a list and returns it.                                                             |
| [`doShift(list myList)`](./Functions/DoShift.md)                                                              | `any`   | Removes the first element from a list and returns the removed element.                                           |
| [`doSwap(list myList, number a, number b)`<br>`doSwap(map myMap, string a, string b)`](./Functions/DoSwap.md) | `bool`  | Swaps the values of two list indices or two map keys and returns `true` if the values were swapped successfully. |

## Pure

| Function                                                                                                                                                                                  | Returns | Description |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------|-------------|
| [`substring(string subject, number start, number length)`](./Functions/Substring.md)                                                                                                      |         |             |
| [`length(string text)`<br>`length(list myList)`<br>`length(map myMap)`](./Functions/Length.md)                                                                                            |         |             |
| [`indexOf(string subject, string substring)`<br>`indexOf(list subject, any element)`<br>`indexOf(map subject, any value)`](./Functions/IndexOf.md)                                        |         |             |
| [`contains(string subject, string substring)`<br>`contains(list subject, any value)`<br>`contains(map subject, any value)`](./Functions/Contains.md)                                      |         |             |
| [`startsWith(string subject, string prefix)`](./Functions/StartsWith.md)                                                                                                                  |         |             |
| [`endsWith(string subject, string suffix)`](./Functions/EndsWith.md)                                                                                                                      |         |             |
| [`converted(list subject, function callback)`](./Functions/Converted.md)                                                                                                                  |         |             |
| [`filtered(list subject, function callback)`](./Functions/Filtered.md)                                                                                                                    |         |             |
| [`reversed(list subject)`](./Functions/Reversed.md)                                                                                                                                       |         |             |
| [`sorted(list list, function compare = compareTo)`](./Functions/Sorted.md)                                                                                                                |         |             |
| [`compareTo(number a, number b)`<br>`compareTo(string a, string b)`<br>`compareTo(bool a, bool b)`<br>`compareTo(list a, list b)`<br>`compareTo(map a, map b)`](./Functions/CompareTo.md) |         |             |
| [`sin(number x)`](./Functions/Sin.md)<br>[`cos(number x)`](./Functions/Cos.md)<br>[`tan(number x)`](./Functions/Tan.md)                                                                   |         |             |
| [`min(number ...x)`](./Functions/Min.md)<br>[`max(number ...x)`](./Functions/Max.md)                                                                                                      |         |             |
| [`clamp(number min, number max, number x)`](./Functions/Clamp.md)                                                                                                                         |         |             |
| [`random()`](./Functions/Random.md)                                                                                                                                                       |         |             |
| [`interpolate(number a, number b, number t)`](./Functions/Interpolate.md)                                                                                                                 |         |             |
| [`abs(number x)`](./Functions/Abs.md)                                                                                                                                                     |         |             |
| [`floor(number x)`](./Functions/Floor.md)                                                                                                                                                 |         |             |
| [`ceil(number x)`](./Functions/Ceil.md)                                                                                                                                                   |         |             |
| [`round(number x)`](./Functions/Round.md)                                                                                                                                                 |         |             |
| [`sqrt(number x)`](./Functions/Sqrt.md)                                                                                                                                                   |         |             |
| [`print(any ...value)`](./Functions/Print.md)                                                                                                                                             |         |             |
| [`println(any ...value)`](./Functions/Println.md)                                                                                                                                         |         |             |
| [`readline()`](./Functions/Readline.md)                                                                                                                                                   |         |             |
| [`read()`](./Functions/Read.md)                                                                                                                                                           |         |             |
| [`clone(any subject)`](./Functions/Clone.md)                                                                                                                                              |         |             |

## Converting
