use serde::{Deserialize, Serialize};

#[derive(Clone, Copy, Serialize, Deserialize)]
pub struct LanecoverConfig {
    pub visible_ratio: u16,
    pub djmax_target_speed: u8,
}

impl LanecoverConfig {
    pub fn new() -> Self {
        Self {
            visible_ratio: 200,
            djmax_target_speed: 25,
        }
    }
}
