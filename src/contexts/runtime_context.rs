use std::path::PathBuf;

use hudhook::util;

pub struct RuntimeContext {
    dll_path: Option<PathBuf>,
    viewport_width: f32,
    viewport_height: f32,
}

impl RuntimeContext {
    /// get cached value of this payload's path
    pub fn dll_path(&self) -> &Option<PathBuf> {
        &self.dll_path
    }

    /// get true width of current Viewport
    pub fn viewport_width(&self) -> &f32 {
        &self.viewport_width
    }

    /// get width of current effective Viewport,
    /// which is 16:9 ratio area having the same height of current Viewport.
    pub fn viewport_effective_width(&self) -> f32 {
        (&self.viewport_height * 16. / 9.).floor()
    }

    /// get horizontal offset toward effective Viewport
    pub fn viewport_effective_offset_h(&self) -> f32 {
        ((self.viewport_width() - self.viewport_effective_width()) / 2.).floor()
    }

    /// get height of current Viewport
    pub fn viewport_height(&self) -> &f32 {
        &self.viewport_height
    }
}

impl RuntimeContext {
    pub fn new() -> Self {
        Self {
            dll_path: util::get_dll_path(),
            viewport_width: 0.,
            viewport_height: 0.,
        }
    }

    pub fn before_render(&mut self, ctx: &mut imgui::Context) {
        match ctx.main_viewport().size {
            [width, height] => {
                self.viewport_width = width;
                self.viewport_height = height;
            }
        }
    }
}
