use imgui::{Context, Ui};

use crate::configs::DjmaxplusConfig;

use super::{config_editor_context::ConfigEditorContext, lanecover_context::LanecoverContext};

pub struct HotkeyContext {
    last_editor_hotkey_pressed: f64,
    editor_hotkey_doubletap_state: DoubletapState,
}

const EDITOR_HOTKEY_DOUBLETAP_WINDOW: f64 = 3.;

#[derive(PartialEq, Debug)]
enum DoubletapState {
    Wait,
    FirstDown,
    FirstUp,
    SecondDown,
}

impl HotkeyContext {
    pub fn new() -> Self {
        Self {
            last_editor_hotkey_pressed: 0.,
            editor_hotkey_doubletap_state: DoubletapState::Wait,
        }
    }

    pub fn before_render(
        &mut self,
        ctx: &mut Context,
        config_editor: &mut ConfigEditorContext,
        lanecover: &mut LanecoverContext,
        config: &mut DjmaxplusConfig,
    ) {
        let now = ctx.time();

        match self.editor_hotkey_doubletap_state {
            DoubletapState::Wait => {
                if ctx.io().keys_down[config.hotkey_config.config_editor as usize] {
                    self.last_editor_hotkey_pressed = now.clone();
                    self.editor_hotkey_doubletap_state = DoubletapState::FirstDown;
                }
            }
            DoubletapState::FirstDown => {
                if !ctx.io().keys_down[config.hotkey_config.config_editor as usize] {
                    self.editor_hotkey_doubletap_state = DoubletapState::FirstUp;
                }
            }
            DoubletapState::FirstUp => 'fu: {
                if &self.last_editor_hotkey_pressed + EDITOR_HOTKEY_DOUBLETAP_WINDOW < now {
                    config_editor.visible ^= true; // flip
                    self.editor_hotkey_doubletap_state = DoubletapState::Wait;
                    break 'fu;
                }

                if ctx.io().keys_down[config.hotkey_config.config_editor as usize] {
                    config.lanecover_visible ^= true; // flip
                    self.editor_hotkey_doubletap_state = DoubletapState::SecondDown;
                }
            }
            DoubletapState::SecondDown => {
                if !ctx.io().keys_down[config.hotkey_config.config_editor as usize] {
                    self.editor_hotkey_doubletap_state = DoubletapState::Wait;
                }
            }
        }

        for (index, load_key) in config.hotkey_config.load_config.iter().enumerate() {
            if ctx.io().keys_down[load_key.clone() as usize] {
                lanecover.config_index = index;
            }
        }
    }

    pub fn render(&self, ui: &mut Ui) {
        ui.window("hotkey_context_debug")
            .build(||{
                ui.text(format!("framerate: {}", ui.io().framerate));
                ui.text(format!("now: {}", ui.time()));
                ui.text(format!("last input: {}", self.last_editor_hotkey_pressed));
                ui.text(format!("state: {:?}", self.editor_hotkey_doubletap_state));
            });
    }
}
