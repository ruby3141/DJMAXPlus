mod djmaxplus_config;
mod lanecover_config;
mod hotkey_config;
mod config_enums;

pub use self::djmaxplus_config::DjmaxplusConfig;
pub use self::lanecover_config::LanecoverConfig;
pub use self::hotkey_config::HotkeyConfig;
pub use self::config_enums::*;

#[cfg(test)]
mod tests;
