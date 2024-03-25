use serde::{Serialize, Deserialize};

#[derive(Serialize, Deserialize)]
pub enum LaneDirection {
    Left,
    Center,
    Right
}
