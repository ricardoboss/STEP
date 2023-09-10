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

## Converting
