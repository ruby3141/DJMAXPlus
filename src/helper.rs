use std::path::PathBuf;

pub fn config_path(dll_path: &PathBuf) -> PathBuf {
    let mut path = dll_path.clone();
    path.pop();
    path.push("config.yaml");
    path
}