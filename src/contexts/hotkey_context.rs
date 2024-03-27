use imgui::Context;

use crate::configs::DjmaxplusConfig;

use super::{config_editor_context::ConfigEditorContext, lanecover_context::LanecoverContext};

pub struct HotkeyContext {
    last_editor_hotkey_pressed: f64,
    editor_hotkey_doubletap_state: DoubletapState,
}

const EDITOR_HOTKEY_DOUBLETAP_WINDOW: f64 = 0.35;

#[derive(PartialEq)]
enum DoubletapState {
    Wait,
    FirstDown,
    FirstUp,
    SecondDown,
}

impl HotkeyContext {
    pub fn new() -> Self {
        Self {
            last_editor_hotkey_pressed: f64::MIN,
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

        if ![DoubletapState::Wait, DoubletapState::SecondDown]
            .contains(&self.editor_hotkey_doubletap_state)
        {
            if &self.last_editor_hotkey_pressed + EDITOR_HOTKEY_DOUBLETAP_WINDOW < now {
                config_editor.visible ^= true; // flip
                self.editor_hotkey_doubletap_state = DoubletapState::Wait;
            }
        }

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
            DoubletapState::FirstUp => {
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
}
