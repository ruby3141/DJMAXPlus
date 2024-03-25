use std::fs;
use std::path::Path;

use crate::{common::LaneDirection, configs::LanecoverConfig};
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub struct DjmaxPlusConfig {
    pub lane_direction: LaneDirection,
    pub lane_offset: i8,
    pub lanecover_visible: bool,
    pub lanecover_configs: [LanecoverConfig; 4],
}

impl DjmaxPlusConfig {
    pub fn new() -> Self {
        Self {
            lane_direction: LaneDirection::Center,
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
