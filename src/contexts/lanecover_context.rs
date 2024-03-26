use super::runtime_context::RuntimeContext;
use crate::{common::LaneDirection, configs::*};

use imgui::{StyleVar, WindowFlags};

pub struct LanecoverContext {
    config_index: usize,
}

impl LanecoverContext {
    pub fn new() -> Self {
        Self { config_index: 0 }
    }

    pub fn render(
        &mut self,
        ui: &mut imgui::Ui,
        runtime: &RuntimeContext,
        config: &DjmaxplusConfig,
    ) {
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
                    (runtime.viewport_effective_offset_h()
                        + match &config.lane_direction {
                            LaneDirection::Left => runtime.viewport_effective_width() * 0.041,
                            LaneDirection::Center => runtime.viewport_effective_width() * 0.375,
                            LaneDirection::Right => runtime.viewport_effective_width() * 0.709,
                        })
                    .floor(),
                    0.,
                ],
                imgui::Condition::Always,
            )
            .size(
                [
                    runtime.viewport_effective_width() / 4., // 25% of viewport width, with forced 16:9 screen aspect ratio
                    (runtime.viewport_height()
                        * config.lanecover_configs[self.config_index].visible_ratio as f32
                        / 1000.)
                        .floor(),
                ],
                imgui::Condition::Always,
            )
            .build(|| {});
        style_lanecover_windowpadding.pop();
        style_lanecover_windowbordersize.pop();
    }
}
