use super::{lanecover_context::LanecoverContext, runtime_context::RuntimeContext};
use crate::configs::DjmaxplusConfig;

use imgui::{Condition, Context, Ui, WindowFlags};

pub struct ConfigEditorContext {
    pub visible: bool,
}

impl ConfigEditorContext {
    pub fn new() -> Self {
        Self { visible: false }
    }

    pub fn before_render(&self, ctx: &mut Context) {
        if self.visible {
            ctx.io_mut().want_capture_keyboard = true;
        }
    }

    pub fn render(
        &mut self,
        ui: &mut Ui,
        config: &mut DjmaxplusConfig,
        lanecover: &mut LanecoverContext,
        runtime: &RuntimeContext,
    ) {
        if self.visible {
            ui.window("config_editor")
                .flags(WindowFlags::from_iter([
                    WindowFlags::NO_DOCKING,
                    WindowFlags::NO_COLLAPSE,
                    WindowFlags::NO_RESIZE,
                    WindowFlags::NO_MOVE,
                ]))
                .position([0.0, 0.0], Condition::Always)
                .build(|| {
                });
        }
    }
}
