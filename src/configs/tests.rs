use crate::{
    common::LaneDirection,
    configs::DjmaxplusConfig,
};
use std::borrow::Borrow;

use hudhook::util;

#[test]
fn generate_and_load() {
    let config = DjmaxplusConfig::new();
    let path = crate::helper::config_path(Borrow::borrow(
        &util::get_dll_path().unwrap(),
    ));
    config.save(path.clone()).unwrap();

    let config = DjmaxplusConfig::load(path).unwrap();
    assert!(matches!(
        config.lane_direction,
        LaneDirection::Center
    ));
    assert_eq!(config.lane_offset, 0);
    assert_eq!(config.lanecover_visible, true);
    for lc in config.lanecover_configs.iter() {
        assert_eq!(lc.visible_ratio, 200);
        assert_eq!(lc.djmax_target_speed, 25);
    }
}