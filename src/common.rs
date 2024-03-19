use serde::{Serialize, Deserialize};

#[derive(Serialize, Deserialize)]
pub enum LaneDirection {
    LEFT,
    CENTER,
    RIGHT
}
