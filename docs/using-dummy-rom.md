# Using dummy ROM file

You can import `.fake` files as games where which contains valid PS2 game serial which you want to spoof.

## Format

```
!#PS2_GAME
SCES50760
```

* File must contain exact header `!#PS2_GAME` as first line
* Second line contains your serial code.

## Example

See [`game.fake`](../game.fake) for an example dummy file.
