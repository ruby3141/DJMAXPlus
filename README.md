# DJMAXPlus

Lang: **EN** / [KO](README.ko.md)

use Sudden+ in DJMAX, and maybe more?

![Preview](https://github.com/ruby3141/readme-assets/blob/main/DJMAXPlus/preview.gif?raw=true)

## How to use

1. Start DJMAX RESPECT V. Wait until it shows epilepsy warning.
2. Start DJMAXPlus and switch back to the game.
3. Lane cover overlay will poke out from top.

### Hotkeys

- F2: Hide Lane Cover (to unhide, do Open Config or Load Config.)
- Ctrl + F2 : Open Config
- Z/X/C/V: Load Config 1/2/3/4

Alt+F4 will close both DJMAX and DJMAXPlus.

### Lane Cover Image

You can put `cover.png` or `cover.jpg` on `./resources/app` directory to load image on lane cover.

### etc.

Please note that due to the limitation of GOverlay, restarting DJMAXPlus will make it project overlay in wrong resolution. <br>
You can fix it by changing game's resolution option, or simply restart both DJMAX and DJMAXPlus.

## Dependencies

- copyfiles: ^2.4.1
- electron: ^28.1.0
- electron-builder: ^24.9.1
- typescript: ^5.3.3
- GOverlay: please read [Before start development](#Before-start-development)

## Before start development

GOverlay is node native module and it isn't on npm.

1. clone this repo and https://github.com/hiitiger/goverlay/. <br>
   make sure both have same parent directory.
2. copy-paste goverlay/game-overlay/prebuilt/injector_helper.x64.exe into goverlay/electron-overlay
3. from https://github.com/ruby3141/goverlay-dll/releases/, download n_overlay.x64.dll and save into goverlay/electron-overlay
4. `npm install`. I haven't tested `yarn`, but I think it would work.
