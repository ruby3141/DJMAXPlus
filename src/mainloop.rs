use crate::configs::DjmaxPlusConfig;
use crate::helper;
use std::{borrow::Borrow, collections::VecDeque, path::PathBuf};

use hudhook::{util, ImguiRenderLoop};
use imgui::{
    sys::{igSetNextWindowPos, ImGuiCond, ImGuiCond_Once, ImVec2}, Ui
};

pub struct DjmaxPlusMainLoop {
    config: DjmaxPlusConfig,
    current_lanecover_index: u8,
    is_noti_popup_opened: bool,
    notifications: VecDeque<String>,
    dll_path: Option<PathBuf>,
}

impl DjmaxPlusMainLoop {
    pub fn new() -> Self {
        Self {
            config: DjmaxPlusConfig::new(),
            current_lanecover_index: 0,
            is_noti_popup_opened: false,
            notifications: VecDeque::new(),
            dll_path: util::get_dll_path(),
        }
    }

    fn load_config(&mut self) -> Result<(), String> {
        let dll_path_ref = self
            .dll_path
            .borrow()
            .as_ref()
            .ok_or("dll_path is None. You shouldn't be here.".to_string())?;
        self.config = DjmaxPlusConfig::load(helper::config_path(dll_path_ref))?;
        Ok(())
    }
}

impl ImguiRenderLoop for DjmaxPlusMainLoop {
    fn initialize<'a>(
        &'a mut self,
        _ctx: &mut imgui::Context,
        _loader: hudhook::TextureLoader<'a>,
    ) {
        // Load config and Lanecover Image
        match &self.dll_path {
            None => self.notifications.push_back(
                "Cannot obtain dll path.\n\
                Default config will be loaded. Load/save disabled."
                    .to_string(),
            ),
            Some(_) => match self.load_config() {
                Err(msg) => self.notifications.push_back(msg),
                _ => (),
            },
        }
    }

    fn before_render(&mut self, ctx: &mut imgui::Context) {
        if self.is_noti_popup_opened {
            ctx.io_mut().mouse_draw_cursor = true;
        }
    }

    fn render(&mut self, ui: &mut Ui) {
        if !self.is_noti_popup_opened && !self.notifications.is_empty() {
            self.is_noti_popup_opened = true;
            ui.open_popup("notification_popup")
        }
        // Inevitable unsafe call to set popup position
        // 5 years old problem which hasn't been fixed yet
        // https://github.com/imgui-rs/imgui-rs/issues/201
        unsafe {
            igSetNextWindowPos(
                ImVec2::new(500., 500.),
                ImGuiCond_Once as ImGuiCond,
                ImVec2::new(0., 0.),
            )
        }
        ui.popup("notification_popup", || {
            ui.text(self.notifications.front().unwrap());
            if ui.button("Ok") {
                self.is_noti_popup_opened = false;
                self.notifications.pop_front();
                ui.close_current_popup();
            }
        });
    }
}
