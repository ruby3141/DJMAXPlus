use crate::{common::LaneDirection::*, configs::DjmaxPlusConfig, helper};
use std::{borrow::Borrow, collections::VecDeque, path::PathBuf};

use hudhook::{util, ImguiRenderLoop};
use imgui::{
    sys::{igSetNextWindowPos, ImGuiCond, ImGuiCond_Once, ImVec2},
    StyleVar, Ui, WindowFlags,
};

pub struct DjmaxPlusMainLoop {
    config: DjmaxPlusConfig,

    viewport_width: f32,
    viewport_height: f32,
    current_lanecover_index: usize,

    is_noti_popup_opened: bool,
    notifications: VecDeque<String>,

    dll_path: Option<PathBuf>,
}

impl DjmaxPlusMainLoop {
    pub fn new() -> Self {
        Self {
            config: DjmaxPlusConfig::new(),
            viewport_width: 0.,
            viewport_height: 0.,
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

        self.notifications
            .push_back("DJMAXPlus is now operational.".to_string());
    }

    fn before_render(&mut self, ctx: &mut imgui::Context) {
        if self.is_noti_popup_opened {
            ctx.io_mut().mouse_draw_cursor = true;
        }

        let [width, height] = ctx.main_viewport().size;
        self.viewport_width = width;
        self.viewport_height = height;
    }

    fn render(&mut self, ui: &mut Ui) {
        // Notification popup
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

        // Lanecover
        let style_lanecover_windowbordersize = ui.push_style_var(StyleVar::WindowBorderSize(0.));
        let style_lanecover_windowpadding = ui.push_style_var(StyleVar::WindowPadding([0., 0.]));
        ui.window("lanecover")
            .flags(WindowFlags::from_iter([
                WindowFlags::NO_DOCKING,
                WindowFlags::NO_DECORATION,
                WindowFlags::NO_INPUTS,
                WindowFlags::NO_SCROLL_WITH_MOUSE,
                WindowFlags::NO_MOVE,
            ]))
            .position(
                [
                    match self.config.lane_direction {
                        // To be filled
                        Left => 0.,
                        Center => 0.,
                        Right => 0.,
                    },
                    0.,
                ],
                imgui::Condition::Always,
            )
            .size(
                [
                    (self.viewport_height * 4. / 9.).floor(), // 25% of viewport width, with forced 16:9 screen aspect ratio
                    (self.viewport_height
                        * self.config.lanecover_configs[self.current_lanecover_index].visible_ratio
                            as f32
                        / 1000.)
                        .floor(),
                ],
                imgui::Condition::Always,
            )
            .build(|| {
                todo!();
            });
        style_lanecover_windowpadding.pop();
        style_lanecover_windowbordersize.pop();
    }
}
