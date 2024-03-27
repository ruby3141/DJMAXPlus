use super::ImguiKeyProxy;

use imgui::Key;
use serde::{Deserialize, Serialize};
use serde_with::serde_as;

#[serde_as]
#[derive(Serialize, Deserialize)]
pub struct HotkeyConfig {
    #[serde(with="ImguiKeyProxy")]
    pub config_editor: Key,
    #[serde_as(as = "[ImguiKeyProxy; 4]")]
    pub load_config: [Key; 4],
}

impl HotkeyConfig {
    pub fn new() -> Self {
        Self {
            config_editor: Key::F12,
            load_config: [Key::Z, Key::X, Key::C, Key::V],
        }
    }
}
