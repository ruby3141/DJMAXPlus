import { app, BrowserWindow, screen } from 'electron'
import overlay from 'electron-overlay'

app.disableHardwareAcceleration()
app.whenReady().then(() =>
{
	const anchorWindow = new BrowserWindow(
		{
			width: 380,
			height: 220,
		})

	anchorWindow.on('closed', () =>
	{
		overlay.stop()
		app.exit()
	})

	anchorWindow.loadFile('index.html')

	overlay.start()

	overlay.setHotkeys([
		{
			name: 'please.close.yourself',
			keyCode: 0x73, //VK_F4
			modifiers: { alt: true },
		},
		{
			name: 'overlay.hotkey.toggleInputIntercept',
			keyCode: 0x71, //VK_F2
			modifiers: { ctrl: true },
		},
		{
			name: 'djmaxplus.hidelanecover',
			keyCode: 0x71, //VK_F2
			modifiers: {},
		},
		{
			name: 'djmaxplus.saveslot.z',
			keyCode: 0x5A, //VK_Z
			modifiers: {},
		},
		{
			name: 'djmaxplus.saveslot.x',
			keyCode: 0x58, //VK_X
			modifiers: {},
		},
		{
			name: 'djmaxplus.saveslot.c',
			keyCode: 0x43, //VK_C
			modifiers: {},
		},
		{
			name: 'djmaxplus.saveslot.v',
			keyCode: 0x56, //VK_V
			modifiers: {},
		},
	])

	var overlayWindow = new BrowserWindow(
		{
			x: 0,
			y: 0,
			width: 800,
			height: 400,
			frame: false,
			transparent: false,
			show: false,
			backgroundColor: '#00000000',
			webPreferences:
			{
				offscreen: true,
			}
		})

	const display = screen.getDisplayNearestPoint(screen.getCursorScreenPoint())
	overlay.addWindow(
		overlayWindow.id,
		{
			name: 'MainOverlay',
			transparent: false,
			resizable: overlayWindow.isResizable(),
			maxWidth: overlayWindow.isResizable() ? display.bounds.width : overlayWindow.getBounds().width,
			minWidth: overlayWindow.isResizable() ? 100 : overlayWindow.getBounds().width,
			maxHeight: overlayWindow.isResizable() ? display.bounds.height : overlayWindow.getBounds().height,
			minHeight: overlayWindow.isResizable() ? 100 : overlayWindow.getBounds().height,
			nativeHandle: overlayWindow.getNativeWindowHandle().readUInt32LE(0),
			rect:
			{
				x: overlayWindow.getBounds().x,
				y: overlayWindow.getBounds().y,
				width: overlayWindow.getBounds().width,
				height: overlayWindow.getBounds().height
			},
			caption:
			{
				left: 0,
				right: 0,
				top: 0,
				height: 0
			},
			dragBorderWidth: 0
		})

	overlayWindow.webContents.on('paint', (event, dirty, image) =>
	{
		overlay.sendFrameBuffer(overlayWindow.id, image.getBitmap(), image.getSize().width, image.getSize().height)
	})

	overlayWindow.on("ready-to-show", () =>
	{
		overlayWindow.focusOnWebView();
	});

	overlayWindow.on('resize', () =>
	{
		overlay.sendWindowBounds(overlayWindow.id,
			{
				rect:
				{
					x: overlayWindow.getBounds().x,
					y: overlayWindow.getBounds().y,
					width: overlayWindow.getBounds().width,
					height: overlayWindow.getBounds().height
				}
			})
	})

	overlayWindow.loadFile('lanecover.html')

	overlay.setEventCallback((event, ...args) =>
	{
		switch (event)
		{
			case 'graphics.window':
				overlayWindow.setSize(args[0].width, args[0].height)
				break
			case 'graphics.window.event.resize':
				overlayWindow.setSize(args[0].width, args[0].height)
				break
			case 'game.input.intercept':
				overlayWindow.webContents.executeJavaScript('document.querySelector(\"#_dp_LaneCover\").style.maxHeight = \"100%\"')
				overlayWindow.webContents.executeJavaScript(
					`document.querySelector(\"#_dp_Config\").style.visibility = ${args[0].intercepting ? '\"visible\"' : '\"hidden\"'}`)
				break
			case 'game.input':
				const window = BrowserWindow.fromId(args[0].windowId);
				if (window)
				{
					const inputEvent = overlay.translateInputEvent(args[0]);
					window.webContents.sendInputEvent(inputEvent);
				}
				break
			case 'game.hotkey.down':
				switch(args[0].name)
				{
					case 'djmaxplus.hidelanecover':
						overlayWindow.webContents.executeJavaScript('document.querySelector(\"#_dp_LaneCover\").style.maxHeight = \"0%\"')
						break
					case 'djmaxplus.saveslot.z':
					case 'djmaxplus.saveslot.x':
					case 'djmaxplus.saveslot.c':
					case 'djmaxplus.saveslot.v':
						overlayWindow.webContents.executeJavaScript('document.querySelector(\"#_dp_LaneCover\").style.maxHeight = \"100%\"')
						overlayWindow.webContents.executeJavaScript(
							`document.querySelector(\"#_dp_Config form\")[\"SaveSlot\"].value = \"${args[0].name.slice(-1)}\"; loadConfig();`)
						break
					case 'please.close.yourself':
						overlay.stop()
						app.exit()
						break
				}
			default:
				break
		}
	})

	for (let window of overlay.getTopWindows())
	{
		if (window.title === 'DJMAX RESPECT V')
		{
			console.log(overlay.injectProcess(window))
		}
	}
})