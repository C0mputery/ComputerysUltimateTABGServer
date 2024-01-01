# Packets Documentation

## Overview
- Ticks are effectivly used for things like game logic.
- Made to be expandable at runtime for future plugin support.

## [Tick Manager](TickManager.cs)
- The Tick manager is the main class that handles ticks for rooms.

## [Tick Types](TickTypes)
- TickTypes holds the functions that are used to handle the different types of ticks that will be run when the room is updated/
- TickTypes is a partal class broken up into many files to make it easier to read and edit.

## What makes a tick
Using the TickHandlerDelegate delegate.