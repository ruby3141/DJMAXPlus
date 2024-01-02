# DJMAXPlus

use Sudden+ in DJMAX, and maybe more?

### Dependencies

- electron: ^28.1.0
- typescript: ^5.3.3
- GOverlay: please read [To start development](#to-start-development)

### Before start development

GOverlay is node native module and it isn't on npm.

1. clone this repo and https://github.com/hiitiger/goverlay/. <br>
   make sure both have same parent directory.
2. copy-paste goverlay/game-overlay/prebuilt/injector_helper.x64.exe into goverlay/electron-overlay
3. from https://github.com/ruby3141/goverlay-dll/releases/, download n_overlay.x64.dll and save into goverlay/electron-overlay
4. `npm install`. I haven't tested `yarn`, but I think it would work.