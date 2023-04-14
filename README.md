# Adumbration

## Credits:
Adumbration is a game developed by Julian Alvia, Scott Au Yeung, Alex Gough, and Nikki Murello over the course of spring semester 2023 as a part of the course IGME 106.

## Screenshots:
We'll add later

## Level layouts:
HI! this is a guide for what the different symbols mean and how to make a level layout file!

First make sure the text file is in THIS directory, and load the file
in the level class by just using the text file's name (e.g. "levelFile.txt")

All characters/numbers in the file should be separated by a comma (",")

In the file make the first line contain the size of the level tiles (15,15),
and then make the layout in all following lines. There will be errors/messed 
up loading if the size of the size of the written out layout is different from
the top two numbers.

Characters & meanings:
0: wall
_: floor
E: emitter
R: reciever
S: spawn point
/: forward facing mirror
\: backward facing mirror
D: door

We'll add more onto this the more we use the layouts