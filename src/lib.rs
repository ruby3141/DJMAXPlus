mod contexts;
mod common;
mod configs;
mod helper;

use hudhook::hooks::dx11::ImguiDx11Hooks;

// DLL entry point
hudhook::hudhook!(ImguiDx11Hooks, contexts::DjmaxplusContext::new());
