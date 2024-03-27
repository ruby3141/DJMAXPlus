use super::{
    config_editor_context::ConfigEditorContext, hotkey_context::HotkeyContext,
    lanecover_context::LanecoverContext, noti_popup_context::NotiPopupContext,
    runtime_context::RuntimeContext,
};
use crate::{configs::DjmaxplusConfig, helper};

use hudhook::ImguiRenderLoop;
use imgui::Ui;

pub struct DjmaxplusContext {
    config: DjmaxplusConfig,
    runtime: RuntimeContext,
    noti_popup: NotiPopupContext,
    lanecover: LanecoverContext,
    config_editor: ConfigEditorContext,
    hotkey: HotkeyContext,
}

impl DjmaxplusContext {
    pub fn new() -> Self {
        Self {
            config: DjmaxplusConfig::new(),
            runtime: RuntimeContext::new(),
            noti_popup: NotiPopupContext::new(),
            lanecover: LanecoverContext::new(),
            config_editor: ConfigEditorContext::new(),
            hotkey: HotkeyContext::new(),
        }
    }

    fn load_config(&mut self) -> Result<(), String> {
        self.config = DjmaxplusConfig::load(helper::config_path(
            self.runtime
                .dll_path()
                .as_ref()
                .ok_or("dll_path is None. You shouldn't be here.".to_string())?,
        ))?;

        Ok(())
    }
}

impl ImguiRenderLoop for DjmaxplusContext {
    fn initialize<'a>(
        &'a mut self,
        _ctx: &mut imgui::Context,
        _loader: hudhook::TextureLoader<'a>,
    ) {
        // Load config and Lanecover Image
        match &self.runtime.dll_path() {
            None => self.noti_popup.push_notification(
                "Cannot obtain dll path.\n\
                Default config will be loaded. Load/save disabled."
                    .to_string(),
            ),
            Some(_) => match self.load_config() {
                Err(msg) => self.noti_popup.push_notification(msg),
                _ => (),
            },
        }

        self.noti_popup
            .push_notification("DJMAXPlus is now operational.".to_string());
    }

    fn before_render(&mut self, ctx: &mut imgui::Context) {
        if self.noti_popup.is_presenting() {
            ctx.io_mut().mouse_draw_cursor = true;
        }

        // Any context which wants keyboard to be captured should overwrite it on every frame.
        ctx.io_mut().want_capture_keyboard = false;

        self.hotkey.before_render(
            ctx,
            &mut self.config_editor,
            &mut self.lanecover,
            &mut self.config,
        );
        self.config_editor.before_render(ctx);
        self.runtime.before_render(ctx);
    }

    fn render(&mut self, ui: &mut Ui) {
        self.noti_popup.render(ui);
        self.lanecover.render(ui, &self.runtime, &self.config);
        self.config_editor
            .render(ui, &mut self.config, &mut self.lanecover, &self.runtime);
    }
}
