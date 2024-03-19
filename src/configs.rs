use std::fs;
use std::path::Path;

use crate::common::LaneDirection;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub struct DjmaxPlusConfig {
    lane_direction: LaneDirection,
    lane_offset: i8,
    lanecover_visible: bool,
    lanecover_configs: [LanecoverConfig; 4],
}

#[derive(Clone, Copy, Serialize, Deserialize)]
pub struct LanecoverConfig {
    visible_ratio: u16,
    djmax_target_speed: u8,
}

impl DjmaxPlusConfig {
    pub fn new() -> Self {
        Self {
            lane_direction: LaneDirection::CENTER,
            lane_offset: 0,
            lanecover_visible: true,
            lanecover_configs: [LanecoverConfig::new(); 4],
        }
    }

    pub fn load<T>(path: T) -> Result<Self, String>
    where
        T: AsRef<Path>,
    {
        let config =
            fs::read_to_string(path).map_err(|e| format!("Failed to read config file: {:?}", e))?;

        Ok(serde_yaml::from_str(config.as_str())
            .map_err(|e| format!("Failed to parse config file: {:?}", e))?)
    }

    pub fn save<T>(&self, path: T) -> Result<(), String>
    where
        T: AsRef<Path>,
    {
        let config = serde_yaml::to_string(self).map_err(|e| format!("Failed to save: {:?}", e))?;
        fs::write(path, config).map_err(|e| format!("Failed to save: {:?}", e))?;
        Ok(())
    }
}

impl LanecoverConfig {
    pub fn new() -> Self {
        Self {
            visible_ratio: 200,
            djmax_target_speed: 25,
        }
    }
}

#[test]
fn generate_and_load() {
    let config = DjmaxPlusConfig::new();
    let path = crate::helper::config_path(std::borrow::Borrow::borrow(
        &hudhook::util::get_dll_path().unwrap(),
    ));
    config.save(path.clone()).unwrap();

    let config = DjmaxPlusConfig::load(path).unwrap();
    assert!(matches!(
        config.lane_direction,
        crate::common::LaneDirection::CENTER
    ));
    assert_eq!(config.lane_offset, 0);
    assert_eq!(config.lanecover_visible, true);
    for lc in config.lanecover_configs.iter() {
        assert_eq!(lc.visible_ratio, 200);
        assert_eq!(lc.djmax_target_speed, 25);
    }
}
