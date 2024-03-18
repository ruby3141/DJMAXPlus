use hudhook::{hooks::dx11::ImguiDx11Hooks, ImguiRenderLoop};
use imgui::Ui;

struct DPOverlay;

impl DPOverlay {
    fn new() -> Self {
        Self
    }
}

impl ImguiRenderLoop for DPOverlay {
    fn render(&mut self, ui: &mut Ui) {

    }
}

// DLL entry point
hudhook::hudhook!(ImguiDx11Hooks, DPOverlay::new());
