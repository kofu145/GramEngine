


```cs
test
```


Quick notes to self:

Inputs handled by gamestate, top of the stack will have processed inputs (can buffer and act upon them every frame)

setsmoothing is equivalent to bilinear (if set to false, it's point)

Need to clear up ``Initialize`` and ``OnLoad`` differences.\
Initialize: on load of the scene
OnLoad: On load of the game (if possible)


game settings:
pause on resize/when out of focus
width/height < make a manager class for this too!

Merge gamestate/scenes?\
Get rid of singletons? (just have static classes...?)