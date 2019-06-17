# CleaningRobot
Cleaning Robot Exercise

For using it, you need to use the content inside "Executable" folder.
The application was made for running in a windows environment.

Before running you need to add an input file json inside "Executable" folder. Also, an output file with json extension.

The input file needs to have the next structure. In other way, it don't work.

```json
{
  "map": [
    ["S", "S", "S", "S"],
    ["S", "S", "C", "S"],
    ["S", "S", "S", "S"],
    ["S", "null", "S", "S"]
  ],
  "start": {"X": 3, "Y": 0, "facing": "N"},
  "commands": [ "TL","A","C","A","C","TR","A","C"],
  "battery": 80
}
```
